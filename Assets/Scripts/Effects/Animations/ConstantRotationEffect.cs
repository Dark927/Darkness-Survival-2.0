using UnityEngine;

namespace Visuals.Effects.Animations
{
    /// <summary>
    /// A reusable component that constantly rotates a Transform over time.
    /// Perfect for auras, projectiles, etc.
    /// </summary>
    public class ConstantRotationEffect : MonoBehaviour
    {
        [Header("Rotation Settings")]
        [Tooltip("The speed of rotation in degrees per second.")]
        [SerializeField] private float _rotationSpeed = 90f;

        [Tooltip("The axis to rotate around. For 2D games (0, 0, 1).")]
        [SerializeField] private Vector3 _rotationAxis = new Vector3(0f, 0f, 1f);

        [Header("Target (Optional)")]
        [Tooltip("Leave empty to rotate this GameObject, or assign a specific child to rotate.")]
        [SerializeField] private Transform _targetTransform;

        private void Awake()
        {
            // If no target is assigned, default to the object this script is attached to
            if (_targetTransform == null)
            {
                _targetTransform = transform;
            }

            // Normalize the axis to ensure consistent speed math
            _rotationAxis.Normalize();
        }

        private void Update()
        {
            if (_targetTransform != null)
            {
                // Rotate smoothly based on time
                _targetTransform.Rotate(_rotationAxis, _rotationSpeed * Time.deltaTime);
            }
        }

        // Methods to dynamically change speed via code
        public void SetRotationSpeed(float newSpeed)
        {
            _rotationSpeed = newSpeed;
        }

        public void InvertDirection()
        {
            _rotationSpeed = -_rotationSpeed;
        }
    }
}
