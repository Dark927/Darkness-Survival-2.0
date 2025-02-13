using Characters.Enemy.Data;
using System;
using Unity.Collections;
using UnityEngine;

namespace Characters.Stats
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/Characters/Data/DefaultCharacterData")]
    public class CharacterBaseData : ScriptableObject
    {
        #region Fields 

        public int ID { get => _id; private set => _id = value; }
        public string Name => _name;
        public CharacterStats Stats => _stats;

        #endregion


        #region Properties

        [Header("Stats")]

        [SerializeField] private string _name;
        [SerializeField, ReadOnly] private int _id;
        [SerializeField] private CharacterStats _stats;

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
                Stats.GetHashCode()
            );
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is not EnemyData other)
                return false;

            return Name == other.Name &&
                   Stats.Equals(other.Stats);
        }

        #endregion
    }
}
