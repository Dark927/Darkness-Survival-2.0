

using UnityEngine;

namespace Gameplay.Components.Items
{
    public interface IItemParameters
    {
        public Color TintColor { get; }
        public void Set(IItemParameters parameters);
    }
}
