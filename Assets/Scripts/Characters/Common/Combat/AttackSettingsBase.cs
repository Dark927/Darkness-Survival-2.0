using UnityEngine;

namespace Characters.Common.Combat
{
    [System.Serializable]
    public class AttackSettingsBase
    {
        [Space, Header("Main Settings")]
        [SerializeField] private float _triggerActivityTimeSec;
        [SerializeField] private float _fullDurationTimeSec;
        [SerializeField] private float _reloadTimeSec;

        public float TriggerActivityTimeSec => _triggerActivityTimeSec;
        public float FullDurationTimeSec => _fullDurationTimeSec;
        public float ReloadTimeSec => _reloadTimeSec;  

    }
}
