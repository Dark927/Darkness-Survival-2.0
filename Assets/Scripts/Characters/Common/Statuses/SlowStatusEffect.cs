using Utilities.ErrorHandling;

namespace Characters.Common.Statuses
{
    public class SlowStatusEffect : StatusEffectBase
    {
        public float SpeedMultiplier { get; private set; }

        public SlowStatusEffect(float duration, float speedMultiplier)
        {
            Duration = duration;
            SpeedMultiplier = speedMultiplier;
        }

        public override void OnApply(IEntityDynamicLogic target)
        {
            base.OnApply(target);
            target.Body.Movement.SetTemporarySpeedMultiplier(SpeedMultiplier);
        }

        public override void OnRemove(IEntityDynamicLogic target)
        {
            target.Body.Movement.RestoreSpeedMultiplier();
        }

        public override void Merge(IStatusEffect newEffect)
        {
            base.Merge(newEffect); // Extends the duration

            if (newEffect is SlowStatusEffect newSlow)
            {
                // In our system, a smaller multiplier is a STRONGER slow (e.g., 0.4 is slower than 0.8)
                if (newSlow.SpeedMultiplier < SpeedMultiplier)
                {
                    SpeedMultiplier = newSlow.SpeedMultiplier;

                    // We instantly force the Movement component to accept the update
                    Target.Body.Movement.SetTemporarySpeedMultiplier(SpeedMultiplier);
                }
            }
        }
    }
}
