using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Buttons
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonBaseUI : MonoBehaviour, IDisposable
    {
        public const float MinDelayedReactionTime = 0.01f;

        [SerializeField, Min(0)] private float _delayInSec = 0.1f;

        private Button _button;
        private Coroutine _activeRoutine = null;

        public Button TargetButton => _button;


        public abstract void Click();

        public void ClickListener()
        {
            if (_activeRoutine != null)
            {
                return;
            }

            if (_delayInSec > MinDelayedReactionTime)
            {
                _activeRoutine = StartCoroutine(ClickWithDelayRoutine(_delayInSec, Click));
            }
            else
            {
                Click();
            }
        }

        protected virtual void Awake()
        {
            _button = GetComponent<Button>();
            TargetButton.onClick.AddListener(ClickListener);
        }

        public virtual void Dispose()
        {
            TargetButton.onClick.RemoveListener(ClickListener);
        }

        protected IEnumerator ClickWithDelayRoutine(float delayInSec, Action clickAction)
        {
            yield return new WaitForSecondsRealtime(delayInSec);

            clickAction?.Invoke();

            _activeRoutine = null;
        }


        private void OnDestroy()
        {
            Dispose();
        }
    }
}
