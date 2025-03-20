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

    public override void InstallBindings()
    {
        BindPlayerCharacter();
    }

    private void BindPlayerCharacter()
    {
        AsyncOperationHandle<GameObject> handle = AddressableAssetsHandler.Instance.TryLoadAssetAsync<GameObject>(_characterAsset);
        handle.WaitForCompletion();

        AddressableAssetsCleaner assetCleaner = AddressableAssetsHandler.Instance?.Cleaner;
        assetCleaner.SubscribeOnCleaning(AddressableAssetsCleaner.CleanType.SceneSwitch, handle);

        GameObject playerPrefab = handle.Result;
        GameObject playerObject = Instantiate(playerPrefab, Vector2.zero, Quaternion.identity, _parentContainer);
        playerObject.name = playerPrefab.name;
        PlayerCharacterController characterController = playerObject.GetComponent<PlayerCharacterController>();

        Container
            .Bind<PlayerCharacterController>()
            .FromInstance(characterController)
            .AsSingle();
    }
}
