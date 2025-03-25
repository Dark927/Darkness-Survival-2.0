using System.Collections.Generic;
using UnityEngine;
using World.Data;

namespace Gameplay.Stage
{
    [CreateAssetMenu(fileName = "NewDayStatesSetData", menuName = "Game/World/Data/Day States Set Data")]
    public class DayStatesSetData : ScriptableObject
    {
        [SerializeField] private List<DayStateData> _dayList = default;
        [Header("Beginning DayStateData")]
        [SerializeField] private DayStateData _startDayState = default;

        public List<DayStateData> DayList => _dayList;
        public DayStateData StartDayState => _startDayState;
    }
}
