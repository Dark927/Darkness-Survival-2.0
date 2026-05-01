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
        private float _currentHealPercentageTotal;

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
            _ownerLogic.OnEnemyKilled -= ListenEnemyKilled;
        }

        #endregion

        #region IUpgradableAbility Implementation

        public void ApplyStrengthUpgrade(float incomingPercentage)
        {
            _currentHealPercentageTotal += incomingPercentage;

            //Debug.Log($"[DemonicAbsorption] Heal Percent Upgraded! New Total: {_currentHealPercentageTotal}% HP per kill.");
        }

        public void ApplyRadiusUpgrade(float percent)
        {
            // Ignored: Demonic Absorption does not use Radius
        }

        public void ApplyDurationUpgrade(float percent)
        {
            // Ignored: Demonic Absorption does not use Duration
        }

        #endregion

        private void ListenEnemyKilled(object sender, IEntityDynamicLogic killedEnemy)
        {
            // Example total is 8.5% (Heal 8.5% of max health per kill)
            // actualHealMultiplier = 8.5f / 100f = 0.085f
            float actualHealMultiplier = _currentHealPercentageTotal / 100f;

            // Calculate the exact heal amount based on player's Max Health
            // Max Health 1000 * 0.085f = Heal 85HP per kill.
            float healAmount = _ownerHealth.MaxHp * actualHealMultiplier;

            _ownerHealth.Heal(healAmount);
        }

        public void SetStaticStats(AbilityStats abilityStats)
        {
            _currentHealPercentageTotal = abilityStats.Strength;
        }

        #endregion
    }
}
