using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Materials.DarkEntityFX;
using UnityEngine;
using Utilities.ErrorHandling;
using Utilities.Math;

namespace Characters.Common.Visual
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class EntityVisualBase : MonoBehaviour, IEntityVisual
    {
        #region Fields

        private SpriteRenderer _spriteRenderer;
        private CancellationTokenSource _blinkCts;
        private DarkEntityFXComponent _entityFXComponent;
        private ParametricProps _properties;
        private List<IEntityCustomVisualPart> _customVisualParts;

        #endregion


        #region Properties

        public SpriteRenderer Renderer => _spriteRenderer;
        public Sprite CharacterSprite { get => _spriteRenderer.sprite; set { _spriteRenderer.sprite = value; } }
        public bool HasAnimation { get => GetAnimatorController() != null; }
        public bool IsVisibleForCamera { get => _spriteRenderer.isVisible; }
        public DarkEntityFXComponent EntityFXComponent => _entityFXComponent;

        #endregion


        #region Methods

        public abstract AnimatorController GetAnimatorController();
        public abstract T GetAnimatorController<T>() where T : AnimatorController;


        #region Init 

        public virtual void Initialize()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _entityFXComponent = GetComponent<DarkEntityFXComponent>();
            _customVisualParts = new List<IEntityCustomVisualPart>();
        }

        public void Dispose()
        {
            DeactivateActualColorBlink();

            foreach (var visualPart in _customVisualParts)
            {
                visualPart.Dispose();
            }
        }

        #endregion


        public void ActivateColorBlink(Color targetColor, float durationInSec, float period)
        {
            DeactivateActualColorBlink();

            if (EntityFXComponent == null)
            {
                return;
            }

            _blinkCts = new CancellationTokenSource();
            ColorBlink(targetColor, durationInSec, period, _blinkCts.Token).Forget();
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

        public void GiveCustomVisualPart(IEntityCustomVisualPart customVisualPart)
        {
            customVisualPart.gameObject.transform.SetParent(transform, false);
            _customVisualParts.Add(customVisualPart);
        }


        private async UniTask ColorBlink(Color targetColor, float durationInSec, float repeats, CancellationToken token = default)
        {
            float lerpTime = 0;

            repeats = repeats == 0 ? 1 : repeats;

            while (lerpTime < durationInSec)
            {
                float perc = CustomMath.Frac((lerpTime / durationInSec) * repeats);

                if (token.IsCancellationRequested)
                {
                    return;
                }

                DoFlash(targetColor, 1f - perc);

                lerpTime += Time.deltaTime;
                await UniTask.NextFrame();
            }

            DoFlash(targetColor, 0);
        }


        private void DoFlash(Color targetColor, float flashAmount)
        {
            _properties = _entityFXComponent.MaterialPropContainer.Properties;
            _properties.FlashColor = targetColor;
            _properties.FlashAmount = flashAmount;
            _entityFXComponent.MaterialPropContainer.Properties = _properties;
        }
    }

    #endregion
}

