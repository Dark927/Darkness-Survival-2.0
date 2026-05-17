
namespace Characters.Common.Statuses
{
    public interface IEntityStatusLogic
    {
        void Apply(IStatusEffect newEffect);
        void UpdateTimers();
        void ClearAll();
        void Remove<T>() where T : IStatusEffect;
    }
}
