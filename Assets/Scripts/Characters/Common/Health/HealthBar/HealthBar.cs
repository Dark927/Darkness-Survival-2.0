using Characters.Interfaces;
using System;
using UnityEngine;

namespace UI.Local.Health
{
    [ExecuteAlways]
    public class HealthBar : MonoBehaviour, IHealthBar
    {
        #region Fields 

        #region Inspector

        [Header("Actual Hp - Settings")]
        [SerializeField] private Color _actualHpColor = Color.red;

        [Space, Header("Background - Settings")]
        [SerializeField] private Color _backgroundColor = Color.black;

        #endregion

        private Vector2 _initialScale;
        private bool _isVisible;
        private ICharacterBody _characterBody;
        private Slider _hpVisual;
        private BackgroundSprite _backgroundVisual;

        #endregion


        #region Methods 

        #region Init 

        private void Awake()
        {
            _hpVisual = GetComponentInChildren<Slider>();
            _backgroundVisual = GetComponentInChildren<BackgroundSprite>();

            _isVisible = false;
        }

        private void OnEnable()
        {
            if (!_isVisible)
            {
                return;
            }

            ConfigureEventLinks();
        }

        public void Initialize(ICharacterBody characterBody)
        {
            _initialScale = transform.localScale;
            SetCharacter(characterBody);

            ConfigureBarVisual();
            Hide();
        }

        public void SetCharacter(ICharacterBody characterBody)
        {
            if (_characterBody != null)
            {
                RemoveEventLinks();
            }

            _characterBody = characterBody;
            ConfigureEventLinks();
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

        public void ConfigureEventLinks()
        {
            _characterBody.View.OnSideSwitch += CharacterScaleChangedListener;
            _characterBody.Health.OnCurrentHpPercentChanged += UpdateActualHp;
        }

        public void RemoveEventLinks()
        {
            _characterBody.View.OnSideSwitch -= CharacterScaleChangedListener;
            _characterBody.Health.OnCurrentHpPercentChanged -= UpdateActualHp;
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            RemoveEventLinks();
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
            if (!_isVisible)
            {
                Show();
            }

            actualHpPercent = Mathf.Clamp(actualHpPercent, 0, 100);
            _hpVisual.UpdateActualValue(actualHpPercent);
        }

        private void OnValidate()
        {
            ConfigureBarVisual();
        }

        #endregion
    }
}
