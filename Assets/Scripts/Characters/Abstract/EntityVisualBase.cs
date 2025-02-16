using Unity.VisualScripting;
using UnityEngine;

namespace Characters.Common.Visual
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class EntityVisualBase : MonoBehaviour, IInitializable
    {
        #region Fields

        private SpriteRenderer _spriteRenderer;

        #endregion

        public SpriteRenderer Renderer => _spriteRenderer;
        public Sprite CharacterSprite { get => _spriteRenderer.sprite; set { _spriteRenderer.sprite = value; } }
        public bool HasAnimation { get => GetAnimatorController() != null; }
        public bool IsVisibleForCamera { get => _spriteRenderer.isVisible; }


        #region Methods

        public abstract AnimatorController GetAnimatorController();
        public abstract T GetAnimatorController<T>() where T : AnimatorController;


        public virtual void Initialize()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        #endregion
    }
}
