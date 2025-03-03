using System;
using UnityEngine;

namespace Utilities
{
    [Serializable]
    public class MaterialPropContainer<T> where T : IMaterialProps, new()
    {
        public MaterialPropertyBlock _mpb;
        [SerializeField]
        private T _properties;

        public T Properties
        {
            get => _properties;
            set
            {
                _properties = value;
                if (_properties != null)
                    _properties.NeedsUpdate = true;
            }
        }
        public MaterialPropContainer()
        {
            //_mpb = null;//materialPropertyBlock;
            _properties = new()
            {
                NeedsUpdate = true
            };
        }

        public void Update(Renderer renderer)
        {
            if (_properties.NeedsUpdate)
            {
                _mpb ??= new MaterialPropertyBlock();
                renderer.GetPropertyBlock(_mpb);
                _properties.UpdateAllProperties(_mpb);
                renderer.SetPropertyBlock(_mpb);
                _properties.NeedsUpdate = false;
            }
        }
    }
}
