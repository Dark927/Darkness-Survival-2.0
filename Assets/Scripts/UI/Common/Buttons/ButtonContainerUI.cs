using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Buttons
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ButtonContainerUI : MonoBehaviour
    {
        #region Fields 

        private CanvasGroup _canvasGroup;

        #endregion


        #region Properties

        public CanvasGroup CanvasGroup => _canvasGroup;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        #endregion

        #endregion
    }
}
