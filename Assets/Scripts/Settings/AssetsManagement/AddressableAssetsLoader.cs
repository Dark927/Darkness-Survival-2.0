﻿using System;
using Settings.Abstract;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Settings.AssetsManagement
{
    public class AddressableAssetsLoader : LazySingletonMono<AddressableAssetsLoader>
    {
        #region Methods

        #region Static

        public static bool AreAssetRefsEqual(AssetReference first, AssetReference second)
        {
            return first.RuntimeKey.ToString() == second.RuntimeKey.ToString();
        }

        public static bool IsAssetRefValid(AssetReference assetRef)
        {
            bool result = !string.IsNullOrEmpty(assetRef.AssetGUID);

#if UNITY_EDITOR
            result = result && (assetRef.editorAsset != null);
#endif

            return result;
        }

        #endregion


        /// <summary>
        /// try load an asset from the asset reference, throw exception if an asset is not valid.
        /// </summary>
        /// <typeparam name="TResult">the asset type to load</typeparam>
        /// <param name="assetRef">asset reference to load</param>
        /// <returns>loaded asset</returns>
        /// <exception cref="ArgumentException">an argument exception if the asset is not valid</exception>
        public AsyncOperationHandle<TResult> TryLoadAssetAsync<TResult>(AssetReference assetRef)
        {
            if (IsAssetRefValid(assetRef))
            {
                return LoadAssetAsync<TResult>(assetRef);
            }
            throw new ArgumentException($"Bad Asset Reference: {assetRef}");
        }

        /// <summary>
        /// load an asset from the asset reference, do not throw an exception if an asset is not valid.
        /// </summary>
        /// <typeparam name="TResult">the asset type to load</typeparam>
        /// <param name="assetRef">asset reference to load</param>
        /// <returns>loaded asset</returns>
        public AsyncOperationHandle<TResult> LoadAssetAsync<TResult>(AssetReference assetRef)
        {
            return Addressables.LoadAssetAsync<TResult>(assetRef);
        }

        // - Unload 

        public void TryUnloadAsset(AsyncOperationHandle handle)
        {
            if (handle.IsValid())
            {
                UnloadAsset(handle);
            }
        }

        public void UnloadAsset(AsyncOperationHandle handle)
        {
            Addressables.Release(handle);
        }

        public void UnloadAssetInstance(GameObject instance)
        {
            Addressables.ReleaseInstance(instance);
        }

        public AsyncOperationHandle<GameObject> InstantiateAsync(AssetReference assetRef)
        {
            return Addressables.InstantiateAsync(assetRef);
        }

        #endregion
    }
}
