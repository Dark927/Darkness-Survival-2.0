
using Characters.Common.Combat;

namespace Characters.Interfaces
{
    public delegate void DamageEventHandlerWithArgs(object sender, Damage damage);
    public delegate void DamageEventHandler();

    public interface IDamageable
    {
        public bool CanAcceptDamage { get; }
        public void TakeDamage(Damage damage);
    }
}
