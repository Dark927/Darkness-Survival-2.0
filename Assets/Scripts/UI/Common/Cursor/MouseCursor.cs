using System;
using DG.Tweening;
using Settings.Abstract;
using Settings.Global;
using Settings.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utilities.ErrorHandling;
using Utilities.UI;
using Zenject;

namespace UI.CustomCursor
{
    [RequireComponent(typeof(Image))]
    public class MouseCursor : LazySingletonMono<MouseCursor>, IEventListener, IResetable
    {
        #region Fields 

        private Image _cursor;
        private CustomCursorData _cursorData;
        private Vector2 _cursorPosition = Vector2.zero;

        private Sequence _currentAnimation;
        private DefaultAnimationParamsUI _startParameters;

        #endregion

        #region Methods

        #region Init

        [Inject]
        public void Construct(CustomCursorData cursorData)
        {
            _cursorData = cursorData;
        }

        private void Awake()
        {
            _cursor = GetComponent<Image>();
            GameSceneLoadHandler.Instance.SceneCleanEvent.Subscribe(this);
        }

        private void Start()
        {
            if (_cursorData != null)
            {
                Cursor.visible = false;
                _cursor.sprite = _cursorData.DefaultState;

                _startParameters = new DefaultAnimationParamsUI()
                {
                    DurationInSec = _cursorData.HoverParams.DurationInSec,
                    TargetColor = _cursor.color,
                    TargetScale = _cursor.transform.localScale
                };
            }
            else
            {
                ErrorLogger.LogComponentIsNull(LogOutputType.Console, gameObject.name, nameof(CustomCursorData));
            }
        }

        private void OnDestroy()
        {
            KillActiveAnimation();
            GameSceneLoadHandler.Instance.SceneCleanEvent.Unsubscribe(this);
        }

        #endregion

        private void Update()
        {
            _cursorPosition = Mouse.current.position.ReadValue();
            transform.position = _cursorPosition;
        }

        public void HoverInteractiveUI()
        {
            _currentAnimation = PlayAnimation(_cursorData.HoverParams);
        }

        public void ExitInteractiveUI()
        {
            _currentAnimation = PlayAnimation(_startParameters);
        }

        public void Listen(object sender, EventArgs args)
        {
            if (sender is GameSceneLoadHandler)
            {
                ResetState();
            }
        }

        public void ResetState()
        {
            KillActiveAnimation();
            _cursor.transform.localScale = _startParameters.TargetScale;
            _cursor.color = _startParameters.TargetColor;
        }

        private Sequence PlayAnimation(DefaultAnimationParamsUI parameters)
        {
            KillActiveAnimation();

            Sequence animation = DOTween.Sequence();

            if (_cursor == null)
            {
                return animation;
            }

            animation.Append
            (
                _cursor
                    .DOColor(parameters.TargetColor, parameters.DurationInSec)
                    .From(_cursor.color)
            )
            .Join
            (
                _cursor.transform
                    .DOScale(parameters.TargetScale, parameters.DurationInSec)
                    .From(transform.localScale)
            );

            animation.SetUpdate(true);

            return animation;
        }

        private void KillActiveAnimation()
        {
            TweenHelper.KillTweenIfActive(_currentAnimation);
        }

        #endregion
    }
}
