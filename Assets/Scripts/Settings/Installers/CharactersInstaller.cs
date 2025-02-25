using Characters.Player;
using Characters.Player.Data;
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
        AsyncOperationHandle<GameObject> handle = AddressableAssetsLoader.Instance.TryLoadAssetAsync<GameObject>(_characterAsset);
        handle.WaitForCompletion();

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