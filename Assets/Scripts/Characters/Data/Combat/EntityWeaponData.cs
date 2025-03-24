using UnityEngine;
using UnityEngine.AddressableAssets;
using Utilities.Math;

namespace Characters.Common.Combat.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/Combat/Data/Weapons/WeaponData")]
    public class EntityWeaponData : ScriptableObject
    {
        #region Fields 

        private int _weaponID;
        [SerializeField] private string _weaponName;
        [SerializeField] private AssetReferenceGameObject _weaponAsset;
        [SerializeField] private WeaponAttackDataBase _attackData;

        #endregion


        #region Properties 

        public string WeaponName
        {
            get
            {
                TrySetDefaultWeaponName();
                return _weaponName;
            }
        }

        public int ID => _weaponID;

        public AssetReferenceGameObject WeaponAsset => _weaponAsset;
        public WeaponAttackDataBase AttackData => _attackData;

        #endregion


        #region Methods

        private void TrySetDefaultWeaponName()
        {
            if (string.IsNullOrEmpty(_weaponName) && (_weaponAsset != null))
            {
                _weaponName = "entity_weapon_without_name";
            }
        }

        private void Awake()
        {
            _weaponID = CustomIdentifierGenerator.GenerateID(WeaponName, WeaponAsset);
        }



        private void OnValidate()
        {
            //TrySetDefaultWeaponName();
        }

        #endregion
    }
}
