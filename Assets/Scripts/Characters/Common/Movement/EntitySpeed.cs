using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Common.Movement
{
    public class EntitySpeed
    {
        #region Fields 

        public static readonly EntitySpeed DefaultStatic = new EntitySpeed()
        {
            _settings = SpeedSettings.Zero
        };

        private Vector2 _velocity;
        private Vector2 _direction;
        private SpeedSettings _settings;

        public event EventHandler<SpeedChangedArgs> OnActualSpeedChanged;
        public event EventHandler<Vector2> OnVelocityUpdate;

        #endregion


        #region Properties

        public SpeedSettings Settings => _settings;

        public float ActualSpeed { get => Velocity.magnitude; }
        public Vector2 Velocity
        {
            get => _velocity;
            private set => _velocity = value;
        }

        public Vector2 Direction => _direction;

        #endregion


        #region Methods 

        public void Stop()
        {
            _settings.CurrentSpeedMultiplier = 0;
            UpdateVelocity(_direction);
        }

        public void ClearDirection()
        {
            _direction = Vector2.zero;
        }

        public void TryUpdateVelocity(Vector2 direction)
        {
            _direction = direction;
            UpdateVelocity(_direction);
        }

        public void UpdateVelocity(Vector2 direction)
        {
            Vector2 calculatedVelocity = direction * _settings.CurrentSpeedMultiplier;
            bool speedChanged = !Mathf.Approximately(calculatedVelocity.sqrMagnitude, Velocity.sqrMagnitude);

            Velocity = calculatedVelocity;
            OnVelocityUpdate?.Invoke(this, Velocity);

            if (speedChanged)
            {
                OnActualSpeedChanged?.Invoke(this, new SpeedChangedArgs(ActualSpeed, _settings.MaxSpeedMultiplier));
            }
        }

        public void SetSettings(SpeedSettings settings)
        {
            _settings.CurrentSpeedMultiplier = settings.CurrentSpeedMultiplier;
            _settings.MaxSpeedMultiplier = settings.MaxSpeedMultiplier;
        }

        public void SetMaxSpeedMultiplier()
        {
            _settings.CurrentSpeedMultiplier = _settings.MaxSpeedMultiplier;
        }

        public async UniTask UpdateSpeedMultiplierLinear(float targetSpeedMultiplier, int timeInMs, CancellationToken token)
        {
            float startSpeedMultiplier = _settings.CurrentSpeedMultiplier;
            float endSpeedMultiplier = targetSpeedMultiplier;
            float elapsedTime = 0f;

            while (elapsedTime < timeInMs)
            {
                if (token.IsCancellationRequested)
                {
                    _settings.CurrentSpeedMultiplier = endSpeedMultiplier;
                    return;
                }

                _settings.CurrentSpeedMultiplier = Mathf.Lerp(startSpeedMultiplier, endSpeedMultiplier, elapsedTime / timeInMs);
                elapsedTime += Time.deltaTime * 1000f;
                await UniTask.Yield();
            }

            _settings.CurrentSpeedMultiplier = endSpeedMultiplier;
        }

        #endregion

    }
}
