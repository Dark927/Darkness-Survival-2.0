using System;
using TMPro;
using UnityEngine;

namespace Materials.DarkMeshPro
{
    [ExecuteAlways]
    public class DarkMeshProComponent : MonoBehaviour
    {

        private TextMeshProUGUI _textMeshPro;

        private void Awake()
        {
            _textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void OnRenderObject()
        {
#if UNITY_EDITOR
            if (_textMeshPro == null)
            {
                _textMeshPro = GetComponent<TextMeshProUGUI>();
            }
#endif
            _textMeshPro.materialForRendering.SetFloat("_UnscaledTime", Time.unscaledTime);
        }
    }
}
