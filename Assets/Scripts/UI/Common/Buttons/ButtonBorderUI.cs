using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    [RequireComponent(typeof(Image))]
    public class ButtonBorderUI : MonoBehaviour
    {
        #region Fields 

        private Image _borderImage;

        #endregion


        #region Properties

        public Image Img => _borderImage;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _borderImage = GetComponent<Image>();
        }

        #endregion


        #endregion
    }
}
