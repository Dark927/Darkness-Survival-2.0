using UnityEngine;

namespace Visuals.Effects.Animations
{
    /// <summary>
    /// Attach to any child object to prevent it from flipping or rotating 
    /// when the parent character turns around.
    /// </summary>
    public class VisualStabilizer : MonoBehaviour
    {
        [Header("Stabilization Settings")]
        [Tooltip("Prevents the object from rotating when the parent rotates.")]
        [SerializeField] private bool _lockRotation = true;

        [Tooltip("Prevents the object from mirroring/flipping when the parent's scale becomes negative.")]
        [SerializeField] private bool _lockScale = true;

        private Quaternion _startGlobalRotation;

        private void Awake()
        {
            // Remember exactly how we were rotated when spawned
            _startGlobalRotation = transform.rotation;
        }

        private void LateUpdate()
        {
            if (_lockRotation)
            {
                HandleRotation();
            }

            if (_lockScale)
            {
                HandleFlipping();
            }
        }

        // Counter-act Parent Rotation
        private void HandleRotation()
        {
            transform.rotation = _startGlobalRotation;
        }

        // Counter-act Parent Flipping (Negative Scale)
        private void HandleFlipping()
        {
            Vector3 currentWorldScale = transform.lossyScale;
            Vector3 correctedLocalScale = transform.localScale;

            // If the parent flipped us on the X axis, we flip ourselves back
            if (currentWorldScale.x < 0)
            {
                correctedLocalScale.x *= -1;
            }

            // If the parent flipped us on the Y axis, we flip ourselves back
            if (currentWorldScale.y < 0)
            {
                correctedLocalScale.y *= -1;
            }

            transform.localScale = correctedLocalScale;
        }
    }
}
