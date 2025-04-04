using UnityEngine;

#nullable enable

namespace Materials.DarkEntityFX
{
    public class DarkEntityFXComponent : MonoBehaviour
    {
        [SerializeField] private MaterialPropContainer<ParametricProps> _materialPropContainer = new();


        public MaterialPropContainer<ParametricProps> MaterialPropContainer => _materialPropContainer;
        private SpriteRenderer? _spriteRenderer; //TODO: replace with requirecomponent and without errors
        private SpriteRenderer SpriteRenderer
        {
            get
            {
                if (_spriteRenderer == null)
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                return _spriteRenderer;
            }
        }

        private void OnWillRenderObject()
        {
            _materialPropContainer.Update(SpriteRenderer);
        }

        private void OnDrawGizmos()
        {
            _materialPropContainer.Update(SpriteRenderer);
        }
    }
}
