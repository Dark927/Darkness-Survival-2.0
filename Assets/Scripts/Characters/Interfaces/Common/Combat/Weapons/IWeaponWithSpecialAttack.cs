namespace Characters.Common.Combat.Weapons
{
    public interface IWeaponWithSpecialAttack : IWeapon
    {
        public bool IsSpecialAttackActive { get; }
        public void EnableSpecialAttack();
        public void DisableSpecialAttack();
    }
}
