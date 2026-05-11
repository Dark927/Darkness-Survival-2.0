using UnityEngine;

namespace Characters.Common.Statuses
{
    public abstract class StatusEffectBase : IStatusEffect
    {
        protected IEntityDynamicLogic Target;

        public float Duration { get; protected set; }
        public bool IsFinished => Duration <= 0f;

        public virtual void OnApply(IEntityDynamicLogic target)
        {
            Target = target;
        }

        public virtual void OnUpdate(float deltaTime)
        {
            Duration -= deltaTime;
        }

        public virtual void OnRemove(IEntityDynamicLogic target) { }

        public virtual void Merge(IStatusEffect newEffect)
        {
            if (newEffect is StatusEffectBase otherBase)
            {
                Duration = Mathf.Max(Duration, otherBase.Duration);
            }
        }
    }
}
