using System;
using Dark.Utils;
using UnityEngine;
using Utilities;



[Serializable]
public struct JitterFreeProps : IMaterialProps
{
    [Range(0, 3)]
    public float _Gamma;
    [Header("Kinda useless, replace it with flash/emission color later.")]
    public Color _ColorTint;

    public bool NeedsUpdate { get; set; }

    public void UpdateAllProperties(MaterialPropertyBlock mpb)
    {
        mpb.SetFloat("_Gamma", _Gamma);
        mpb.SetColor("_Color", _ColorTint);
    }
}


public class JitterFreeComponent : MonoBehaviour
{
    public MaterialPropContainer<JitterFreeProps> _materialPropContainer;

    private SpriteRenderer _spriteRenderer;
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
