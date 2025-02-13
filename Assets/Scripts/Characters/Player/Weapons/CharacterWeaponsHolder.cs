using System;
using System.Collections.Generic;
using Characters.Common.Combat.Weapons;
using Characters.Interfaces;
using Characters.Player.Weapons.Data;
using UnityEngine;

namespace Characters.Player.Weapons
{
    public class CharacterWeaponsHolder : IDisposable
    {
        #region Fields


        private IEntityLogic _entityLogic;
        private GameObject _defaultWeaponContainer;

        private string _weaponsContainerName;
        private List<CharacterWeaponBase> _activeWeapons;

        #endregion


        #region Properties

        public List<CharacterWeaponBase> ActiveWeapons => _activeWeapons;
        public string DefaultContainerName => $"{_entityLogic.Data.Name ?? "default"}_weapons";
        public string WeaponsContainerName => _weaponsContainerName;

        #endregion


        #region Methods

        #region Init

        public CharacterWeaponsHolder(IEntityLogic entityLogic, string weaponsContainerName = null)
        {
            _entityLogic = entityLogic;
            _weaponsContainerName = weaponsContainerName;
        }

        public void Init()
        {
            _activeWeapons = new List<CharacterWeaponBase>();
            TryInitContainer(_weaponsContainerName);
        }

        private void TryInitContainer(string name)
        {
            if (_defaultWeaponContainer != null)
            {
                return;
            }

            string targetName = DefaultContainerName;

            if (!String.IsNullOrEmpty(name))
            {
                targetName = name;
            }

            _defaultWeaponContainer = new GameObject(targetName);
            _defaultWeaponContainer.transform.SetParent(_entityLogic.Body.transform, false);
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
            foreach (var weapon in _activeWeapons)
            {
                weapon.Dispose();
            }
        }

        #endregion
    }
}