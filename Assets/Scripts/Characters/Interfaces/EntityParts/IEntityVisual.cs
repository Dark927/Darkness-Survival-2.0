

using System;
using Settings.Global;
using UnityEngine;

namespace Characters.Common.Visual
{
    public interface IEntityVisual : IInitializable, IDisposable
    {
        #region Properties

        public SpriteRenderer Renderer { get; }
        public Sprite CharacterSprite { get; set; }
        public bool HasAnimation { get; }
        public bool IsVisibleForCamera { get; }

        #endregion


        #region Methods

        public AnimatorController GetAnimatorController();
        public T GetAnimatorController<T>() where T : AnimatorController;

        public void ActivateColorBlink(Color targetColor, float durationInSec, float stepInSec);
        public void DeactivateActualColorBlink();

        #endregion
    }
}
