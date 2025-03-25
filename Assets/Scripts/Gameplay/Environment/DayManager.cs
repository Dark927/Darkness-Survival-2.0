using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Player;
using Gameplay.Components;
using Gameplay.Stage;
using Settings.Global;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utilities;
using Utilities.Math;
using Utilities.ErrorHandling;
using World.Data;
using World.Light;

#nullable enable

namespace World.Environment
{
    public class DayManager : MonoBehaviour, Settings.Global.IInitializable
    {
        #region events

        public event EventHandler<DayChangedEventArgs>? ThresholdReached;

        #endregion

        #region Fields 

        private DayStateData _previousData = default!;
        private List<DayStateData> _dayList = default!;

        private StageLight _stageLight = default!;
        private Light2D _playerLight = default!;

        private DayStateData _targetDayStateData = default!;
        private float _transitionState = 0;


        #region Debug
        [Header("DEBUG : In Game time. 0,1 - day, 0.5 - night")]
        [SerializeField]
        [Range(0, 1)]
        private float _inGameTime = 0;
        private float _previousTime = 0f;

        #endregion

        #endregion

        #region Properties 

        public float InGameTime => _inGameTime;

        #endregion


        public void Initialize()
        {
            if (_dayList == null)
            {
                ErrorLogger.LogWarning($"{gameObject.name} | {nameof(_dayList)} is null | Deactivating..");
                gameObject.SetActive(false);
                return;
            }

            _targetDayStateData = GetNewDayStateData();

            var playerService = ServiceLocator.Current.Get<PlayerService>();

            if (playerService.TryGetPlayer(out PlayerCharacterController player))
            {
                _playerLight = player.GetComponentInChildren<Light2D>();
            }
            else
            {
                ErrorLogger.LogWarning($"# Player is null! - {gameObject.name}");
            }
        }

        public void SetDayManagerSettings(StageLight targetLight, DayStatesSetData dayStatesSetData)
        {
            _previousData = dayStatesSetData.StartDayState;
            _dayList = new List<DayStateData>(dayStatesSetData.DayList);
            _stageLight = targetLight;
        }


        public void UpdateDayState(float currentTime)
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
            _inGameTime += (currentTime - _previousTime) * rate;

            // Color interpolation
            var color = Color.Lerp(_previousData.TargetColor, _targetDayStateData.TargetColor, _transitionState);
            _stageLight.Light.color = color;
            _stageLight.Light.intensity = GlobalLightFx(CustomMath.Frac(_inGameTime)); //intensity curve

            TryUpdatePlayerLight();

            //animation integration += dx (duration to [0,1] range)

            _transitionState += (currentTime - _previousTime) / _targetDayStateData.RealTimeDuration;

            _previousTime = currentTime;
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
