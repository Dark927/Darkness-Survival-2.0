using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBaseUI : MonoBehaviour, IDisposable
    {
        private Button _button;

        public Button TargetButton => _button;

        public abstract void ClickListener();

        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            TargetButton.onClick.AddListener(ClickListener);
        }

        public void Dispose()
        {
            TargetButton.onClick.RemoveListener(ClickListener);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
