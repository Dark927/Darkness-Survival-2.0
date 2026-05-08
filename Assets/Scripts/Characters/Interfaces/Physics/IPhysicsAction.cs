

namespace Characters.Common.CustomPhysics2D
{
    public interface IPhysicsAction
    {
        public void Perform(IEntityPhysics2D entityPhysics);
        public void SetValues(params object[] values);
    }
}
