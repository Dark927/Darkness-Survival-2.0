

using UnityEngine;

namespace Characters.Common.Physics2D
{
    public class KnockbackAction : IPhysicsAction
    {
        private float _force;
        private Vector2 _direction;

        public KnockbackAction(params object[] values)
        {
            SetValues(values);
        }

        public void Perform(IEntityPhysics2D entityPhysics)
        {
            Knockback(entityPhysics);
        }

        public void SetValues(params object[] values)
        {
            _force = (float)values[0];
            _direction = (Vector2)values[1];
        }

        public void Knockback(IEntityPhysics2D entityPhysics)
        {
            entityPhysics.Rigidbody2D.AddForce(_force * _direction, ForceMode2D.Impulse);
        }
    }
}
