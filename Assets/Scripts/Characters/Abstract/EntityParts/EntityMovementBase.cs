using System;
using Cysharp.Threading.Tasks;
using Settings.Global;
using UnityEngine;

namespace Characters.Common.Movement
{
    public abstract class EntityMovementBase : IEntityMovement, IResetable
    {
        protected float _cachedOriginalSpeedMultiplier;

        public virtual bool IsMoving { get; }
        public virtual Vector2 Direction { get; }
        public virtual EntitySpeed Speed { get; }
        public virtual bool IsBlocked { get; }

        public event Action<Vector2> OnMovementPerformed;
        public event EventHandler<bool> OnMovementStateChanged;


        public virtual void SetTemporarySpeedMultiplier(float multiplier)
        {
            _cachedOriginalSpeedMultiplier = Speed.Settings.CurrentSpeedMultiplier;

            SpeedSettings modifiedSettings = Speed.Settings;
            modifiedSettings.CurrentSpeedMultiplier = _cachedOriginalSpeedMultiplier * multiplier;

            Speed.SetSettings(modifiedSettings);

            if (!IsBlocked)
            {
                Speed.UpdateVelocity(Speed.Direction);
            }
        }

        public virtual void RestoreSpeedMultiplier()
        {
            SpeedSettings restoredSettings = Speed.Settings;
            restoredSettings.CurrentSpeedMultiplier = _cachedOriginalSpeedMultiplier;

            Speed.SetSettings(restoredSettings);

            if (!IsBlocked)
            {
                Speed.UpdateVelocity(Speed.Direction);
            }
        }


        // --- External Physics Interactions ---
        public virtual void ApplyExternalPush(Vector2 direction, float force)
        {

        }


        public virtual void Move(Vector2 direction)
        {

        }

        public virtual void UpdateSpeedSettings(SpeedSettings settings, bool moveWithMaxSpeed = false)
        {
            Speed.SetSettings(settings);

            if (moveWithMaxSpeed)
            {
                Unblock();
                Speed.SetMaxSpeedMultiplier();
            }
        }

        /// <summary>
        /// Stop the character until the next movement
        /// </summary>
        public virtual void Stop()
        {
        }

        public virtual void Block(int timeInMs)
        {

        }

        public virtual void Block()
        {

        }

        public virtual void Unblock()
        {

        }

        public virtual void DropSpeed()
        {
            Speed.Stop();
            OnMovementStateChanged?.Invoke(this, false);
        }

        public virtual void ConfigureEventLinks()
        {

        }

        public virtual void RemoveEventLinks()
        {

        }

        public virtual void ResetState()
        {
            // Unblock any active movement restrictions
            Unblock();

            // Stop speed calculations and clear internal directions
            Stop();
        }

        protected void RaiseMovementPerformed()
        {
            OnMovementStateChanged?.Invoke(this, true);
            OnMovementPerformed?.Invoke(Direction);
        }
    }
}
