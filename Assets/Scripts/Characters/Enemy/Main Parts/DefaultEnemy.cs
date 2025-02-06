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
        
        private CharacterBodyBase _body;
        public CharacterStats Stats => _data.Stats;
        public CharacterBodyBase Body => _body;

        private void Awake()
        {
            InitComponents();
        }

        private void InitComponents()
        {
            _body = GetComponent<CharacterBodyBase>();
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
