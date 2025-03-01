
using UnityEngine;

namespace Gameplay.Visual
{
    [CreateAssetMenu(fileName = "NewIndicatorServiceData", menuName = "Game/World/Data/Indicator Service Data")]
    public class IndicatorServiceData : ScriptableObject
    {
        #region Fields

        [Header("Main Settings")]
        [SerializeField] private TextIndicator _textIndicator;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private Vector2 _randomOffset;

        #endregion


        #region Properties
        public TextIndicator TextIndicator => _textIndicator;
        public Vector2 Offset => _offset;
        public Vector2 RandomOffset => _randomOffset;

        #endregion
    }
}
