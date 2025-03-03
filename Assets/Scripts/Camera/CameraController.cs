using System;
using Characters.Player;
using Cinemachine;
using Settings.Global;

namespace Settings.CameraManagement
{
    public sealed class CameraController : IService, IDisposable
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

        public CameraController(CinemachineVirtualCamera virtualCamera)
        {
            _virtualCamera = virtualCamera;
        }

        public void Init()
        {
            _cameraShake = new CameraShake(_virtualCamera);
            ServiceLocator.Current.Get<PlayerService>().OnPlayerReady += FollowPlayer;
        }

        #endregion

        public void FollowPlayer(PlayerCharacterController player)
        {
            if (_virtualCamera != null)
            {
                _virtualCamera.Follow = player.CharacterLogic.Body.Transform;
            }
        }

        public void Dispose()
        {
            ServiceLocator.Current.Get<PlayerService>().OnPlayerReady -= FollowPlayer;
        }

        #endregion
    }
}
