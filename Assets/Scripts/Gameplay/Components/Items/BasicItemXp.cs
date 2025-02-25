
using Characters.Interfaces;
using UnityEngine;

namespace Gameplay.Components.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class BasicItemXp : MonoBehaviour, IPickupItemT<ItemXpParameters>
    {
        #region Fields 

        private SpriteRenderer _spriteRenderer;
        private ItemXpParameters _parameters = new ItemXpParameters();

        #endregion


        #region Properties

        public ItemXpParameters XpParameters => _parameters;
        public IItemParameters Parameters => XpParameters;

        #endregion


        #region Methods

        public void SetAllParameters(IItemParameters parameters)
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
            _spriteRenderer.color = parameters.TintColor;

            SetXpParameters(parameters as ItemXpParameters);
        }

        public void SetXpParameters(ItemXpParameters parameters)
        {
            _parameters.Set(parameters);
        }

        public void Pickup(ICharacterLogic targetCharacter)
        {
            targetCharacter.Level.AddXp(_parameters.XpAmount);
        }

        private void OnTriggerEnter2D(Collider2D collision)
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
