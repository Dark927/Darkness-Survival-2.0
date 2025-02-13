﻿using Characters.Common.Features;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace UI.Local.Health
{
    [ExecuteAlways]
    public class HealthBar : MonoBehaviour, IHealthBar
    {
        #region Fields 

        #region Inspector

        [Header("Settings")]
        [SerializeField] private IEntityFeature.TargetEntityPart _entityConnectionPart;

        [Header("Visual")]
        [Header("Actual Hp - Settings")]
        [SerializeField] private Color _actualHpColor = Color.red;

        [Space, Header("Background - Settings")]
        [SerializeField] private Color _backgroundColor = Color.black;

        #endregion

        private bool _isReady;
        private bool _isVisible;
        private Vector2 _initialScale;
        private IEntityBody _entityBody;
        private Slider _hpVisual;
        private BackgroundSprite _backgroundVisual;

        private UniTaskVoid _configureEventsTask;

        #endregion


        #region Properties 

        public IEntityFeature.TargetEntityPart EntityConnectionPart => _entityConnectionPart;
        public bool IsReady => _isReady;
        public GameObject RootObject => gameObject;

        #endregion


        #region Methods 

        #region Init 

        private void Awake()
        {
            _initialScale = transform.localScale;
            _isReady = false;
        }

        public void Initialize(IEntityLogic entityLogic)
        {
            _initialScale = transform.localScale;
            InitVisualParts();

            SetCharacterBody(entityLogic.Body);
            ConfigureBarVisual();
            //Hide();
            _configureEventsTask = TryConfigureEventsAsync();
        }

        public void SetCharacterBody(IEntityBody entityBody)
        {
            if (_entityBody != null)
            {
                RemoveEventLinks();
            }

            _entityBody = entityBody;

        }

        public async UniTaskVoid TryConfigureEventsAsync()
        {
            await UniTask.WaitUntil(() => _entityBody.IsReady);

            ConfigureEventLinks();
        }

        private void CharacterScaleChangedListener(object sender, EventArgs args)
        {
            _initialScale.x *= -1;
            transform.localScale = _initialScale;
        }

        private void InitVisualParts()
        {
            _hpVisual = GetComponentInChildren<Slider>();
            _backgroundVisual = GetComponentInChildren<BackgroundSprite>();

            _hpVisual.Initialize();
            _backgroundVisual.Initialize();
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
            _entityBody.OnBodyDies += Hide;
            _entityBody.View.OnSideSwitch += CharacterScaleChangedListener;
            _entityBody.Health.OnCurrentHpPercentChanged += UpdateActualHp;
        }

        public void RemoveEventLinks()
        {
            _entityBody.OnBodyDies -= Hide;
            _entityBody.View.OnSideSwitch -= CharacterScaleChangedListener;
            _entityBody.Health.OnCurrentHpPercentChanged -= UpdateActualHp;
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

        public void Dispose()
        {
            RemoveEventLinks();
        }

        private void OnValidate()
        {
            ConfigureBarVisual();
        }

        #endregion
    }
}
