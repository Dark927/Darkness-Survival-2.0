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

        private const float MinPercentDiff = 0.001f;
        private RectTransform _rectTransform;
        private Coroutine _currentCoroutine;
        private Queue<IEnumerator> _taskQueue = new Queue<IEnumerator>();
        private UniTask _currentUpdateTask = UniTask.CompletedTask;
        private CancellationTokenSource _cts;
        private float _lastTargetPercent = 0f;

        #endregion

        public event Action<float> OnProgressPercentUpdate;

        public UniTask CurrentUpdateTask => _currentUpdateTask;
        public bool IsFull => (_rectTransform != null) && Mathf.Approximately(_rectTransform.localScale.x, 1f);
        public float CurrentPercent => _rectTransform.localScale.x * 100;
        public float LastTargetPercent => _lastTargetPercent;

        #region Methods

        #region Init

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.localScale = new Vector3(0, _rectTransform.localScale.y, 1f);
        }

        #endregion

        /// <summary>
        /// Update slider fill percent
        /// </summary>
        /// <param name="targetPercent">target percent</param>
        /// <param name="duration">lerp duration</param>
        /// <param name="cancelPreviousUpdates">if true - cancel all previous animations and play a new one, false - wait for the all animations end (queue)</param>
        /// <param name="callback">called when current animation is finished</param>
        /// <param name="forceApply">force apply percent update without considering the min percent diff</param>
        public void UpdatePercent(float targetPercent, float duration = 1f, bool cancelPreviousUpdates = true, Action callback = null, bool forceApply = false)
        {
            if (cancelPreviousUpdates)
            {
                ClearAllTasks();
            }

            if (_cts == null || _cts.IsCancellationRequested)
            {
                _cts = new CancellationTokenSource();
            }

            float startPercent = _lastTargetPercent;
            float targetPercentClamped = Mathf.Clamp01(targetPercent);

            if (!forceApply && (Mathf.Abs(targetPercentClamped - startPercent) < MinPercentDiff))
            {
                return;
            }

            _lastTargetPercent = targetPercentClamped;
            _taskQueue.Enqueue(AnimateScale(startPercent, targetPercentClamped, duration, callback));

            if (_currentUpdateTask.Status != UniTaskStatus.Pending)
            {
                _currentUpdateTask = UpdatePercentAsync(_cts.Token);
            }
        }

        private async UniTask UpdatePercentAsync(CancellationToken token = default)
        {
            if (_currentCoroutine == null)
            {
                _currentCoroutine = StartCoroutine(ProcessQueue());
            }
            else
            {
                return;
            }

            await UniTask.WaitUntil(() => _currentCoroutine == null, cancellationToken: token);

            if (token.IsCancellationRequested)
            {
                ClearAllTasks();
            }
        }

        private void ClearAllTasks()
        {
            _taskQueue.Clear();

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;

            if (_currentCoroutine != null)
            {
                StopCoroutine(ProcessQueue());
                _currentCoroutine = null;
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

        private IEnumerator AnimateScale(float startPercent, float targetPercent, float duration, Action callback = null)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                float newScaleX = Mathf.Lerp(startPercent, targetPercent, elapsedTime / duration);
                _rectTransform.localScale = new Vector3(newScaleX, _rectTransform.localScale.y, _rectTransform.localScale.z);

                OnProgressPercentUpdate?.Invoke(CurrentPercent);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _rectTransform.localScale = new Vector3(targetPercent, _rectTransform.localScale.y, _rectTransform.localScale.z);
            callback?.Invoke();
        }

        private void OnDestroy()
        {
            ClearAllTasks();
        }

        #endregion
    }
}
