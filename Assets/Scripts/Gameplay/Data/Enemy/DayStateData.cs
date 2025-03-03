using UnityEngine;

namespace World.Data
{
    public enum DayTimeType
    {
        Morning,
        Day,
        Evening,
        Night
    }

    [CreateAssetMenu(fileName = "NewDayStateData", menuName = "Game/World/Data/Day State Data")]
    public class DayStateData : ScriptableObject
    {
        [SerializeField] private float _realTimeDuration;
        [Range(0, 1)]
        [SerializeField] private float _targetGameTime;
        [SerializeField] private Color _targetColor = Color.white;

        [Header("Tags")]
        [SerializeField] private DayTimeType _dayTimeType;
        [SerializeField] private string _tag;


        public float RealTimeDuration => _realTimeDuration;
        public float TargetGameTime => _targetGameTime;
        public Color TargetColor => _targetColor;

        public DayTimeType DayTimeType => _dayTimeType;
        public string Tag => _tag;
    }
}
