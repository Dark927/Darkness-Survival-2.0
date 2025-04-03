using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Buttons
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ButtonContainerUI : MonoBehaviour
    {
        #region Fields 

        private CanvasGroup _canvasGroup;
        private EventTrigger _eventTrigger;

        #endregion


        #region Properties

        public float Alpha
        {
            get => _canvasGroup.alpha;

            set
            {
                if (0f <= value && value <= 1f)
                {
                    _canvasGroup.alpha = value;
                }
            }
        }


        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _eventTrigger = GetComponentInChildren<EventTrigger>();
        }

        #endregion

        public void DisableInteraction()
        {
            _canvasGroup.interactable = false;
            _eventTrigger.enabled = false;
        }

        public void EnableInteraction()
        {
            _canvasGroup.interactable = true;
            _eventTrigger.enabled = true;
        }

        #endregion
    }
}
