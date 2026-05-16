using UnityEngine;

namespace Characters.Common.Visual
{
    public class EntityMarkVisualPart : MonoBehaviour, IEntityCustomVisualPart
    {
        [Header("Visuals")]
        [Tooltip("The GameObject that holds the Sprite, Light, and Particles.")]
        [SerializeField] private GameObject _visualContainer;

        [Header("Positioning")]
        [Tooltip("How high above the target center the mark should float.")]
        [SerializeField] private Vector3 _localOffset = new Vector3(0, 1.5f, 0);


        public void Initialize(IEntityBody targetEntityBody)
        {
            transform.localPosition = _localOffset;

            // Ensure rotation stays fixed (so if the target flips/rotates, the rune stays upright)
            transform.localRotation = Quaternion.identity;

            // Turn on the visual elements
            if (_visualContainer != null)
            {
                _visualContainer.SetActive(true);
            }
        }

        public void Dispose()
        {
            // Turn off visuals immediately
            if (_visualContainer != null)
            {
                _visualContainer.SetActive(false);
            }

            // CLEANUP
            // ToDo : use Pool.Return(this) here later
            Destroy(gameObject);
        }
    }
}
