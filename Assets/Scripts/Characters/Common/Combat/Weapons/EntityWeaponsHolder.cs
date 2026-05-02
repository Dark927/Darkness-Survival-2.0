using System;
using Characters.Common.Combat.Weapons;
using Characters.Common.Combat.Weapons.Data;
using Cysharp.Threading.Tasks;
using Settings.AssetsManagement;
using UnityEngine;

namespace Characters.Common.Combat
{
    public class EntityWeaponsHolder : EntityFeaturesHolderBase<IWeapon, EntityWeaponData>
    {
        #region Events

        public event Action<IWeapon> OnNewWeaponGiven;

        #endregion


        #region Properties

        public override string DefaultContainerName => $"{EntityLogic.Info.Name ?? "default"}_weapons";

        #endregion


        #region Methods

        protected override void DestroyFeatureLogic(IWeapon feature)
        {
            GameObject.Destroy(feature.GameObject);
        }


        #region Init

        public EntityWeaponsHolder(IEntityDynamicLogic entityLogic, string weaponsContainerName = null) : base(entityLogic, weaponsContainerName)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            TryInitContainer();
        }

        #endregion


        public override async UniTask GiveAsync(EntityWeaponData weaponData)
        {
            if ((weaponData == null) || !AddressableAssetsHandler.IsAssetRefValid(weaponData.WeaponAsset))
            {
                return;
            }

            var weaponLoadHandle = AddressableAssetsHandler.Instance.LoadAssetAndCacheAsync<GameObject>(weaponData.WeaponAsset, AddressableAssetsCleaner.CleanType.SceneSwitch, false);
            await weaponLoadHandle;

            var weapon = CreateFeature(weaponLoadHandle.Result, weaponData.WeaponName, DefaultFeaturesContainer.transform);
            weapon.Initialize(weaponData.AttackData);

            ActiveOnesDict.TryAdd(weaponData.ID, weapon);
            OnNewWeaponGiven?.Invoke(weapon);
        }

        #endregion
    }
}
