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


        [Header("Custom Enemy Settings")]

        [SerializeField] private EnemyType _type;

        #endregion


        #region Properties

        public EnemyType Type => _type;

        #endregion


        #region Methods

        public override int GetHashCode()
        {
            int hash = HashCode.Combine(
                Name != null ? Name.GetHashCode() : 0,
                Type.GetHashCode(),
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
                   Stats.Equals(other.Stats);
        }

        #endregion
    }
}
