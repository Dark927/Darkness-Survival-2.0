

using System;
using Cysharp.Threading.Tasks;
using Settings.Global;
using UnityEngine;

namespace Characters.Common.Movement
{
    public interface IEntityMovement : IEventsConfigurable
    {
        #region Properties

        public bool IsMoving { get; }
        public Vector2 Direction { get; }
        public EntitySpeed Speed { get; }
        public bool IsBlocked { get; }

        #endregion


        #region Events

        public event Action<Vector2> OnMovementPerformed;
        public event EventHandler<bool> OnMovementStateChanged;

        #endregion


        #region Methods

        public void Move(Vector2 direction);

        public void UpdateSpeedSettings(SpeedSettings settings, bool moveWithMaxSpeed = false);

        /// <summary>
        /// Stop the character until the next movement
        /// </summary>
        public void Stop();

        /// <summary>
        /// Stop the character and block movement for certain time
        /// </summary>
        /// <param name="timeInSec">The amount of time the character has to stand</param>
        public void Block(int timeInMs);


        /// <summary>
        /// Stop the character and block movement untill unblock.
        /// </summary>
        public void Block();


        /// <summary>
        /// Disable the character movement block if it is active.
        /// </summary>
        public void Unblock();

        /// <summary>
        /// Applies a temporary modifier to the movement speed (e.g., from a Slow status).
        /// </summary>
        public void SetTemporarySpeedMultiplier(float multiplier);

        /// <summary>
        /// Removes temporary modifiers and restores the underlying speed settings.
        /// </summary>
        public void RestoreSpeedMultiplier();


        /// <summary>
        /// External physics interaction logic
        /// </summary>
        public void ApplyExternalPush(Vector2 direction, float force);


        #endregion
    }
}
