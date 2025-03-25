using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Dark.Utils;
using Gameplay.Components;
using Settings.Global;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utilities;
using Utilities.Math;
using World.Data;
using World.Light;
using Zenject;

#nullable enable

namespace World.Environment
{
    public class DayManager : MonoBehaviour
    {
        #region Fields 

        private StageLight _stageLight = default!;
        private Light2D _playerLight = default!;
        [SerializeField] private List<DayStateData> _dayList = default!;

        [Header("Beginning DayStateData")]
        [SerializeField] private DayStateData _previousData = default!;

        #endregion

        #region events

        public event EventHandler<DayChangedEventArgs>? ThresholdReached;

        #endregion

        private DayStateData _targetDayStateData = default!;
        public float InGameTime => _inGameTime;
        private float _transitionState = 0;

        [Header("Beginning DayStateData")]

        [Header("In Game time. 0,1 - day, 0.5 - night")]
        [SerializeField]
        [Range(0, 1)]
        private float _inGameTime;


        [Inject]
        public void Construct(StageLight stageLight)
        {
            _stageLight = stageLight;
        }

        private void Start()
        {
            _targetDayStateData = GetNewDayStateData();

            var playerService = ServiceLocator.Current.Get<PlayerService>();

            if (playerService.TryGetPlayer(out PlayerCharacterController player))
            {
                _playerLight = player.GetComponentInChildren<Light2D>();
            }
            else
            {
                Debug.LogWarning($"# Player is null! - {gameObject.name}");
            }
        }

        private void Update()
        {
            if (_transitionState >= 1)
            {
                //reset animation to beginning
                _transitionState = 0;

                //send event end, pass finished state
                ThresholdReached?.Invoke(this, new DayChangedEventArgs(_targetDayStateData));

                //swap scriptable objects
                _previousData = _targetDayStateData;
                if (_dayList.Count > 1)
                    _dayList.Remove(_targetDayStateData);

                _targetDayStateData = GetNewDayStateData();
            }

            //loop the day cycle, extrapolate step:  0.7 -> 0.3  =>  0.7 -> 1.3
            var targetGameTime = _targetDayStateData.TargetGameTime;
            if (_previousData.TargetGameTime >= targetGameTime)
            {
                targetGameTime++;
            }


            if (_targetDayStateData.RealTimeDuration < 0.01f)
            {
                throw new DivideByZeroException();
            }

            // V = S / t
            var rate = (targetGameTime - _previousData.TargetGameTime) / _targetDayStateData.RealTimeDuration;
            _inGameTime += rate * Time.deltaTime;

            // Color interpolation
            var color = Color.Lerp(_previousData.TargetColor, _targetDayStateData.TargetColor, _transitionState);
            _stageLight.Light.color = color;
            _stageLight.Light.intensity = GlobalLightFx(CustomMath.Frac(_inGameTime)); //intensity curve

            TryUpdatePlayerLight();

            //animation integration += dx (duration to [0,1] range)
            _transitionState += Time.deltaTime / _targetDayStateData.RealTimeDuration;
            //Telemetry.Log(0, _transitionState, DarkMath.Frac(_inGameTime), _stageLight.Light.intensity);
        }

        private void TryUpdatePlayerLight()
        {
            if (_playerLight == null)
            {
                return;
            }

            _playerLight.intensity = 1 - _stageLight.Light.intensity;
        }

        private DayStateData GetNewDayStateData()
        {
            return _dayList.FirstOrDefault();
        }
        public static float GlobalLightFx(float x, float minAmount = 0.3f, float maxAmount = 1)
        {
            return 4 * (maxAmount - minAmount) * Mathf.Pow(x - 0.5f, 2) + minAmount; //quadratic function with extremum in (0.5, minAmount)
        }
    }
}
