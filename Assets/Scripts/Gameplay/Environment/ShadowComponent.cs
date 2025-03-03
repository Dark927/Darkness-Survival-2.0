using System;
using UnityEngine;
using Utilities;
using World.Data;
using Zenject;


namespace Dark.Environment
{
    [Serializable]
    public class MatParams : IMaterialProps
    {
        //[HideInInspector]
        public float TimeMultiplier;
        public Color ShadowColor = new(0, 0, 0, 0.2f);
        public float SunDirection;
        public float PerspectiveStrength = 0.1f;
        public Vector3 ShadowScale = new(0, 0.1f, 0);
        public Vector3 RootPos;

        public DayManager _dayManager;

        public bool NeedsUpdate { get => true; set { } }

        public void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            if (_dayManager == null)
            {
                Debug.LogError("Timer is null!");
                return;
            }

            /// 0 - 12pm day
            /// 0.5 - 12am night
            /// 1 - 12pm day again
            mpb.SetFloat("_TimeFactor", _dayManager.InGameTime);

            mpb.SetColor("_ShadowColorDay", ShadowColor);
            mpb.SetColor("_ShadowColorSunset", ShadowColor);

            mpb.SetFloat("_SunDirection", SunDirection);
            mpb.SetFloat("_PerspectiveStrength", PerspectiveStrength);
            mpb.SetVector("_ShadowScale", ShadowScale);

            mpb.SetVector("_RootPos", RootPos);
        }
    }


    public class ShadowComponent : MonoBehaviour
    {
        private ShadowSettings _shadowSettings;

        [SerializeField] private MaterialPropContainer<MatParams> _materialPropContainer;
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
