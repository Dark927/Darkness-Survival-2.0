using UnityEngine;

namespace Gameplay.Stage.Design
{
    public class GrassVelocityController : MonoBehaviour
    {
        #region Fields 
        public static readonly int ExternalInfluencePropID = Shader.PropertyToID("_ExternalInfluence");

        [Tooltip("Use this parameter to configure how strong must look the external influence.")]
        [SerializeField, Range(0f, 1f)] private float _externalInfluenceMult = 0.25f;

        [SerializeField] private float _easeInTimeSec = 0.15f;
        [SerializeField] private float _easeOutTimeSec = 0.15f;

        [Tooltip("Determines how strong the velocity must be to affect the object.")]
        [SerializeField] private float _velocityThreshold = 5f;

        #endregion


        #region Properties

        public float EaseInTimeSec => _easeInTimeSec;
        public float EaseOutTimeSec => _easeOutTimeSec;
        public float ExternalInfluenceMult => _externalInfluenceMult;
        public float VelocityThreshold => _velocityThreshold;

        #endregion


        #region Methods

        #region Init

        #endregion

        public void InfluenceGrass(Material targetMaterial, float velocityX)
        {
            targetMaterial.SetFloat(ExternalInfluencePropID, velocityX);
        }


        #endregion
    }
}
