using Characters.Enemy.Data;
using Characters.Interfaces;
using Characters.Stats;
using UnityEngine;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DefaultEnemy : MonoBehaviour, IEnemyLogic
    {
        [Header("Settings")]
        [SerializeField] private EnemyData _data;
        
        private CharacterBody _body;
        public CharacterStats Stats => _data.Stats;


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
