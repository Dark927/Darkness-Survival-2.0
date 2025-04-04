using System.Collections.Generic;
using Characters.Common;
using Characters.Common.Combat.Weapons;
using Characters.Common.Settings;
using Characters.Enemy.Settings;
using Gameplay.Components.Items;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DefaultEnemyLogic : AttackableEntityLogicBase, IEnemyLogic
    {
        #region Fields 

        private List<ItemDropSettings> _dropData;
        private IPickupItem _currentDropItem;

        #endregion


        #region Properties


        #endregion


        #region Methods 

        #region Init

        public override void Initialize(IEntityData data)
        {
            base.Initialize(data);

            EnemyData enemyData = data as EnemyData;
            InitItemDrop(enemyData.DropItemReferences);
        }

        // ToDo : instead of this use ItemsManager to load a few instances of object to the pool and reuse them.
        private void InitItemDrop(IEnumerable<AssetReferenceT<ItemDropData>> data)
        {
            _dropData = new List<ItemDropSettings>();

            foreach (var itemDropDataRef in data)
            {
                AsyncOperationHandle<ItemDropData> handle = AddressableAssetsHandler.Instance.TryLoadAssetAsync<ItemDropData>(itemDropDataRef);
                handle.Completed +=
                    (handle) =>
                    {
                        if (_dropData != null)
                        {
                            _dropData.Add(handle.Result.DropSettings);
                        }
                        AddressableAssetsHandler.Instance.UnloadAsset(handle);
                    };
            }
        }

        protected override BasicAttack GetBasicAttacks()
        {
            return new BasicAttack(Body, WeaponsHandler.ActiveOnes);
        }

        #endregion

        public void SetTarget(Transform targetTransform)
        {
            (Body as DefaultEnemyBody).SetTarget(targetTransform);
        }

        public void SpawnRandomDropItem()
        {
            if (_dropData == null || _dropData.Count == 0)
            {
                return;
            }

            _dropData.Sort((x, y) => x.DropChance.CompareTo(y.DropChance));
            int randomValue;

            foreach (var itemDropData in _dropData)
            {
                randomValue = Random.Range(0, 100);

                if (itemDropData.Data != null && randomValue < itemDropData.DropChance)
                {
                    GameObject itemObj = GameObject.Instantiate(itemDropData.Data.Prefab, transform.position, Quaternion.identity);
                    _currentDropItem = itemObj.GetComponent<IPickupItem>();
                    _currentDropItem.SetAllParameters(itemDropData.Data.Parameters);
                    break;
                }
            }
        }


        #endregion
    }
}
