 

using UnityEngine;
using Utilities.Attributes;

namespace Settings.Global
{
    [CreateAssetMenu(fileName = "NewStagePostProcessData", menuName = "Game/Settings/Post Process/Stage Post Process Data")]
    public class StagePostProcessData : DescriptionBaseData
    {
        [CustomHeader("Grayscale", 2, 0)]
        [SerializeField] private float _grayscaleTransDurationInSec = 1.0f;
        [SerializeField] private GrayscaleSettings _grayscale;

        public GrayscaleSettings Grayscale => _grayscale;
        public float GrayscaleTransDurationInSec => _grayscaleTransDurationInSec;
    }
}
