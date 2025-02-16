using Cysharp.Threading.Tasks;
using Settings.Global;
using System;
using UnityEngine;

namespace Characters.Common.Movement
{
    public abstract class EntityMovementBase : IEventListener
    {
        public virtual bool IsMoving { get; }
        public virtual Vector2 Direction { get; }
        public virtual CharacterSpeed Speed { get; }
        public virtual bool IsBlocked { get; }

        public virtual UniTaskVoid MoveAsync(Vector2 direction)
        {
            throw new NotImplementedException();
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

        /// <summary>
        /// Stop the character and block movement for certain time
        /// </summary>
        /// <param name="timeInSec">The amount of time the character has to stand</param>
        public virtual void Block(int timeInMs)
        {

        }

        /// <summary>
        /// Stop the character and block movement untill unblock.
        /// </summary>
        public virtual void Block()
        {

        }


        /// <summary>
        /// Disable the character movement block if it is active.
        /// </summary>
        public virtual void Unblock()
        {

        }


        public virtual void ConfigureEventLinks()
        {

        }

        public virtual void RemoveEventLinks()
        {

        }
    }
}
