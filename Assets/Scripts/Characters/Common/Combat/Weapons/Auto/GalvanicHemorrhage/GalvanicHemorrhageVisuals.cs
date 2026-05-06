using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Handles only the visual GameObjects/Particles for the Galvanic Hemorrhage.
    /// Completely separated from combat math and physics.
    /// </summary>
    public class GalvanicHemorrhageVisuals : MonoBehaviour
    {
        [Header("Logic Link")]
        [Tooltip("Link the weapon logic component here so we can listen to its events.")]
        [SerializeField] private GalvanicHemorrhageWeapon _weaponLogic;

        [Header("Visual Elements")]
        [SerializeField] private GameObject _baseAuraVisual;
        [SerializeField] private GameObject _bloodAuraVisual;

        private bool _isBloodAuraUnlocked = false;

        private void OnEnable()
        {
            if (_weaponLogic == null) return;

            // Subscribe to the weapon's state changes
            _weaponLogic.OnAuraPhaseStarted += ShowAura;
            _weaponLogic.OnAuraPhaseEnded += HideAura;
            _weaponLogic.OnAuraUnlockedEvent += EnableBloodAuraState;
            _weaponLogic.OnAttackRadiusUpgraded += UpdateAuraRadius;
        }

        private void OnDisable()
        {
            if (_weaponLogic == null) return;

            _weaponLogic.OnAuraPhaseStarted -= ShowAura;
            _weaponLogic.OnAttackRadiusUpgraded -= UpdateAuraRadius;
            _weaponLogic.OnAuraPhaseEnded -= HideAura;
            _weaponLogic.OnAuraUnlockedEvent -= EnableBloodAuraState;
        }

        private void Start()
        {
            // Ensure visuals are off by default
            HideAura();
        }

        private void ShowAura(float radius)
        {
            UpdateAuraRadius(radius);

            if (_baseAuraVisual != null)
            {
                _baseAuraVisual.SetActive(true);
            }
            if (_isBloodAuraUnlocked && _bloodAuraVisual != null)
            {
                _bloodAuraVisual.SetActive(true);
            }
        }

        private void UpdateAuraRadius(float radius)
        {
            if (_baseAuraVisual != null)
            {
                UpdateVisualElementRadius(_baseAuraVisual.transform, radius);
            }
            if (_isBloodAuraUnlocked && _bloodAuraVisual != null)
            {
                UpdateVisualElementRadius(_bloodAuraVisual.transform, radius);
            }
        }

        private void HideAura()
        {
            if (_baseAuraVisual != null) _baseAuraVisual.SetActive(false);
            if (_bloodAuraVisual != null) _bloodAuraVisual.SetActive(false);
        }

        private void UpdateVisualElementRadius(Transform visualElementTransform, float currentRadius)
        {
            if (visualElementTransform != null)
            {
                float diameter = currentRadius * 2;
                visualElementTransform.localScale = new Vector3(diameter, diameter, 1f);
            }
        }

        private void EnableBloodAuraState()
        {
            _isBloodAuraUnlocked = true;
        }
    }
}
