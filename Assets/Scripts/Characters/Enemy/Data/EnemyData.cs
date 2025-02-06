
using Characters.Common.Combat.Weapons.Data;
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
        [SerializeField, ReadOnly] private int _id;
        public int ID { get => _id; private set => _id = value; }

        [Header("Custom Enemy Settings")]

        public EnemyType Type;
        public GameObject Prefab;


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
                Prefab != null ? Prefab.GetHashCode() : 0,
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
                   Prefab == other.Prefab &&
                   Stats.Equals(other.Stats);
        }

        #endregion
    }
}
