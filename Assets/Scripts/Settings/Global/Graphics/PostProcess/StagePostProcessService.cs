
using System;
using UnityEngine.Rendering;
using Zenject;

namespace Settings.Global
{
    public class StagePostProcessService : IService, IDisposable
    {
        #region Fields 

        private Volume _volume;
        private GrayscaleEffect _grayscaleEffect;
        private StagePostProcessData _data;

        #endregion


        #region Properties

        public GrayscaleEffect Grayscale => _grayscaleEffect;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public StagePostProcessService(Volume volume, StagePostProcessData data)
        {
            _volume = volume;
            _data = data;
            _grayscaleEffect = new GrayscaleEffect(volume, _data.Grayscale, _data.GrayscaleTransDurationInSec);
        }

        public void Dispose()
        {
            _grayscaleEffect?.Dispose();
        }

        #endregion

        #endregion
    }
}
