using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Settings
{
    public class DataLoader
    {
        public async UniTask<T> Load<T>(AssetReference reference)
        {
            var handle = reference.LoadAssetAsync<T>();
            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                return handle.Result;
            }

            throw new Exception($"Failed to load asset of type {typeof(T)} from reference: {reference}");
        }
    }
}
