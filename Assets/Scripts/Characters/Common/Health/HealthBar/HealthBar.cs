using System;
using UnityEngine;

namespace Characters.Health.HealthBar
{
    [ExecuteAlways]
    public class HealthBar : MonoBehaviour, IHealthBar, IDisposable
    {
        #region Fields 

        #region Inspector

        [Header("Actual Hp - Settings")]
        [SerializeField] private Color _actualHpColor = Color.red;

        [Space, Header("Background - Settings")]
        [SerializeField] private Color _backgroundColor = Color.black;

        #endregion

        private Vector2 _initialScale;
        private bool _isVisible = true;
        private CharacterBodyBase _characterBody;
        private HealthBarHp _hpVisual;
        private HealthBarBackground _backgroundVisual;

        #endregion


        #region Methods 

        #region Init 

        private void Awake()
        {
            _initialScale = transform.localScale;
            _characterBody = GetComponentInParent<CharacterBodyBase>();
            _hpVisual = GetComponentInChildren<HealthBarHp>();
            _backgroundVisual = GetComponentInChildren<HealthBarBackground>();
        }

        private void Start()
        {
            ConfigureBarVisual();

            if (Application.isPlaying)
            {
                _characterBody.View.OnSideSwitch += CharacterScaleChangedListener;
                _characterBody.Health.OnCurrentHpPercentChanged += UpdateActualHp;
                Hide();
            }
        }

        private void Update()
        {

            // ToDo : Remove this input (it is for tests only)
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.P))
            {
                Show();
            }
#endif
        }

        private void CharacterScaleChangedListener(object sender, EventArgs args)
        {
            _initialScale.x *= -1;
            transform.localScale = _initialScale;
        }

        private void ConfigureBarVisual()
        {
            if (_hpVisual != null)
            {
                ConfigureHpVisual();
            }

            if (_backgroundVisual != null)
            {
                ConfigureBackgroundVisual();
            }
        }

        private void ConfigureHpVisual()
        {
            _hpVisual.Renderer.color = _actualHpColor;
        }

        private void ConfigureBackgroundVisual()
        {
            _backgroundVisual.Renderer.color = _backgroundColor;
        }

        #endregion

        public void Hide()
        {
            _hpVisual.Hide();
            _backgroundVisual.Hide();
            _isVisible = false;
        }

        public void Show()
        {
            _hpVisual.Show();
            _backgroundVisual.Show();
            _isVisible = true;
        }

        public void UpdateActualHp(float actualHpPercent)
        {
            if(!_isVisible)
            {
                Show();
            }

            actualHpPercent = Mathf.Clamp(actualHpPercent, 0, 100);
            _hpVisual.UpdateActualHp(actualHpPercent);
        }

        public void Dispose()
        {
            _characterBody.View.OnSideSwitch -= CharacterScaleChangedListener;
            _characterBody.Health.OnCurrentHpPercentChanged -= UpdateActualHp;
        }

        private void OnValidate()
        {
            ConfigureBarVisual();
        }

        #endregion
    }
}