
namespace Characters.Interfaces
{
    public interface IDamageable
    {
        public bool CanAcceptDamage { get; }
        public void TakeDamage(float damage);
    }
}
