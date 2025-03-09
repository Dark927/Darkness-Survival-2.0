using Characters.Player;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public sealed class CharactersInstaller : MonoInstaller
{
    [SerializeField] private AssetReference _characterAsset;
    [SerializeField] private Transform _parentContainer;

    // ToDo : implement addressable assets handler to release all assets when stage is closed, etc.
    private AsyncOperationHandle<GameObject> _characterLoadHandle;

    public override void InstallBindings()
    {
        BindPlayerCharacter();
    }

    private void BindPlayerCharacter()
    {
        AsyncOperationHandle<GameObject> handle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<GameObject>(_characterAsset);
        handle.WaitForCompletion();

        // ToDo : remove later

        _characterLoadHandle = handle;

        GameObject playerPrefab = handle.Result;
        GameObject playerObject = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity, _parentContainer);
        playerObject.name = playerPrefab.name;
        PlayerCharacterController characterController = playerObject.GetComponent<PlayerCharacterController>();

        Container
            .Bind<PlayerCharacterController>()
            .FromInstance(characterController)
            .AsSingle();
    }

    private void OnDestroy()
    {
        if (_characterLoadHandle.IsValid())
        {
            AddressableAssetsLoader.Instance.UnloadAsset(_characterLoadHandle);
        }
    }
}
