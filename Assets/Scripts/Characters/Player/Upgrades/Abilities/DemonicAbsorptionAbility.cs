using Characters.Common;
using Characters.Common.Abilities;
using Characters.Common.Features;
using Characters.Health;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Characters.Player.Upgrades
{
    public class DemonicAbsorptionAbility : MonoBehaviour, IUpgradableAbility
    {
        #region Fields 

        [SerializeField] private IEntityFeature.TargetEntityPart _entityConnectionPart;

        // An internal tracker for the total percentage Sum (5.0f + 1.25f + 2.25f = 8.5f)
        private float _currentHealMultiplierTotal = 0;

        private IAttackableEntityLogic _ownerLogic;
        private IHealth _ownerHealth;

        #endregion

        #region Properties

        public IEntityFeature.TargetEntityPart EntityConnectionPart => _entityConnectionPart;
        public GameObject RootObject => gameObject;

        #endregion

        #region Methods

        #region Init

        public void Initialize(IEntityDynamicLogic characterLogic)
        {
            if (characterLogic is not IAttackableEntityLogic attackableEntityLogic)
            {
                ErrorLogger.Log($"{characterLogic} does not implement {nameof(attackableEntityLogic)}, so this ability can not be used | Deactivating..");
                gameObject.SetActive(false);
                return;
            }

            _ownerLogic = attackableEntityLogic;
            _ownerHealth = _ownerLogic.Body.Health;
            _ownerLogic.OnEnemyKilled += ListenEnemyKilled;
        }

        public void Dispose()
        {
            Debug.Log("DISPOSE DDDDDD");
            Debug.Log("DISPOSE DDDDDD");
            Debug.Log("DISPOSE DDDDDD");
            Debug.Log("DISPOSE DDDDDD");
            Debug.Log("DISPOSE DDDDDD");
            _ownerLogic.OnEnemyKilled -= ListenEnemyKilled;
        }

        #endregion

        #region IUpgradableAbility Implementation

        public void ApplyStrengthUpgrade(float multiplier)
        {
            _currentHealMultiplierTotal += multiplier;

            //Debug.Log($"[DemonicAbsorption] Heal Percent Upgraded! New Total: {_currentHealPercentageTotal}% HP per kill.");
        }

        public void ApplyRadiusUpgrade(float multiplier)
        {
            // Ignored: Demonic Absorption does not use Radius
        }

        public void ApplyDurationUpgrade(float multiplier)
        {
            // Ignored: Demonic Absorption does not use Duration
        }

        #endregion

        private void ListenEnemyKilled(object sender, IEntityDynamicLogic killedEnemy)
        {
            // Calculate the exact heal amount based on player's Max Health
            // Max Health 1000 * 0.085f = Heal 85HP per kill.
            float healAmount = _ownerHealth.MaxHp * _currentHealMultiplierTotal;

            _ownerHealth.Heal(healAmount);
        }

        public void SetStaticStats(AbilityStats abilityStats)
        {
            _currentHealMultiplierTotal += (abilityStats.StrengthPercent / 100f);
        }

        #endregion
    }
}
