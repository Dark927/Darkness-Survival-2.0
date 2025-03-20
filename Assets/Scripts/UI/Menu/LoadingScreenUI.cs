using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public sealed class LoadingScreenUI : MonoBehaviour
    {
        #region Fields 

        [SerializeField] private float _deactivationDelay = 3f;
        [SerializeField] private float _maxProgressUpdateDuration = 2f;
        private SliderUI _progressSlider;
        private TMP_Text _progressText;

        #endregion


        #region Properties

        public float DeactivationDelay => _deactivationDelay;
        public bool IsFullProgress => (_progressSlider != null) && _progressSlider.IsFull;

        #endregion

        #region Methods

        #region Init

        private void Awake()
        {
            _progressSlider = GetComponentInChildren<SliderUI>();
            _progressText = GetComponentInChildren<TMP_Text>();
            _progressSlider.OnProgressPercentUpdate += UpdateProgressText;
        }

        #endregion

        /// <summary>
        /// set the loading progress to the loading screen indicators
        /// </summary>
        /// <param name="progressPercent">target progress percent (0-1)</param>
        /// <param name="forceApply">apply the progress without any restrictions (like the ignoring the same values). Note : it will create the same update animations, etc.</param>
        public void SetLoadingProgress(float progressPercent, bool forceApply = false)
        {
            if (_progressSlider == null)
            {
                return;
            }

            float progressDifference = Mathf.Abs(_progressSlider.LastTargetPercent - progressPercent);
            float updateDuration = _maxProgressUpdateDuration * progressDifference;
            _progressSlider.UpdatePercent(progressPercent, updateDuration, false, forceApply: forceApply);
        }

        public void SetFullProgress()
        {
            SetLoadingProgress(1f, true);
        }

        public async UniTask<bool> Deactivate()
        {
            await UniTask.WaitForSeconds(_deactivationDelay);

            return true;
        }

        private IEnumerator DeactivationRoutine()
        {
            yield return new WaitForSeconds(_deactivationDelay);
        }

        private void UpdateProgressText(float progressPercent)
        {
            _progressText.text = $"loading - {progressPercent:00}";
        }

        private void OnDestroy()
        {
            _progressSlider.OnProgressPercentUpdate -= UpdateProgressText;
        }

        #endregion
    }
}
