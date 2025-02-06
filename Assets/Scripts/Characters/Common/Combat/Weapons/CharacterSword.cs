using Settings.CameraManagement.Shake;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public class CharacterSword : CharacterWeaponBase
    {
        #region Enums

        public enum AttackType
        {
            Fast = 1,
            Heavy = 2,
        }

        #endregion


        #region Fields 

        private List<SwordAttackTrigger> _attackTriggers;

        private ShakeImpact _fastAttackImpact;
        private ShakeImpact _heavyAttackImpact;
        private SwordAttackSettings _swordAttackSettings;

        #endregion


        #region Properties
        public SwordAttackSettings AttackSettings => _swordAttackSettings;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            try
            {
                _swordAttackSettings = (SwordAttackSettings)AttackData.AttackSettings;
            }
            catch (Exception ex)
            {
                Debug.LogError($"# Can not convert the {(AttackData.AttackSettings.GetType())} to {nameof(SwordAttackSettings)}! Settings is NULL!");
                Debug.LogException(ex);
            }

            _attackTriggers = GetComponentsInChildren<SwordAttackTrigger>().ToList();
            _fastAttackImpact = new ShakeImpact(AttackSettings.FastShakeSettings);
            _heavyAttackImpact = new ShakeImpact(AttackSettings.HeavyShakeSettings);

            foreach (SwordAttackTrigger trigger in _attackTriggers)
            {
                trigger.OnTriggerEnter += HitEnemyListener;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (SwordAttackTrigger trigger in _attackTriggers)
            {
                trigger.OnTriggerEnter -= HitEnemyListener;
            }
        }

        #endregion

        public void TriggerAttack(AttackType attackType)
        {
            SwordAttackTrigger attackTrigger = _attackTriggers.FirstOrDefault(trigger => trigger.TargetAttackType == attackType);

            if (attackTrigger != null)
            {
                attackTrigger.Activate(AttackSettings.TriggerActivityTimeSec);
            }

            ActivateImpact(attackType);
        }

        private void ActivateImpact(AttackType attackType)
        {
            switch (attackType)
            {
                case AttackType.Fast:
                    _fastAttackImpact.Activate();
                    break;


                case AttackType.Heavy:
                    _heavyAttackImpact.Activate();
                    break;


                default:
                    throw new NotImplementedException();
            }
        }

        private void HitEnemyListener(object sender, EventArgs args)
        {
            SwordAttackTriggerArgs attackArgs = (SwordAttackTriggerArgs)args;
            GameObject enemyObject = attackArgs.TargetCollider.gameObject;

            Debug.Log("HIT -> " + enemyObject.name);
            Destroy(enemyObject);
        }


        #endregion

    }
}
