using Characters.Common.Combat.Weapons;
using Characters.Player.Weapons.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Weapons
{
    public class CharacterWeaponsHolder : IDisposable
    {
        #region Fields

        private ICharacterLogic _character;
        private GameObject _defaultWeaponContainer;

        private List<CharacterWeaponBase> _activeWeapons;

        #endregion


        #region Properties

        public List<CharacterWeaponBase> ActiveWeapons => _activeWeapons;

        #endregion


        #region Methods

        #region Init

        public CharacterWeaponsHolder(ICharacterLogic characterLogic, GameObject weaponsContainer = null)
        {
            _character = characterLogic;
            _defaultWeaponContainer = weaponsContainer;
        }

        public void Init()
        {
            _activeWeapons = new List<CharacterWeaponBase>();
            TryInitContainer();
        }

        private void TryInitContainer()
        {
            if (_defaultWeaponContainer != null)
            {
                return;
            }
            _defaultWeaponContainer = new GameObject($"{_character.Data.Name}_weapons");
            _defaultWeaponContainer.transform.parent = _character.Body.transform;
        }

        #endregion

        public void GiveMultipleWeapons(List<CharacterWeaponData> weaponsDataList, GameObject weaponContainer = null)
        {
            foreach (var weaponData in weaponsDataList)
            {
                GiveWeapon(weaponData, weaponContainer);
            }
        }

        public void GiveWeapon(CharacterWeaponData weaponData, GameObject weaponContainer = null)
        {
            GameObject container = weaponContainer != null ? weaponContainer : _defaultWeaponContainer;

            CharacterWeaponBase weapon = GameObject.Instantiate(weaponData.Weapon, container.transform.position, Quaternion.identity, container.transform);
            weapon.gameObject.name = weaponData.WeaponName;

            _activeWeapons.Add(weapon);
        }

        public void Dispose()
        {
            foreach(var weapon in _activeWeapons)
            {
                weapon.Dispose();
            }
        }

        #endregion
    }
}