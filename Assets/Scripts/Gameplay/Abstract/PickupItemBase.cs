using Characters.Interfaces;
using UnityEngine;


namespace Gameplay.Components.Items
{
    public abstract class PickupItemBase : MonoBehaviour, IPickupItem
    {
        #region Fields

        private SpriteRenderer _spriteRenderer;

        #endregion


        #region Properties

        public abstract IItemParameters Parameters { get; }

        #endregion


        #region Methods

        #region Abstract

        public abstract void Pickup(ICharacterLogic targetCharacter);

        #endregion


        public virtual void SetAllParameters(IItemParameters parameters)
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
            _spriteRenderer.color = parameters.TintColor;

            Parameters.Set(parameters);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out ICharacterLogic playerCharacter))
            {
                Pickup(playerCharacter);
                Destroy(gameObject);
            }
        }

        #endregion
    }
}
