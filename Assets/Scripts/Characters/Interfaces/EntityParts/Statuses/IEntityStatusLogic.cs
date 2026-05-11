
namespace Characters.Common.Statuses
{
    public interface IEntityStatusLogic
    {
        void Apply(IStatusEffect newEffect);
        void UpdateTimers();
        void ClearAll();
    }
}
