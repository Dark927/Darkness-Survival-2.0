using System.Diagnostics;
using Utilities.ErrorHandling;

namespace Characters.Common.Statuses
{
    public class StunStatusEffect : StatusEffectBase
    {
        public StunStatusEffect(float duration)
        {
            Duration = duration;
        }

        public override void OnApply(IEntityDynamicLogic target)
        {
            base.OnApply(target);
            target.Body.Movement.Block();
        }

        public override void OnRemove(IEntityDynamicLogic target)
        {
            // Unblock when the status timer expires
            target.Body.Movement.Unblock();
        }
    }
}
