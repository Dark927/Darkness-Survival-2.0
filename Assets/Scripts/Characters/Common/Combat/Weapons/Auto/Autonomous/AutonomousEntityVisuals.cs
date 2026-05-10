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
        private bool _isMainVisualActive = false;

        private bool CanActivateExtraVisual => _isMainVisualActive && _isExtraVisualActive;

        public bool IsSpecialVisualActive => _isExtraVisualActive;

        protected void OnEnable()
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

        private void Awake()
        {
            HideVisuals();
        }

        protected virtual void ShowVisuals()
        {
            if (_baseVisual != null)
            {
                _isMainVisualActive = true;
                _baseVisual.SetActive(true);
            }

            ShowExtraVisuals();
        }

        protected virtual void HideVisuals()
        {
            if (_baseVisual != null)
            {
                _isMainVisualActive = false;
                _baseVisual.SetActive(false);
            }
            HideExtraVisuals();
        }

        protected virtual void ShowExtraVisuals()
        {
            if (CanActivateExtraVisual && _extraVisual != null)
            {
                _extraVisual.SetActive(true);
            }
        }

        protected virtual void HideExtraVisuals()
        {
            if (_extraVisual != null) _extraVisual.SetActive(false);
        }

        public void EnableSpecialVisual()
        {
            _isExtraVisualActive = true;
            ShowExtraVisuals();
        }

        public void DisableSpecialVisual()
        {
            _isExtraVisualActive = false;
            HideExtraVisuals();
        }
    }
}
