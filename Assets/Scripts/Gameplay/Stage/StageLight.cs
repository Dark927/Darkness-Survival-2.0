
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace World.Light
{
    [RequireComponent(typeof(Light2D))]
    public class StageLight : MonoBehaviour
    {
        #region Fields 

        private Light2D _globalLight;

        #endregion


        #region Properties

        public Light2D Light => _globalLight;

        #endregion


        #region Methods

        #region Init

        private void Awake()
        {
            _globalLight = GetComponent<Light2D>();
        }

        #endregion

        #endregion
    }
}
