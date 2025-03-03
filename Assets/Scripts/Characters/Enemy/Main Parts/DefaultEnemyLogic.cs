using System.Collections.Generic;
using Characters.Common;
using Characters.Common.Combat.Weapons;
using Characters.Enemy.Data;
using Characters.Interfaces;
using Characters.Stats;
using Gameplay.Components.Items;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Characters.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DefaultEnemyLogic : AttackableEntityLogic, IEnemyLogic, IAttackable<BasicAttack>
    {
        #region Fields 

        private List<ItemDropData> _dropData;
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

        private void InitItemDrop(IEnumerable<AssetReferenceT<ItemDropData>> data)
        {
            _dropData = new List<ItemDropData>();

            foreach (var itemDropDataRef in data)
            {
                AsyncOperationHandle<ItemDropData> handle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<ItemDropData>(itemDropDataRef);
                handle.Completed += (handle) => _dropData.Add(handle.Result);
            }
        }

        protected override void InitBasicAttacks()
        {
            SetBasicAttacks(new BasicAttack(Body, Weapons.ActiveWeapons));
            base.InitBasicAttacks();
        }

        #endregion

        public void SpawnRandomDropItem()
        {
            _dropData.Sort((x, y) => x.DropChance.CompareTo(y.DropChance));
            int randomValue;

            foreach (var itemDropData in _dropData)
            {
                randomValue = Random.Range(0, 100);

                if (randomValue < itemDropData.DropChance)
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
