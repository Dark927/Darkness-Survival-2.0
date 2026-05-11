namespace Characters.Common.Statuses
{
    public interface IStatusEffect
    {
        bool IsFinished { get; }

        void OnApply(IEntityDynamicLogic target);
        void OnUpdate(float deltaTime);
        void OnRemove(IEntityDynamicLogic target);

        /// <summary>
        /// Defines how to handle a duplicate status (e.g., extend duration, add stacks, or overwrite).
        /// </summary>
        void Merge(IStatusEffect newEffect);
    }
}
