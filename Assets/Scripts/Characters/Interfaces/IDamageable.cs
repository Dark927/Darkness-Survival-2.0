
using System;

namespace Characters.Common.Combat
{
    public enum DamageableType
    {
        Undefined = 0,
        Player,
        Enemy,
        Destructible
    }

    public delegate void DamageEventHandlerWithArgs(object sender, Damage damage);
    public delegate void DamageEventHandler();

    public interface IDamageable
    {
        public DamageableType Type { get; }

        public event Action<IAttackable> OnKilled;
        public bool CanAcceptDamage { get; }
        public void TakeDamage(IAttackable sender, Damage damage);
        public void SetDamageableType(DamageableType type);
    }
}
