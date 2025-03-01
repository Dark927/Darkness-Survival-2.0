namespace Characters.Common.Physics2D
{
    public class StunAction : IPhysicsAction
    {
        private int _durationMs;

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
            entityPhysics.EntityLogic.ApplyStun(_durationMs);
        }
    }
}
