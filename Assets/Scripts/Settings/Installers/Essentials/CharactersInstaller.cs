using Characters.Player;
using Characters.Player.Upgrades;
using Settings.AssetsManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities.Attributes;
using Zenject;


public sealed class CharactersInstaller : MonoInstaller
{
    [CustomHeader("Main Character Settings", count = 2, depth = 0)]
    [SerializeField] private AssetReference _characterAsset;
    [SerializeField] private Transform _parentContainer;


    [CustomHeader("Character Upgrades", count = 3, depth = 0)]
    [SerializeField] private UpgradesSetData _upgradesSetData;

    public override void InstallBindings()
    {
        BindPlayerCharacter();
        BindCharacterUpgrades();
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

    private void BindCharacterUpgrades()
    {
        Container
            .Bind<UpgradesSetData>()
            .FromInstance(_upgradesSetData)
            .AsSingle();
    }
}
