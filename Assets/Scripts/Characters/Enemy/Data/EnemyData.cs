using Characters.Stats;
using System;
using Unity.Collections;
using UnityEngine;

namespace Characters.Enemy.Data
{
    public enum EnemyType
    {
        Default = 0,
        Toxin = 1,
        Bloody = 2,
        Golden = 3,
    }

    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Characters/Data/EnemyData")]
    public class EnemyData : AttackableCharacterData
    {
        #region Fields 

        [SerializeField, ReadOnly] private int _id;

        [Header("Custom Enemy Settings")]

        [SerializeField] private EnemyType _type;
        [SerializeField] private GameObject _enemyPrefab;

        #endregion


        #region Properties
        public int ID { get => _id; private set => _id = value; }
        public EnemyType Type => _type;
        public GameObject EnemyPrefab => _enemyPrefab;

        #endregion


        #region Methods

        private void OnValidate()
        {
            ID = GetHashCode();
        }

        public override int GetHashCode()
        {
            int hash = HashCode.Combine(
                Name != null ? Name.GetHashCode() : 0,
                Type.GetHashCode(),
                EnemyPrefab != null ? EnemyPrefab.GetHashCode() : 0,
                Stats.GetHashCode()
            );
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is not EnemyData other)
                return false;

            return Name == other.Name &&
                   Type == other.Type &&
                   EnemyPrefab == other.EnemyPrefab &&
                   Stats.Equals(other.Stats);
        }

        #endregion
    }
}
