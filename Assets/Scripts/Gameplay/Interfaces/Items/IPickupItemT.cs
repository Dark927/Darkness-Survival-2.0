

using Characters.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gameplay.Components.Items
{
    public interface IPickupItemT<TItemParameters> : IPickupItem where TItemParameters : IItemParameters
    {
        public void SetXpParameters(TItemParameters parameters);
    }
}
