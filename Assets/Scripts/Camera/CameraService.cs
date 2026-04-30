using System;
using Characters.Player;
using Cinemachine;
using Settings.Global;
using UnityEngine;

namespace Settings.CameraManagement
{
    public sealed class CameraService : IService, IInitializable
    {
        #region Fields 

        private CinemachineVirtualCamera _playerCamera;
        private CinemachineVirtualCamera _introCamera;
        private CameraShake _cameraShake;

        public event Action OnCameraStartFollowPlayer;
        public event Action OnCameraReset;

        private Vector3 _cameraDefaultPosition = new Vector3(0, 0, -10);

        #endregion

        #region Properties

        public CameraShake Shake => _cameraShake;
        public CinemachineVirtualCamera MainCamera => _playerCamera;

        #endregion

        #region Methods

        #region Init

        public CameraService(CinemachineVirtualCamera playerCamera, CinemachineVirtualCamera introCamera)
        {
            _playerCamera = playerCamera;
            _introCamera = introCamera;

            _introCamera.ForceCameraPosition(_cameraDefaultPosition, Quaternion.identity);
            _playerCamera.ForceCameraPosition(_cameraDefaultPosition, Quaternion.identity);
        }

        public void Initialize()
        {
            _cameraShake = new CameraShake(_playerCamera);
        }

        #endregion

        public void FollowPlayer(PlayerCharacterController player)
        {
            if (_playerCamera != null && _introCamera != null)
            {
                // Assign the player to the gameplay camera
                Transform playerTransform = player.CharacterLogic.Body.Transform;
                _playerCamera.Follow = playerTransform;

                /*
                    The CinemachineBrain will see this camera died, drop down to the Player Camera, 
                    and automatically create a cinematic blend
                */
                _introCamera.gameObject.SetActive(false);

                OnCameraStartFollowPlayer?.Invoke();
            }
        }

        public void ResetCamera()
        {
            if (_playerCamera != null && _introCamera != null)
            {
                _playerCamera.Follow = null;

                // Turn the Intro Camera back ON.
                _introCamera.gameObject.SetActive(true);

                _playerCamera.ForceCameraPosition(_introCamera.transform.position, Quaternion.identity);
                _playerCamera.PreviousStateIsValid = false;

                OnCameraReset?.Invoke();
            }
        }

        #endregion
    }
}
