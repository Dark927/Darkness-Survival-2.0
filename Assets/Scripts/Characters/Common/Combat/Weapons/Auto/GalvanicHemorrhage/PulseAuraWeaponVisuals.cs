using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Universal visual handler for Pulse Aura Weapons.
    /// Scales and toggles aura visuals based on the weapon's phase and radius events.
    /// </summary>
    public class PulseAuraWeaponVisuals : MonoBehaviour, IElementWithExtraVisual
    {
        [Header("Logic Link")]
        [Tooltip("Link the universal aura weapon logic here.")]
        [SerializeField] private PulseAuraWeapon _weaponLogic;

        [Header("Visual Elements")]
        [SerializeField] private GameObject _baseAuraVisual;
        [SerializeField] private GameObject _specialAuraVisual;

        private bool _isSpecialAuraUnlocked = false;
        public bool IsSpecialVisualActive => _isSpecialAuraUnlocked;

        private void Awake()
        {
            // Ensure visuals are strictly off before any events can fire
            HideAura();
        }

        private void OnEnable()
        {
            if (_weaponLogic == null) return;

            _weaponLogic.OnAuraPhaseStarted += ShowAura;
            _weaponLogic.OnAuraPhaseEnded += HideAura;
            _weaponLogic.OnAttackRadiusUpgraded += UpdateAuraRadius;
            _weaponLogic.OnSpecialAuraUnlockedEvent += EnableSpecialVisual;
            _weaponLogic.OnSpecialAuraLockedEvent += DisableSpecialVisual;
        }

        private void OnDisable()
        {
            if (_weaponLogic == null) return;

            _weaponLogic.OnAuraPhaseStarted -= ShowAura;
            _weaponLogic.OnAttackRadiusUpgraded -= UpdateAuraRadius;
            _weaponLogic.OnAuraPhaseEnded -= HideAura;
            _weaponLogic.OnSpecialAuraUnlockedEvent -= EnableSpecialVisual;
            _weaponLogic.OnSpecialAuraLockedEvent -= DisableSpecialVisual;
        }

        private void ShowAura(float radius)
        {
            UpdateAuraRadius(radius);

            if (_baseAuraVisual != null) _baseAuraVisual.SetActive(true);

            if (_isSpecialAuraUnlocked && _specialAuraVisual != null)
            {
                _specialAuraVisual.SetActive(true);
            }
        }

        private void UpdateAuraRadius(float radius)
        {
            if (_baseAuraVisual != null)
            {
                UpdateVisualElementRadius(_baseAuraVisual.transform, radius);
            }
            if (_isSpecialAuraUnlocked && _specialAuraVisual != null)
            {
                UpdateVisualElementRadius(_specialAuraVisual.transform, radius);
            }
        }

        private void HideAura()
        {
            if (_baseAuraVisual != null) _baseAuraVisual.SetActive(false);
            if (_specialAuraVisual != null) _specialAuraVisual.SetActive(false);
        }

        private void UpdateVisualElementRadius(Transform visualElementTransform, float currentRadius)
        {
            // Converts radius to diameter for localScale
            float diameter = currentRadius * 2f;
            visualElementTransform.localScale = new Vector3(diameter, diameter, 1f);
        }

        public void EnableSpecialVisual()
        {
            _isSpecialAuraUnlocked = true;
        }

        public void DisableSpecialVisual()
        {
            _isSpecialAuraUnlocked = false;
        }
    }
}
