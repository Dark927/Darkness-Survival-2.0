using System;
using System.Collections.Generic;
using System.Linq;
using Dark.Utils;
using Settings.Global;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using World.Components;
using World.Data;
using World.Environment;
using Utilities;
using Zenject;

#nullable enable

namespace Dark.Environment
{
    public class DayManager : MonoBehaviour
    {
        #region Fields 
        [SerializeField] private Light2D _globalLight;
        [SerializeField] private Light2D _playerLight;
        [SerializeField] private List<DayStateData> _dayList;
        [Header("Beginning DayStateData")]
        [SerializeField] private DayStateData _previousData;
        #endregion

        #region events
        public event EventHandler<DayChangedEventArgs>? ThresholdReached;
        #endregion

        private DayStateData _targetDayStateData;
        public float InGameTime => _inGameTime;
        private float _transitionState = 0;

        [Header("Beginning DayStateData")]

        [Header("In Game time. 0,1 - day, 0.5 - night")]
        [SerializeField]
        [Range(0, 1)]
        private float _inGameTime;


        [Inject]
        public void Construct(GameTimer gameTimer)
        {
            //TODO:
            //_gameTimer = gameTimer;
        }

        private void Start()
        {
            _targetDayStateData = GetNewDayStateData();

        }

        private void Destroy()
        {
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
            _globalLight.color = color;
            _globalLight.intensity = GlobalLightFx(DarkMath.Frac(_inGameTime)); //intensity curve

            if (_playerLight == null) ///TODO: 
            {
                var player = ServiceLocator.Current.Get<PlayerManager>();
                _playerLight = player.GetCharacterTransform().parent.GetComponentInChildren<Light2D>();
            }
            else
                _playerLight.intensity = 1 - _globalLight.intensity;


            //animation integration += dx (duration to [0,1] range)
            _transitionState += Time.deltaTime / _targetDayStateData.RealTimeDuration;
            Telemetry.Log(0, _transitionState, DarkMath.Frac(_inGameTime), _globalLight.intensity);
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
