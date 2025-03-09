using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    [ExecuteAlways]
    public class ElementSettingsUI : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private Color _childImagesColor = Color.white;
        private List<Image> _images;

        #endregion

        #region Methods

        #region Init

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }

            _images = new List<Image>(GetComponentsInChildren<Image>());

            foreach (var image in _images)
            {
                image.color = _childImagesColor;
            }
        }

#endif

        #endregion

        #endregion
    }
}
