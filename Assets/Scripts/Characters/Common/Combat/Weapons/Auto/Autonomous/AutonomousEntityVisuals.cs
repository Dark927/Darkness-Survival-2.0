using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Listens to the AutonomousEntity lifecycle and handles visual toggling.
    /// </summary>
    public class AutonomousEntityVisuals : MonoBehaviour, IElementWithExtraVisual
    {
        [Header("Logic Link")]
        [Tooltip("The logic component on this prefab.")]
        [SerializeField] private AutonomousEntityBase _entityLogic;

        [Header("Visual Elements")]
        [SerializeField] private GameObject _baseVisual;
        [SerializeField] private GameObject _extraVisual;

        // Needs to be updated by the weapon when the upgrade is acquired.
        private bool _isExtraVisualActive = false;

        public bool IsSpecialVisualActive => _isExtraVisualActive;

        private void OnEnable()
        {
            if (_entityLogic == null) return;

            _entityLogic.OnEntityActivated += ShowVisuals;
            _entityLogic.OnEntityDied += HideVisuals;
        }

        private void OnDisable()
        {
            if (_entityLogic == null) return;

            _entityLogic.OnEntityActivated -= ShowVisuals;
            _entityLogic.OnEntityDied -= HideVisuals;
        }

        private void Start()
        {
            HideVisuals();
        }

        private void ShowVisuals()
        {
            if (_baseVisual != null) _baseVisual.SetActive(true);

            if (_isExtraVisualActive && _extraVisual != null)
            {
                _extraVisual.SetActive(true);
            }
        }

        private void HideVisuals()
        {
            if (_baseVisual != null) _baseVisual.SetActive(false);
            if (_extraVisual != null) _extraVisual.SetActive(false);
        }

        public void EnableSpecialVisual()
        {
            _isExtraVisualActive = true;
        }

        public void DisableSpecialVisual()
        {
            _isExtraVisualActive = false;
        }
    }
}
