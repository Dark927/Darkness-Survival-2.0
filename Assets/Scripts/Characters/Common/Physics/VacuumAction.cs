using UnityEngine;

namespace Characters.Common.CustomPhysics2D
{
    public class VacuumAction : IPhysicsAction
    {
        private float _force;
        private Vector2 _direction;

        // Priority 100: Executes alongside knockbacks
        public int Priority => 100;

        public VacuumAction(params object[] values)
        {
            SetValues(values);
        }

        public void Perform(IEntityPhysics2D entityPhysics)
        {
            Pull(entityPhysics);
        }

        public void SetValues(params object[] values)
        {
            _force = (float)values[0];
            _direction = (Vector2)values[1];
        }

        private void Pull(IEntityPhysics2D entityPhysics)
        {
            // smooth, continuous suction effect instead of the jerky explosions caused by Impulse.
            entityPhysics.Rigidbody2D.AddForce(_force * _direction, ForceMode2D.Force);
        }

        public int CompareTo(IPhysicsAction other) => Priority.CompareTo(other.Priority);
    }
}
