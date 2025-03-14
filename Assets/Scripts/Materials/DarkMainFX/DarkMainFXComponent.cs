using System;
using UnityEngine;

#nullable enable

namespace Materials.DarkMainFX
{
    public class DarkMainFXComponent : MonoBehaviour
    {
        public MaterialPropContainer<ParametricProps> _materialPropContainer = new();

        private SpriteRenderer? _spriteRenderer;
        private SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        private void OnRenderObject()
        {
            _materialPropContainer.Update(SpriteRenderer);
        }

        private void OnDrawGizmos()
        {
            _materialPropContainer.Update(SpriteRenderer);
        }
    }
}
