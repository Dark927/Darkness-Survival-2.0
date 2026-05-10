using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    /// <summary>
    /// Listens to the AutonomousEntity lifecycle and handles visual toggling.
    /// </summary>
    public class OrbitalEntityVisuals : AutonomousEntityVisuals, IElementWithExtraVisual
    {
        [SerializeField] private TrailRenderer _trailRenderer;

        protected override void ShowVisuals()
        {
            base.ShowVisuals();

            if (_trailRenderer != null)
            {
                _trailRenderer.Clear();
            }
        }
    }
}
