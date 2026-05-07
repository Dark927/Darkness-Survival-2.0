using Characters.Common.Combat.Weapons.Data;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class GalvanicHemorrhageWeapon : UpgradableAutoWeaponBase
    {
        [Header("References")]
        [SerializeField] private WeaponAnimatorController _animatorController;

        [Header("Physics Scanner")]
        [SerializeField] private LayerMask _enemyLayerMask;
        [SerializeField] private const int MAX_HIT_TARGETS = 100;

        [Header("Extra Settings")]
        [SerializeField] private float _yOffset = 0f;

        private Collider2D[] _hitBuffer = new Collider2D[MAX_HIT_TARGETS];

        private AoeAttackSettings _upgradedAoeAttackSettings = null;

        public event Action<float> OnAuraPhaseStarted;
        public event Action<float> OnAttackRadiusUpgraded;
        public event Action OnAuraPhaseEnded;
        public event Action OnAuraUnlockedEvent;

        public override void Initialize(WeaponAttackData attackData)
        {
            base.Initialize(attackData);
            _upgradedAoeAttackSettings = (AoeAttackSettings)UpgradedAttackSettings;

            if (_animatorController != null)
            {
                _animatorController.OnAttackImpactTriggered += ApplyDamageOnHit;
            }

            transform.position = (Vector2)transform.position + new Vector2(0, _yOffset);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_animatorController != null)
            {
                _animatorController.OnAttackImpactTriggered -= ApplyDamageOnHit;
            }
        }

        public void UnlockAura()
        {
            OnAuraUnlockedEvent?.Invoke();
        }

        protected override async UniTask PerformAttackPhase(CancellationToken token)
        {
            OnAuraPhaseStarted?.Invoke(_upgradedAoeAttackSettings.AttackRadius);

            float animSpeed = InitialAttackSettings.TriggerActivityTimeSec / _upgradedAoeAttackSettings.TriggerActivityTimeSec;
            if (_animatorController != null)
            {
                _animatorController.SetAttackSpeedMultiplier(animSpeed);
            }

            float pulseTime = _upgradedAoeAttackSettings.TriggerActivityTimeSec;
            int attackCount = Mathf.FloorToInt(_upgradedAoeAttackSettings.FullDurationTimeSec / pulseTime);
            if (attackCount < 1) attackCount = 1;

            for (int i = 0; i < attackCount; i++)
            {
                FireWeapon();
                await UniTask.WaitForSeconds(pulseTime, cancellationToken: token);
            }

            OnAuraPhaseEnded?.Invoke();
        }

        protected override void FireWeapon()
        {
            if (_animatorController != null)
            {
                _animatorController.TriggerAttackAnimation();
            }
            else
            {
                ApplyDamageOnHit();
            }
        }

        private void ApplyDamageOnHit()
        {
            BaseAttackImpact.Shake?.Activate();

            Vector2 center = transform.position;
            float radius = _upgradedAoeAttackSettings.AttackRadius;

            ContactFilter2D filter = new ContactFilter2D
            {
                useTriggers = false,
                useLayerMask = true,
                layerMask = _enemyLayerMask
            };

            int hitCount = UnityEngine.Physics2D.OverlapCircle(center, radius, filter, _hitBuffer);

            for (int i = 0; i < hitCount; i++)
            {
                Collider2D targetCollider = _hitBuffer[i];
                HitTargetListener(this, new AttackTriggerArgs(targetCollider));
            }
        }

        public override void ApplyAttackRadiusUpgrade(float multiplier)
        {
            base.ApplyAttackRadiusUpgrade(multiplier);
            OnAttackRadiusUpgraded?.Invoke(_upgradedAoeAttackSettings.AttackRadius);
        }

        #region Debug

        private void OnDrawGizmos()
        {
            float defaultRadius = 0.5f; // 0.5 coz diameter = 0.5+0.5=1 (1 game unit)

            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            float radius = Application.isPlaying && _upgradedAoeAttackSettings != null
                ? _upgradedAoeAttackSettings.AttackRadius
                : defaultRadius;

            Gizmos.DrawWireSphere((Vector2)transform.position, radius);
        }

        #endregion
    }
}
