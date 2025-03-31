

using Characters.Common.Movement;
using Characters.Interfaces;
using UnityEngine;

namespace Characters.Common.Visual
{
    [RequireComponent(typeof(ParticleSystem))]
    public class EntityParticleTrailVisualPart : MonoBehaviour, IEntityCustomVisualPart
    {
        private ParticleSystem _particleSystem;
        private IEntityPhysicsBody _physicsBody;
        private bool _isActive = false;


        public void Initialize(IEntityBody targetEntityBody)
        {
            _physicsBody = targetEntityBody as IEntityPhysicsBody;
            _physicsBody.Movement.OnMovementStateChanged += ListenMovementStateChanged;

            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystem.Stop();
        }

        private void ListenMovementStateChanged(object sender, bool isMoving)
        {
            if (_isActive == isMoving)
            {
                return;
            }

            _isActive = isMoving;

            if (_isActive)
            {
                _particleSystem.Play();
            }
            else
            {
                _particleSystem.Stop();
            }
        }

        public void Dispose()
        {
            _particleSystem.Stop();
            _physicsBody.Movement.OnMovementStateChanged -= ListenMovementStateChanged;
        }

    }
}
