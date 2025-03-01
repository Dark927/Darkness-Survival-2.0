using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Characters.Common.Visual
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class EntityVisualBase : MonoBehaviour, IEntityVisual
    {
        #region Fields

        private readonly string _matFlashColorName = "_FlashColor";
        private readonly string _matFlashAmountName = "_FlashAmount";

        private SpriteRenderer _spriteRenderer;
        private CancellationTokenSource _blinkCts;
        private Material _material;

        #endregion


        #region Properties

        public SpriteRenderer Renderer => _spriteRenderer;
        public Sprite CharacterSprite { get => _spriteRenderer.sprite; set { _spriteRenderer.sprite = value; } }
        public bool HasAnimation { get => GetAnimatorController() != null; }
        public bool IsVisibleForCamera { get => _spriteRenderer.isVisible; }

        #endregion


        #region Methods

        public abstract AnimatorController GetAnimatorController();
        public abstract T GetAnimatorController<T>() where T : AnimatorController;


        #region Init 

        public virtual void Initialize()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _material = new Material(_spriteRenderer.material);
            _spriteRenderer.material = _material;
        }

        public void Dispose()
        {
            DeactivateActualColorBlink();
        }

        #endregion


        public void ActivateColorBlink(Color targetColor, float durationInSec, float stepInSec)
        {
            DeactivateActualColorBlink();

            _blinkCts = new CancellationTokenSource();
            ColorBlink(targetColor, durationInSec, stepInSec, _blinkCts.Token).Forget();
        }

        public void DeactivateActualColorBlink()
        {
            if (_blinkCts == null)
            {
                return;
            }

            _blinkCts.Cancel();
            _blinkCts.Dispose();
            _blinkCts = null;
        }


        private async UniTask ColorBlink(Color targetColor, float durationInSec, float stepInSec, CancellationToken token = default)
        {
            float lerpTime = 0;
            SetFlashColor(targetColor);

            while (lerpTime < durationInSec)
            {
                lerpTime += Time.deltaTime;
                float perc = lerpTime / durationInSec;

                if (token.IsCancellationRequested)
                {
                    return;
                }

                SetFlashAmount(1f - perc);
                await UniTask.NextFrame();
            }

            SetFlashAmount(0);
        }


        protected void SetFlashColor(Color targetColor)
        {
            _material.SetColor(_matFlashColorName, targetColor);
        }

        private void SetFlashAmount(float flashAmount)
        {
            _material.SetFloat(_matFlashAmountName, flashAmount);
        }
    }

    #endregion
}

