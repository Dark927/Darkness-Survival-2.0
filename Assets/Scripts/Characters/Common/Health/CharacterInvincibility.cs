using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Health
{
    public class CharacterInvincibility : IInvincibility
    {
        #region Fields

        public const float DefaultEffectTime = float.MaxValue;

        private SpriteRenderer _characterRenderer;
        private Color _initialColor;
        private Color _effectColor;
        private float _effectTime;

        private CancellationTokenSource _cancellationTokenSource;
        private UniTask _activeEffectTask;

        #endregion


        #region Properties

        public Color EffectColor => _effectColor;
        public bool IsActive => (_activeEffectTask.Status == UniTaskStatus.Pending);


        #endregion


        #region Methods 

        public CharacterInvincibility(float effectTime = DefaultEffectTime)
        {
            Init(effectTime, null, Color.white);
        }

        public CharacterInvincibility(SpriteRenderer characterRenderer, Color effectColor)
        {
            Init(DefaultEffectTime, characterRenderer, effectColor);

        }

        public CharacterInvincibility(SpriteRenderer characterRenderer, float effectTime, Color effectColor)
        {
            Init(effectTime, characterRenderer, effectColor);
        }

        private void Init(float effectTime, SpriteRenderer characterRenderer, Color effectColor)
        {
            _effectTime = effectTime;
            _characterRenderer = characterRenderer;
            _effectColor = effectColor;

            _activeEffectTask = UniTask.CompletedTask;
            _initialColor = _characterRenderer != null ? _characterRenderer.color : Color.white;
        }

        public void EnableWithVisual()
        {
            EnableWithVisual(_effectTime, _effectColor);
        }

        public void EnableWithVisual(Color effectColor)
        {
            EnableWithVisual(_effectTime, effectColor);
        }

        public void EnableWithVisual(float time, Color effectColor)
        {
            Enable(time);
            TryActivateVisualEffect(effectColor);
        }

        public void Enable()
        {
            Enable(_effectTime);
        }

        public void Enable(float time)
        {
            if (IsActive)
            {
                Disable();
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _activeEffectTask = ActivateEffect(time, _cancellationTokenSource.Token);
        }

        public void Disable()
        {
            CancelActiveEffect();
            TryDeactivateVisualEffect();
        }

        private void CancelActiveEffect()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTask ActivateEffect(float time, CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time), cancellationToken: token).SuppressCancellationThrow();

            return;
        }


        private void TryDeactivateVisualEffect()
        {
            if (!ReferenceEquals(_characterRenderer, null))
            {
                _characterRenderer.color = _initialColor;
            }
        }

        private void TryActivateVisualEffect(Color effectColor)
        {
            if (!ReferenceEquals(_characterRenderer, null))
            {
                _characterRenderer.color = effectColor;
            }
        }

        #endregion
    }
}
