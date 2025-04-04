using System;
using UnityEngine;

namespace Characters.Common.Settings
{
    [CreateAssetMenu(fileName = "NewEntityData", menuName = "Game/Characters/DefaultEntityData")]
    public class EntityBaseData : ScriptableObject, IEntityData
    {
        #region Fields 

        [Header("Stats")]

        [SerializeField] private EntityInfo _info;
        [SerializeField] private CharacterStats _stats;

        #endregion


        #region Properties


        public EntityInfo CommonInfo => _info;
        public CharacterStats Stats => _stats;

        #endregion


        #region Methods

        private void Awake()
        {
            _info.UpdateTypeID(GetHashCode());
        }

        public override int GetHashCode()
        {
            int hash = HashCode.Combine(
                CommonInfo.Name != null ? CommonInfo.Name.GetHashCode() : 0,
                Stats.GetHashCode()
            );
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is not EntityBaseData other)
                return false;

            return CommonInfo.Name == other.CommonInfo.Name &&
                   Stats.Equals(other.Stats);
        }

        #endregion
    }
}
