using UnityEngine;
using Utilities.ErrorHandling;

namespace Settings.CameraManagement
{
    public class CameraOverlay : MonoBehaviour
    {
        [SerializeField] private float _offsetZ = 2f;
        private Transform _targetCameraTransform;
        private Vector3 _targetPosition;

        private void Awake()
        {
            UpdateCameraReference();
        }

        private void LateUpdate()
        {
            if (_targetCameraTransform == null)
            {
                UpdateCameraReference();
                return;
            }

            _targetPosition = _targetCameraTransform.position;
            _targetPosition.z += _offsetZ;
            transform.position = _targetPosition;
        }

        private void UpdateCameraReference()
        {
            _targetCameraTransform = Camera.main.transform;

            if (_targetCameraTransform == null)
            {
                ErrorLogger.LogWarning("No camera found for overlay - " + gameObject.name);
            }
        }
    }
}
