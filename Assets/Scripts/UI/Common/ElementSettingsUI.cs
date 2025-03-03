using System.Collections;
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


        private void OnValidate()
        {
            _images = new List<Image>(GetComponentsInChildren<Image>());

            foreach (var image in _images)
            {
                image.color = _childImagesColor;
            }
        }

        #endregion

        #endregion
    }
}
