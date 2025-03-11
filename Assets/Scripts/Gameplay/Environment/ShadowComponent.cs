using System;
using UnityEngine;
using Materials;
using World.Data;
using Materials.Shadow;
using Zenject;


namespace World.Environment
{



    public class ShadowComponent : MonoBehaviour
    {
        private ShadowSettings _shadowSettings;

        [SerializeField] private MaterialPropContainer<ShadowMaterialProps> _materialPropContainer;
        private SpriteRenderer _newSpriteRenderer;

        [Inject]
        public void Construct(ShadowSettings settings, DayManager dayManager)
        {
            _shadowSettings = settings;
            _materialPropContainer.Properties._dayManager = dayManager;
        }

        private void Update()
        {
            _materialPropContainer?.Update(_newSpriteRenderer);
        }
        void Start()
        {
            if (!_shadowSettings._shadowsEnabled) return;

            //_materialPropContainer = new MaterialPropContainer<MatParams>();
            // Create a copy of the mesh renderer component
            if (!TryGetComponent<SpriteRenderer>(out var origSpriteRenderer))
            {
                Debug.LogError("No Sprite Renderer found on this GameObject.");
                return;
            }

            // Instantiate a new GameObject as a child and add it to the current transform
            var newGameObject = new GameObject();
            newGameObject.transform.parent = transform;
            newGameObject.transform.localScale = Vector3.one;
            newGameObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);


            _newSpriteRenderer = newGameObject.AddComponent<SpriteRenderer>();

            _newSpriteRenderer.sprite = origSpriteRenderer.sprite;
            _newSpriteRenderer.sortingOrder = origSpriteRenderer.sortingOrder;
            //newSpriteRenderer.sprite.packingMode = SpritePackingMode.Rectangle; //todo
            _newSpriteRenderer.material = _shadowSettings._shadowMaterial;
        }
    }
}
