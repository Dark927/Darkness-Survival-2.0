using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class InteractiveElementUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerClickHandler,
        IPointerExitHandler
    {
        public event Action OnPointerEnter;
        public event Action OnPointerClick;
        public event Action OnPointerExit;


        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnPointerClick?.Invoke();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnter?.Invoke();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            OnPointerExit?.Invoke();
        }
    }
}
