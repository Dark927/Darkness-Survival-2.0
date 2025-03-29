
using Characters.Interfaces;
using Materials;

namespace Characters.Common.Combat.Weapons
{
    public interface IConcreteEntityWeapon<TOwnerLogic> where TOwnerLogic : IEntityDynamicLogic
    {
        public TOwnerLogic ConcreteOwner { get; }

        public void ApplySpecialAura(ScriptableMaterialPropsBase auraFxData);
    }
}
