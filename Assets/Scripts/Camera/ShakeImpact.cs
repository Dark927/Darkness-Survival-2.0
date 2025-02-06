using Settings.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Settings.CameraManagement.Shake
{
    public class ShakeImpact
    {
        #region Fields 

        private CameraController _cameraController;

        private ShakeSettings _settings;

        #endregion


        #region Properties

        public ShakeSettings Settings => _settings;

        #endregion


        #region Methods 

        #region Init

        public ShakeImpact(ShakeSettings settings)
        {
            _cameraController = ServiceLocator.Current.Get<CameraController>();
            _settings = settings;
        }

        public ShakeImpact()
        {
            _cameraController = ServiceLocator.Current.Get<CameraController>();
            _settings = ShakeSettings.Default;
        }

        #endregion

        public void Activate()
        {
            _cameraController.Shake.Invoke(Settings);
        }

        public void SetSettings(ShakeSettings settings)
        {
            _settings = settings;
        }

        #endregion
    }
}
