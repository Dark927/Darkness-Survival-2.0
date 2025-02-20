

namespace Characters.Common.Physics2D
{
    public interface IPhysicsAction
    {
        public void Perform(IEntityPhysics2D entityPhysics);
        public void SetValues(params object[] values);
    }
}
