using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LoadingScreenUI : FadeableColorUI<Image>
    {
        #region Fields 

        [SerializeField] private float _deactivationDelay = 3f;
        [SerializeField] private float _progressUpdateDuration = 2f;
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

        public void SetLoadingProgress(float progressPercent)
        {
            if (_progressSlider != null)
            {
                _progressSlider.UpdatePercent(progressPercent, _progressUpdateDuration);
            }
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

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _progressSlider.OnProgressPercentUpdate -= UpdateProgressText;
        }

        #endregion
    }
}
