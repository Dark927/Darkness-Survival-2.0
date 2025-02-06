
using UnityEngine;

namespace Settings.CameraManagement
{
    public sealed class CameraFollowLogic : MonoBehaviour
    {
        [SerializeField] private Transform _targetToFollow;
        private Camera _camera;

        private Vector3 _roundPosition;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void FixedUpdate()
        {
            DefaultFollow();
        }

        private void PixelPerfectFollow()
        {
            _roundPosition = new Vector3(
            PixelCalculator.RoundToNearestPixel(_targetToFollow.position.x, _camera),
            PixelCalculator.RoundToNearestPixel(_targetToFollow.position.y, _camera),
            transform.position.z);

            transform.position = _roundPosition;
        }

        private void DefaultFollow()
        {
            transform.position = new Vector3(_targetToFollow.position.x, _targetToFollow.position.y, transform.position.z);
        }
    }
}