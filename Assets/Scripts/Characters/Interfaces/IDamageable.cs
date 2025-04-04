
using System;

namespace Characters.Common.Combat
{
    public delegate void DamageEventHandlerWithArgs(object sender, Damage damage);
    public delegate void DamageEventHandler();

    public interface IDamageable
    {
        public event Action<IAttackable> OnKilled;

        public bool CanAcceptDamage { get; }
        public void TakeDamage(IAttackable sender, Damage damage);
    }
}
