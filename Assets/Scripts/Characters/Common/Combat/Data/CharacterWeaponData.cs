using Characters.Common.Combat.Weapons;
using UnityEngine;

namespace Characters.Player.Weapons.Data
{
    [CreateAssetMenu(fileName = "NewWeaponData", menuName = "Game/Characters/Data/Weapons/WeaponData")]
    public class CharacterWeaponData : ScriptableObject
    {
        #region Fields 

        [SerializeField] private string _weaponName;
        [SerializeField] private CharacterWeaponBase _weapon;

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
        public CharacterWeaponBase Weapon => _weapon;

        #endregion


        #region Methods

        private void TrySetDefaultWeaponName()
        {
            if (string.IsNullOrEmpty(_weaponName) && (_weapon != null))
            {
                _weaponName = _weapon.gameObject.name;
            }
        }

        private void OnValidate()
        {
            //SetDefaultWeaponName();
        }

        #endregion
    }
}
