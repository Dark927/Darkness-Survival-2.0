namespace Characters.Common.CustomPhysics2D
{
    public class StunAction : IPhysicsAction
    {
        private int _durationMs;

        // Priority 0: Executes First
        public int Priority => 0;

        public StunAction(params object[] values)
        {
            SetValues(values);
        }

        public void Perform(IEntityPhysics2D entityPhysics)
        {
            Stun(entityPhysics);
        }

        public void SetValues(params object[] values)
        {
            _durationMs = (int)values[0];
        }

        public void Stun(IEntityPhysics2D entityPhysics)
        {
            entityPhysics.TriggerStunActivationEvent(_durationMs);
        }

        public int CompareTo(IPhysicsAction other) => Priority.CompareTo(other.Priority);
    }
}
