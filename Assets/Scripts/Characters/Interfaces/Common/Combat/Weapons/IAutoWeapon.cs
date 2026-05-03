
namespace Characters.Common.Combat.Weapons
{
    public interface IAutoWeapon : IWeapon
    {
        public void StartAttack();
        public void StopAttack();
    }
}
