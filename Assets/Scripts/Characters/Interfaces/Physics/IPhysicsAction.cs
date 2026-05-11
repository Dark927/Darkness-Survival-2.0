using System;

namespace Characters.Common.CustomPhysics2D
{
    public interface IPhysicsAction : IComparable<IPhysicsAction>
    {
        int Priority { get; }
        public void Perform(IEntityPhysics2D entityPhysics);
        public void SetValues(params object[] values);
    }
}
