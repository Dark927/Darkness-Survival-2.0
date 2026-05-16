using UnityEngine;

namespace Visuals.Effects.Animations
{
    /// <summary>
    /// Sets a random initial rotation when the object is enabled.
    /// Perfect for decals, explosion particles, or static variations.
    /// </summary>
    public class RandomStartRotation : MonoBehaviour
    {
        [Tooltip("The axis to rotate around. For 2D games (0, 0, 1).")]
        [SerializeField] private Vector3 _rotationAxis = new Vector3(0f, 0f, 1f);

        private void Awake()
        {
            _rotationAxis.Normalize();
        }

        private void OnEnable()
        {
            // Set a random angle between 0 and 360
            float randomAngle = Random.Range(0f, 360f);
            transform.localRotation = Quaternion.AngleAxis(randomAngle, _rotationAxis);
        }
    }
}
