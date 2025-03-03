
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewIndicatorPoolSettings", menuName = "Game/Settings/Pool/Indicator Pool Data")]
    public class IndicatorPoolData : ObjectPoolData
    {
        [SerializeField] private bool _forceReuseEnabled;

        public bool ForceReuseEnabled => _forceReuseEnabled;
    }
}
