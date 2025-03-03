
using Characters.Interfaces;
using UnityEngine;

namespace Gameplay.Components.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class BasicItemXp : PickupItemBase
    {
        #region Fields 

        private ItemXpParameters _parameters = new ItemXpParameters();

        #endregion


        #region Properties

        public override IItemParameters Parameters => _parameters;

        #endregion


        #region Methods



        public override void Pickup(ICharacterLogic targetCharacter)
        {
            targetCharacter.Level.AddXp(_parameters.XpAmount);
        }

        #endregion
    }
}
