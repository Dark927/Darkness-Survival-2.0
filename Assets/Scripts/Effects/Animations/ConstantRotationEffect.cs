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

        [Header("Start Rotation")]
        [SerializeField] private bool _useRandomStartRotation = false;

        [Tooltip("If random is false, the object will start at this exact angle when enabled.")]
        [SerializeField] private float _customStartAngle = 0f;

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

        private void OnEnable()
        {
            if (_targetTransform == null) return;

            // Determine the starting angle
            float startAngle = _useRandomStartRotation ? Random.Range(0f, 360f) : _customStartAngle;

            // Apply the initial rotation.
            _targetTransform.localRotation = Quaternion.AngleAxis(startAngle, _rotationAxis);
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

        public void SetCustomStartAngle(float angle)
        {
            _customStartAngle = angle;
            _useRandomStartRotation = false;

            // Force apply immediately if requested via code while active
            if (gameObject.activeInHierarchy && _targetTransform != null)
            {
                _targetTransform.localRotation = Quaternion.AngleAxis(_customStartAngle, _rotationAxis);
            }
        }
    }
}
