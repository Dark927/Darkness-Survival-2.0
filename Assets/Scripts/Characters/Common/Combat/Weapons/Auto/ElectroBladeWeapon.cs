using Characters.Common.Combat.Weapons.Data;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class ElectroBladeWeapon : UpgradableAutoWeaponBase, IWeaponWithSpecialAttack
    {
        [Header("Electro Blade Triggers")]
        [SerializeField] private AttackTrigger _frontTrigger;
        [SerializeField] private AttackTrigger _backTrigger;

        [Header("Spatial Layout")]
        [Tooltip("Distance from the center point to place the attack triggers.")]
        [SerializeField] private float _triggerOffsetDistance = 1f;

        private bool _isSpecialAttackActive;

        public bool IsSpecialAttackActive => _isSpecialAttackActive;

        // helper to get the dynamically scaled time for a single attack pulse
        private float CurrentPulseTime => UpgradedAttackSettings.TriggerActivityTimeSec;

        public override void Initialize(WeaponAttackDataBase attackData)
        {
            base.Initialize(attackData);

            InitializeAttackTrigger(_frontTrigger, new Vector3(_triggerOffsetDistance, 0f, 0f));

            if (_backTrigger != null)
            {
                InitializeAttackTrigger(_backTrigger, new Vector3(-_triggerOffsetDistance, 0f, 0f));
            }
        }

        private void InitializeAttackTrigger(AttackTrigger trigger, Vector3 triggerOffset)
        {
            trigger.Initialize();
            trigger.transform.localPosition = triggerOffset;
            trigger.OnTriggerEnter += HitTargetListener;
        }

        public override void Dispose()
        {
            base.Dispose();
            DisposeAttackTrigger(_frontTrigger);

            if (_backTrigger != null)
            {
                DisposeAttackTrigger(_backTrigger);
            }
        }

        private void DisposeAttackTrigger(AttackTrigger trigger)
        {
            trigger.OnTriggerEnter -= HitTargetListener;
            trigger.Deactivate();
        }

        protected override async UniTask PerformAttackPhase(CancellationToken token)
        {
            float pulseTime = CurrentPulseTime;

            // calculate how many pulses fit into the total attack window
            int attackCount = Mathf.FloorToInt(UpgradedAttackSettings.FullDurationTimeSec / pulseTime);

            // guarantee at least one attack fires
            if (attackCount < 1) attackCount = 1;

            for (int i = 0; i < attackCount; i++)
            {
                FireWeapon();
                // wait for the physics/visual pulse to finish before firing again
                await UniTask.WaitForSeconds(pulseTime, cancellationToken: token);
            }

            // We don't need to wait leftover time after main pulses finished.
        }

        protected override void FireWeapon()
        {
            BaseAttackImpact.Shake?.Activate();

            // pass the dynamically calculated pulse time to the triggers
            _frontTrigger.Activate(CurrentPulseTime);

            if (IsSpecialAttackActive)
            {
                _backTrigger.Activate(CurrentPulseTime);
            }
        }

        public void EnableSpecialAttack() => _isSpecialAttackActive = true;
        public void DisableSpecialAttack() => _isSpecialAttackActive = false;
    }
}
