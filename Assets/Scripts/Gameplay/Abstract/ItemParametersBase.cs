
using UnityEngine;

namespace Gameplay.Components.Items
{
    public abstract class ItemParametersBase : IItemParameters
    {
        [SerializeField] private Color _tintColor = Color.white;

        public Color TintColor => _tintColor;


        public virtual void Set(IItemParameters parameters)
        {
            _tintColor = parameters.TintColor;
        }
    }
}
