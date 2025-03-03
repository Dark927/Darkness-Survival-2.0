using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public class SliderUI : MonoBehaviour
    {
        #region Fields 

        private RectTransform _rectTransform;
        private Coroutine _currentCoroutine;
        private Queue<IEnumerator> _taskQueue = new Queue<IEnumerator>();
        private UniTask _currentUpdateTask = UniTask.CompletedTask;

        #endregion

        public UniTask CurrentUpdateTask => _currentUpdateTask;



        #region Methods

        #region Init

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        #endregion

        public void UpdatePercent(float percent, float duration = 1f, bool cancelPreviousUpdates = false, Action callback = null)
        {
            if (cancelPreviousUpdates)
            {
                ClearAllTasks();
            }

            _taskQueue.Enqueue(AnimateScale(percent, duration, callback));

            if (_currentUpdateTask.Status != UniTaskStatus.Pending)
            {
                _currentUpdateTask = UpdatePercentAsync();
            }
        }

        private async UniTask UpdatePercentAsync(CancellationToken token = default) ////////////////// token
        {
            if (_currentCoroutine == null)
            {
                _currentCoroutine = StartCoroutine(ProcessQueue());
            }
            else
            {
                return;
            }

            await UniTask.WaitUntil(() => _currentCoroutine == null);
        }

        private void ClearAllTasks()
        {
            _taskQueue.Clear();

            if (_currentCoroutine != null)
            {
                StopCoroutine(ProcessQueue());
            }
        }

        private IEnumerator ProcessQueue()
        {
            while (_taskQueue.Count > 0)
            {
                yield return StartCoroutine(_taskQueue.Dequeue());
            }

            _currentCoroutine = null;
        }

        private IEnumerator AnimateScale(float targetPercent, float duration, Action callback = null)
        {
            float startPercent = _rectTransform.localScale.x;
            float targetX = Mathf.Clamp01(targetPercent);
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float newScaleX = Mathf.Lerp(startPercent, targetX, elapsedTime / duration);
                _rectTransform.localScale = new Vector3(newScaleX, _rectTransform.localScale.y, _rectTransform.localScale.z);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _rectTransform.localScale = new Vector3(targetX, _rectTransform.localScale.y, _rectTransform.localScale.z);
            callback?.Invoke();
        }

        #endregion
    }
}
