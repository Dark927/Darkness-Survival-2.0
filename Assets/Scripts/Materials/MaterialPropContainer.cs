using System;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable enable

namespace Materials
{
    [Serializable]
    public class MaterialPropContainer<T> where T : IMaterialProps, new() //TODO: add SO + struct 2 fields
    {
        public MaterialPropertyBlock? _mpb;

        [SerializeField] public ScriptableMaterialPropsBase? _constMaterialProps;

        [SerializeField] private T _properties;

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
            _properties = new() { NeedsUpdate = true };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateGlobals(MaterialPropertyBlock mpb)
        {
            //set time ig
        }


        public void Update(Renderer renderer)
        {
            var updateOverride = false;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                updateOverride = true;
            }
#endif
            if (_properties == null) return;

            if (_properties.NeedsUpdate || updateOverride)
            {
                _mpb ??= new MaterialPropertyBlock();
                renderer.GetPropertyBlock(_mpb);

                _constMaterialProps?.UpdateAllProperties(_mpb);
                _properties.UpdateAllProperties(_mpb);

                renderer.SetPropertyBlock(_mpb);
                _properties.NeedsUpdate = false;
            }
        }
    }
}
