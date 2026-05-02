using System.Collections.Generic;
using Characters.Common;
using Characters.Common.Abilities;
using Characters.Common.Features;
using Gameplay.Components.Items;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Characters.Player.Abilities
{
    public class SoulMagnetAbility : MonoBehaviour, ISoulMagnetAbility
    {
        #region Fields 

        private const int MAX_ITEMS_TO_GRAB_PER_FRAME = 30;
        private const float MIN_DISTANCE_TO_PICK_UP_SQR = 0.04f;

        [SerializeField] private IEntityFeature.TargetEntityPart _entityConnectionPart;

        [Header("Optimization")]
        [Tooltip("Assign the layer XP Orbs are on to save performance.")]
        [SerializeField] private LayerMask _xpItemLayer;

        [Header("Visuals")]
        [Tooltip("Assign a child object with a SpriteRenderer (Soft Circle) to show the magnet area.")]
        [SerializeField] private Transform _auraVisualTransform;

        // Base Stats
        private float _baseRadius = 0f;
        private float _basePullSpeed = 0f;
        private float _basePullDuration = 0f;

        // Upgraded Multipliers
        private float _radiusMultiplier = 1f;
        private float _pullSpeedMultiplier = 1f;
        private float _pullDurationMultiplier = 1f;
        private float _xpBonusMultiplier = 0f;

        private ICharacterLogic _ownerLogic;
        private Transform _ownerTransform;
        private Collider2D _ownerCollider;

        // Performance caching (Zero Allocation tracking)
        private Collider2D[] _overlapResults = new Collider2D[MAX_ITEMS_TO_GRAB_PER_FRAME];
        private List<BasicItemXp> _grabbedSouls = new List<BasicItemXp>(MAX_ITEMS_TO_GRAB_PER_FRAME);
        private List<float> _grabbedSoulTimers = new List<float>(MAX_ITEMS_TO_GRAB_PER_FRAME);

        private bool _isHijackingForBonusXp => _xpBonusMultiplier > 0f;

        // Helper property to dynamically get the center of the player's body rather than their feet
        private Vector3 PullCenter => _ownerCollider != null ? _ownerCollider.bounds.center : _ownerTransform.position;

        #endregion

        #region Properties

        public IEntityFeature.TargetEntityPart EntityConnectionPart => _entityConnectionPart;
        public GameObject RootObject => gameObject;

        #endregion

        #region Init

        public void Initialize(IEntityDynamicLogic characterLogic)
        {
            if (characterLogic is not ICharacterLogic myCharacterLogic)
            {
                ErrorLogger.Log($"{characterLogic} does not implement {nameof(ICharacterLogic)} | Deactivating Soul Magnet.");
                gameObject.SetActive(false);
                return;
            }

            _ownerLogic = myCharacterLogic;
            _ownerTransform = _ownerLogic.Body.Transform;

            // Cache the collider. We cast to Collider2D just in case Physics.Collider returns a generic type
            _ownerCollider = _ownerLogic.Body.Physics.Collider as Collider2D;
        }

        public void Dispose()
        {
            _grabbedSouls.Clear();
            _grabbedSoulTimers.Clear();
        }

        #endregion

        #region IUpgradableAbility Implementation

        public void ApplyRadiusUpgrade(float multiplier)
        {
            _radiusMultiplier += multiplier;
        }

        public void ApplyStrengthUpgrade(float multiplier)
        {
            _pullSpeedMultiplier += multiplier;
        }

        public void ApplyDurationUpgrade(float multiplier)
        {
            _pullDurationMultiplier += multiplier;
        }

        public void ApplyXpBonusUpgrade(float multiplier)
        {
            _xpBonusMultiplier += multiplier;
        }

        public void SetStaticStats(AbilityStats abilityStats)
        {
            _basePullSpeed = abilityStats.StrengthValue;
            _baseRadius = abilityStats.Radius;
            _basePullDuration = abilityStats.Duration;
        }

        #endregion

        #region Magnet Logic

        private void Update()
        {
            if (_ownerTransform == null)
            {
                return;
            }

            DetectSouls(_isHijackingForBonusXp);
            PullSouls(_isHijackingForBonusXp);

            UpdateVisualAura();
        }

        private void DetectSouls(bool isHijackingForBonusXp)
        {
            float currentRadius = _baseRadius * _radiusMultiplier;

            int hitCount = Physics2D.OverlapCircleNonAlloc(PullCenter, currentRadius, _overlapResults, _xpItemLayer);

            for (int i = 0; i < hitCount; i++)
            {
                if (_overlapResults[i].TryGetComponent(out BasicItemXp xpItem))
                {
                    if (!_grabbedSouls.Contains(xpItem))
                    {
                        _grabbedSouls.Add(xpItem);
                        _grabbedSoulTimers.Add(0f);

                        if (isHijackingForBonusXp)
                        {
                            _overlapResults[i].enabled = false;
                        }
                    }
                }
            }
        }

        private void PullSouls(bool isHijackingForBonusXp)
        {
            float currentSpeed = _basePullSpeed * _pullSpeedMultiplier;
            float currentMaxDuration = _basePullDuration * _pullDurationMultiplier;

            // Cache the center point once per frame so we don't calculate bounds for every single item
            Vector3 targetCenter = PullCenter;

            for (int i = _grabbedSouls.Count - 1; i >= 0; i--)
            {
                BasicItemXp soul = _grabbedSouls[i];

                if (soul == null || !soul.gameObject.activeInHierarchy)
                {
                    _grabbedSouls.RemoveAt(i);
                    _grabbedSoulTimers.RemoveAt(i);
                    continue;
                }

                _grabbedSoulTimers[i] += Time.deltaTime;
                float timePulling = _grabbedSoulTimers[i];

                float remainingTime = Mathf.Max(0.01f, currentMaxDuration - timePulling);

                // Measure distance to the collider's center
                float distanceToPlayer = Vector3.Distance(soul.transform.position, targetCenter);
                float guaranteedSpeed = distanceToPlayer / remainingTime;
                float finalPullSpeed = Mathf.Max(currentSpeed, guaranteedSpeed);

                // Pull toward the collider's center
                soul.transform.position = Vector3.MoveTowards(soul.transform.position, targetCenter, finalPullSpeed * Time.deltaTime);

                if (isHijackingForBonusXp)
                {
                    // Check squared distance against the collider's center
                    if ((soul.transform.position - targetCenter).sqrMagnitude < MIN_DISTANCE_TO_PICK_UP_SQR)
                    {
                        soul.Pickup(_ownerLogic);

                        float bonusXp = soul.XpAmount * _xpBonusMultiplier;
                        _ownerLogic.Level.AddXp((int)bonusXp);

                        _grabbedSouls.RemoveAt(i);
                        _grabbedSoulTimers.RemoveAt(i);

                        Destroy(soul.gameObject);
                    }
                }
            }
        }

        private void UpdateVisualAura()
        {
            if (_auraVisualTransform != null)
            {
                // Follow the exact center where souls are pulled
                _auraVisualTransform.position = PullCenter;

                float currentRadius = _baseRadius * _radiusMultiplier;

                // Unity standard shapes are 1x1 unit in size. 
                // To match a radius, we scale the diameter (radius * 2)
                float diameter = currentRadius * 2f;

                _auraVisualTransform.localScale = new Vector3(diameter, diameter, 1f);
            }
        }

        #endregion

        #region Debugging

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.7f, 0f, 1f, 0.4f);

            // Dynamically draw from the collider center if active, otherwise fallback to transform
            if (Application.isPlaying && _ownerTransform != null)
            {
                Gizmos.DrawWireSphere(PullCenter, _baseRadius * _radiusMultiplier);
            }
            else
            {
                Gizmos.DrawWireSphere(transform.position, _baseRadius);
            }
        }

        #endregion
    }
}
