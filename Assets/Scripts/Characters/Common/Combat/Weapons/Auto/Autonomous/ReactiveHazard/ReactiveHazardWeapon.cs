using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Characters.Common.Combat.Weapons.Data;
using Characters.Common.CustomPhysics2D;
using Characters.Common.Statuses;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class ReactiveHazardWeapon<TSettings, TEntity> : HazardWeaponBase<TSettings, TEntity>
        where TSettings : ReactiveHazardAttackSettings
        where TEntity : AutonomousEntityBase, IReactiveAutonomousEntity
    {
        protected readonly Dictionary<IEntityPhysicsBody, CancellationTokenSource> ActiveMarks = new Dictionary<IEntityPhysicsBody, CancellationTokenSource>();
        private static readonly Collider2D[] MarkBuffer = new Collider2D[64];

        private int _marksAppliedThisPhase;
        private int _maxMarksForPhase;
        private CancellationTokenSource _weaponLifetimeCts;

        public override void Initialize(WeaponAttackData attackData)
        {
            base.Initialize(attackData);
            _weaponLifetimeCts = new CancellationTokenSource();
        }

        protected override async UniTask PerformAttackPhase(CancellationToken token)
        {
            float elapsed = 0f;
            float tickRate = UpgradedAttackSettings.TriggerActivityTimeSec;

            _maxMarksForPhase = UpgradedAttackSettings.SpawnCount;
            _marksAppliedThisPhase = 0;

            // Loop until the Scanning Duration runs out
            while (elapsed < UpgradedAttackSettings.FullDurationTimeSec)
            {
                if (_marksAppliedThisPhase < _maxMarksForPhase)
                {
                    FireWeapon();
                }

                // Early Exit: We spent our ammo, AND all marked enemies are dead or expired.
                if (_marksAppliedThisPhase >= _maxMarksForPhase && ActiveMarks.Count == 0)
                {
#if UNITY_EDITOR
                    Debug.Log("[ReactiveWeapon] Scanning complete and all marks resolved. Early Reload.");
#endif
                    break;
                }

                bool isCanceled = await UniTask.WaitForSeconds(tickRate, cancellationToken: token).SuppressCancellationThrow();
                if (isCanceled) return;

                elapsed += tickRate;
            }

            // Marks applied late in the phase will continue to live into the Reload phase.
        }

        protected override void FireWeapon()
        {
            Vector3 center = Owner.Body.TargetingTransform.position;
            float maxRadius = UpgradedAttackSettings.MaxSpawnRadius;
            float minRadiusSqr = UpgradedAttackSettings.MinSpawnRadius * UpgradedAttackSettings.MinSpawnRadius;

            int hitCount = Physics2D.OverlapCircleNonAlloc(center, maxRadius, MarkBuffer, _enemyLayerMask);

            for (int i = 0; i < hitCount; i++)
            {
                if (_marksAppliedThisPhase >= _maxMarksForPhase) break;

                if (TryMarkTarget(MarkBuffer[i], center, minRadiusSqr))
                {
                    _marksAppliedThisPhase++;
                }
            }
        }

        private bool TryMarkTarget(Collider2D targetCollider, Vector3 center, float minRadiusSqr)
        {
            float sqrDistanceToEnemy = (targetCollider.transform.position - center).sqrMagnitude;
            if (sqrDistanceToEnemy < minRadiusSqr) return false;

            if (!targetCollider.TryGetComponent<EntityColliderLink>(out var link) || link.Logic.Body == null) return false;

            return MarkEnemy(link.Logic.Body);
        }

        private bool MarkEnemy(IEntityPhysicsBody targetBody)
        {
            // Check if they are already in the Dictionary
            if (!targetBody.IsDying && !ActiveMarks.ContainsKey(targetBody))
            {
                targetBody.OnBodyDiesWithArgs += HandleEnemyDeath;
                targetBody.OnBodyDiedCompletelyWithArgs += HandleEnemyCleanup;

                float effectDuration = UpgradedAttackSettings.EffectDurationTimeSec;

                if (targetBody.OriginalTransform.TryGetComponent<EntityColliderLink>(out var link))
                {
                    if (link.Logic.Status != null && UpgradedAttackSettings.MarkVisualPrefab != null)
                    {
                        link.Logic.Status.Apply(new MarkedStatusEffect(UpgradedAttackSettings.MarkVisualPrefab, effectDuration));
                    }
                }

                var markCts = CancellationTokenSource.CreateLinkedTokenSource(_weaponLifetimeCts.Token);
                ActiveMarks.Add(targetBody, markCts);

                // Fire and forget the expiration task
                ExpireMarkAsync(targetBody, effectDuration, markCts.Token).Forget();

                return true;
            }

            return false;
        }

        private async UniTaskVoid ExpireMarkAsync(IEntityPhysicsBody body, float duration, CancellationToken token)
        {
            bool isCanceled = await UniTask.WaitForSeconds(duration, cancellationToken: token).SuppressCancellationThrow();

            // If the token was canceled, it means the target DIED before the timer finished. We do nothing.
            // If it was NOT canceled, the timer finished naturally, so we safely unmark them.
            if (!isCanceled)
            {
#if UNITY_EDITOR
                Debug.Log("[ReactiveWeapon] A mark expired naturally due to time out.");
#endif
                UnmarkEnemy(body);
            }
        }

        protected virtual void HandleEnemyDeath(IEntityPhysicsBody dyingBody)
        {
            if (ActiveMarks.ContainsKey(dyingBody))
            {
                SpawnReactionEntity(dyingBody.TargetingTransform.position);
                UnmarkEnemy(dyingBody); // This cancels the ExpireMarkAsync timer automatically
            }
        }

        private void HandleEnemyCleanup(IEntityPhysicsBody completelyDeadBody)
        {
            UnmarkEnemy(completelyDeadBody);
        }

        private void UnmarkEnemy(IEntityPhysicsBody body)
        {
            if (ActiveMarks.TryGetValue(body, out var cts))
            {
                // Cancel specific expiration timer
                cts.Cancel();
                cts.Dispose();

                ActiveMarks.Remove(body);

                // Unsubscribe
                body.OnBodyDiesWithArgs -= HandleEnemyDeath;
                body.OnBodyDiedCompletelyWithArgs -= HandleEnemyCleanup;
            }
        }

        private void ClearAllMarks()
        {
            // We must copy the keys to a list before iterating, because UnmarkEnemy modifies the Dictionary
            var keys = ActiveMarks.Keys.ToList();
            foreach (var body in keys)
            {
                UnmarkEnemy(body);
            }
        }

        protected virtual void SpawnReactionEntity(Vector3 position)
        {
            TEntity explosionEntity = EntityPool.RequestObject();
            if (explosionEntity == null) return;

            explosionEntity.ActivateReaction(
                position: position,
                radius: UpgradedAttackSettings.AttackRadius,
                visualLifeTime: 0.5f,
                targetMask: _enemyLayerMask,
                onHit: HandleReactionHit,
                onDie: HandleReactionDeath
            );
        }

        protected virtual void HandleReactionHit(Collider2D targetCollider)
        {
            HitTargetListener(this, new AttackTriggerArgs(targetCollider));

            // CHAIN REACTION: Bypasses MaxMarksForPhase
            if (targetCollider.TryGetComponent<EntityColliderLink>(out var link) && link.Logic.Body != null)
            {
                MarkEnemy(link.Logic.Body);
            }
        }

        private void HandleReactionDeath(AutonomousEntityBase entity)
        {
            if (entity is TEntity reactiveEntity)
            {
                EntityPool.ReturnItem(reactiveEntity);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            // Cancel any remaining floating async tasks
            if (_weaponLifetimeCts != null)
            {
                _weaponLifetimeCts.Cancel();
                _weaponLifetimeCts.Dispose();
                _weaponLifetimeCts = null;
            }

            ClearAllMarks();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Gizmos should only draw if the game is running and the weapon is fully initialized.
            if (!Application.isPlaying || Owner?.Body?.TargetingTransform == null || UpgradedAttackSettings == null)
            {
                return;
            }

            Vector3 center = Owner.Body.TargetingTransform.position;

            // Draw the Min Radius (The "Donut Hole" / Inner boundary)
            Gizmos.color = new Color(1f, 0.7f, 0f, 0.6f);
            Gizmos.DrawWireSphere(center, UpgradedAttackSettings.MinSpawnRadius);

            // Draw the Max Radius (The outer boundary)
            Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
            Gizmos.DrawWireSphere(center, UpgradedAttackSettings.MaxSpawnRadius);
        }
#endif
    }
}
