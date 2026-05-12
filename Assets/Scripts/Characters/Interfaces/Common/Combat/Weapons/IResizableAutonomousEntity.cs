
using System;

namespace Characters.Common.Combat.Weapons
{
    public interface IResizableAutonomousEntity
    {
        public event Action<float> OnRadiusUpdated;
        public void SetRadius(float radius);
    }
}
