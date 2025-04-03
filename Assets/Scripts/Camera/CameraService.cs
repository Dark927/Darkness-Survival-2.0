using Characters.Player;
using Cinemachine;
using Settings.Global;
using UnityEngine;

namespace Settings.CameraManagement
{
    public sealed class CameraService : IService, IInitializable
    {
        #region Fields 

        private CinemachineVirtualCamera _virtualCamera;
        private CameraShake _cameraShake;

        #endregion


        #region Properties

        public CameraShake Shake => _cameraShake;

        #endregion


        #region Methods

        #region Init

        public CameraService(CinemachineVirtualCamera virtualCamera)
        {
            _virtualCamera = virtualCamera;
        }

        public void Initialize()
        {
            _cameraShake = new CameraShake(_virtualCamera);
        }


        #endregion

        public void FollowPlayer(PlayerCharacterController player)
        {
            if (_virtualCamera != null)
            {
                Transform playerTransform = player.CharacterLogic.Body.Transform;
                _virtualCamera.ForceCameraPosition(playerTransform.position, Quaternion.identity);
                _virtualCamera.PreviousStateIsValid = false;
                _virtualCamera.Follow = playerTransform;
            }
        }

        public void ResetPosition()
        {
            if (_virtualCamera != null)
            {
                _virtualCamera.ForceCameraPosition(Vector2.zero, Quaternion.identity);
            }
        }

        #endregion
    }
}
