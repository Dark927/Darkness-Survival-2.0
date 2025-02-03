using Characters.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

public sealed class CameraConfigurator : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;


    [Inject]
    public void Construct(CinemachineVirtualCamera virtualCamera)
    {
        _virtualCamera = virtualCamera;
    }

    private void Awake()
    {
        GameManager.Instance.OnPlayerReady += FollowPlayer;
    }

    public void FollowPlayer(Player player)
    {
        if (_virtualCamera != null)
        {
            _virtualCamera.Follow = player.Character.Body.transform;
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnPlayerReady -= FollowPlayer;
    }

}
