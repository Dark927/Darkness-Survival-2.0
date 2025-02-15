using Characters.Common;
using Characters.Common.Combat.Weapons;
using Characters.Common.Combat.Weapons.Data;
using Characters.Interfaces;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Player.Weapons
{
    public class EntityWeaponsHolder : EntityFeaturesHolderBase<WeaponBase>
    {
        #region Fields

        private GameObject _defaultWeaponContainer;
        private string _weaponsContainerName;

        public event Action<WeaponBase> OnNewWeaponGiven;

        #endregion


        #region Properties

        public IEnumerable<WeaponBase> ActiveWeapons => LoadedFeaturesDict?.Keys;
        public string DefaultContainerName => $"{EntityLogic.Data.Name ?? "default"}_weapons";
        public string WeaponsContainerName => _weaponsContainerName;

        #endregion


        #region Methods

        protected override void DestroyFeatureLogic(WeaponBase feature)
        {
            GameObject.Destroy(feature.gameObject);
        }


        #region Init

        public EntityWeaponsHolder(IEntityLogic entityLogic, string weaponsContainerName = null) : base(entityLogic)
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

            if ((weaponData == null) || !AddressableAssetsLoader.IsAssetRefValid(weaponData.WeaponAsset))
            {
                return;
            }

            AsyncOperationHandle<GameObject> weaponLoadHandle = AddressableAssetsLoader.Instance.LoadAssetAsync<GameObject>(weaponData.WeaponAsset);
            await weaponLoadHandle;

            var weapon = CreateFeature(weaponLoadHandle.Result, weaponData.name, _defaultWeaponContainer.transform);
            weapon.Initialize(weaponData.AttackData);

            LoadedFeaturesDict.Add(weapon, weaponLoadHandle);
            OnNewWeaponGiven?.Invoke(weapon);
        }

        #endregion
    }
}
