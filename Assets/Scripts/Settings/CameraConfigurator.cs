using Cinemachine;
using UnityEngine;
using Zenject;

public sealed class CameraConfigurator : MonoBehaviour
{
    private ICharacterLogic _playerLogic;
    private CinemachineVirtualCamera _virtualCamera;


    [Inject]
    public void Construct(ICharacterLogic playerLogic, CinemachineVirtualCamera virtualCamera)
    {
        _playerLogic = playerLogic;
        _virtualCamera = virtualCamera;
    }


    private void Awake()
    {
        if (_virtualCamera != null)
        {
            Transform playerTransform = ((MonoBehaviour)_playerLogic).transform;
            _virtualCamera.Follow = playerTransform;
        }
    }

}
