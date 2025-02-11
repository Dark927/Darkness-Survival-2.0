using Characters.Common.Combat.Weapons;
using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Player.Weapons
{
    public class CharacterWeaponsHolder : IDisposable
    {
        #region Fields


        private IEntityLogic _entityLogic;
        private GameObject _defaultWeaponContainer;

        private string _weaponsContainerName;
        private List<WeaponBase> _activeWeapons;

        #endregion


        #region Properties

        public List<WeaponBase> ActiveWeapons => _activeWeapons;
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
            _activeWeapons = new List<WeaponBase>();
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
            _defaultWeaponContainer.transform.SetParent(_entityLogic.Body.Transform, false);
        }

        #endregion

        public void GiveMultipleWeapons(List<EntityWeaponData> weaponsDataList, GameObject weaponContainer = null)
        {
            foreach (var weaponData in weaponsDataList)
            {
                GiveWeapon(weaponData, weaponContainer);
            }
        }

        public void GiveWeapon(EntityWeaponData weaponData, GameObject weaponContainer = null)
        {
            GameObject container = weaponContainer != null ? weaponContainer : _defaultWeaponContainer;

            WeaponBase weapon = GameObject.Instantiate(weaponData.Weapon, container.transform.position, Quaternion.identity, container.transform);
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
