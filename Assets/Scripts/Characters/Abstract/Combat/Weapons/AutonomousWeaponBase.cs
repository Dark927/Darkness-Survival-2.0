using System.Threading;
using Characters.Common.Combat.Weapons.Data;
using Cysharp.Threading.Tasks;
using Gameplay.Components;
using Settings.Global;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// This class handles auto weapons with attack that don't need to follow player (like projectiles, meteors, etc.)
    /// </summary>
    /// <typeparam name="TSettings"></typeparam>
    /// <typeparam name="TEntity">type of object to spawn (Bullet, Thunder, Meteor)</typeparam>
    public abstract class AutonomousWeaponBase<TSettings, TEntity> : UpgradableAutoWeaponBase<TSettings>
        where TSettings : class, IAttackSettings
        where TEntity : AutonomousEntityBase
    {
        [Header("Entity Factory")]
        [SerializeField] protected GameObject _entityPrefab;
        [SerializeField] protected LayerMask _enemyLayerMask;

        [SerializeField] private int _preloadCount = 2;
        [SerializeField] private int _maxPoolCapacity = 100;

        protected AutonomousEntityPool<TEntity> EntityPool;

        public override void Initialize(WeaponAttackData attackData)
        {
            base.Initialize(attackData);

            var containersService = ServiceLocator.Current.Get<GameplayContainersService>();
            GameObjectsContainer weaponContainer = containersService.GetAutonomousWeaponPoolContainer(WeaponName);

            ObjectPoolSettings poolSettings = new ObjectPoolSettings(_maxPoolCapacity, _preloadCount);
            EntityPool = new AutonomousEntityPool<TEntity>(poolSettings, _entityPrefab, weaponContainer.transform);
            EntityPool.Initialize();
        }

        protected override async UniTask PerformAttackPhase(CancellationToken token)
        {
            float pulseTime = UpgradedAttackSettings.TriggerActivityTimeSec;
            int attackCount = Mathf.FloorToInt(UpgradedAttackSettings.FullDurationTimeSec / pulseTime);
            if (attackCount < 1) attackCount = 1;

            for (int i = 0; i < attackCount; i++)
            {
                FireWeapon();
                bool isCanceled = await UniTask.WaitForSeconds(pulseTime, cancellationToken: token).SuppressCancellationThrow();
                if (isCanceled) return;
            }
        }
    }
}
