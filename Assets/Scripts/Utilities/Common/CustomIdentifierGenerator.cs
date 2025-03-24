
using CommunityToolkit.HighPerformance;
using UnityEngine.AddressableAssets;

namespace Utilities.Math
{
    public static class CustomIdentifierGenerator
    {
        public static int GenerateID(string name, AssetReference assetRef)
        {
            return $"{name}{assetRef.RuntimeKey}".GetDjb2HashCode();
        }
    }
}
