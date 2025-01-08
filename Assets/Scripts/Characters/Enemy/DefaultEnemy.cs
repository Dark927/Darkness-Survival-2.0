using UnityEngine;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DefaultEnemy : MonoBehaviour, IEnemyLogic
    {
        private CharacterBody _body;
        private ICharacterMovement _movement;
        private CharacterLookDirection _lookDirection;

        private void Awake()
        {
            InitComponents();
        }

        private void InitComponents()
        {
            _body = GetComponent<CharacterBody>();
        }


        public void Attack()
        {
            throw new System.NotImplementedException();
        }

        public void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}
