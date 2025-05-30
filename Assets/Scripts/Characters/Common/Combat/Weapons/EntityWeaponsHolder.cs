﻿using System;
using System.Collections.Generic;
using Characters.Common.Combat.Weapons;
using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Common.Combat
{
    public class EntityWeaponsHolder : EntityFeaturesHolderBase<IWeapon>
    {
        #region Fields

        private GameObject _defaultWeaponContainer;
        private string _weaponsContainerName;

        public event Action<IWeapon> OnNewWeaponGiven;

        #endregion


        #region Properties

        public string DefaultContainerName => $"{EntityLogic.Data.Name ?? "default"}_weapons";
        public string WeaponsContainerName => _weaponsContainerName;

        #endregion


        #region Methods

        protected override void DestroyFeatureLogic(IWeapon feature)
        {
            GameObject.Destroy(feature.GameObject);
        }


        #region Init

        public EntityWeaponsHolder(IEntityDynamicLogic entityLogic, string weaponsContainerName = null) : base(entityLogic)
        {
            _weaponsContainerName = weaponsContainerName;
        }

        public override void Initialize()
        {
            base.Initialize();
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
            _defaultWeaponContainer.transform.SetParent(EntityLogic.Body.Transform, false);
        }

        #endregion


        public override async UniTask GiveFeatureAsync<TWeaponsData>(TWeaponsData data)
        {
            EntityWeaponData weaponData = data as EntityWeaponData;

            if ((weaponData == null) || !AddressableAssetsHandler.IsAssetRefValid(weaponData.WeaponAsset))
            {
                return;
            }

            var weaponLoadHandle = AddressableAssetsHandler.Instance.LoadAssetAndCacheAsync<GameObject>(weaponData.WeaponAsset, AddressableAssetsCleaner.CleanType.SceneSwitch, false);
            await weaponLoadHandle;

            var weapon = CreateFeature(weaponLoadHandle.Result, weaponData.WeaponName, _defaultWeaponContainer.transform);
            weapon.Initialize(weaponData.AttackData);

            ActiveOnesDict.TryAdd(weaponData.ID, weapon);
            OnNewWeaponGiven?.Invoke(weapon);
        }

        #endregion
    }
}
