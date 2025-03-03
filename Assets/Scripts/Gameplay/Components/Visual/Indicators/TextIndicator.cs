
using System;
using TMPro;
using UnityEngine;

namespace Gameplay.Visual
{
    public class TextIndicator : MonoBehaviour, ITextIndicator
    {
        #region Fields 

        private TMP_Text _indicatorText;

        #endregion


        #region Properties

        public TMP_Text TMPText => _indicatorText;

        #endregion


        #region Events

        public event Action<ITextIndicator> OnLifeTimeEnd;

        #endregion


        #region Methods

        #region Init

        public void Initialize()
        {
            _indicatorText = GetComponent<TMP_Text>();
            _indicatorText.color = Color.white;
        }

        #endregion

        private void NotifyLifeTimeEnd()
        {
            OnLifeTimeEnd?.Invoke(this);
        }


        #endregion
    }
}
