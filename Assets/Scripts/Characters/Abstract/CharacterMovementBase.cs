
using System;
using UnityEngine;

namespace Characters.Interfaces
{
    public abstract class CharacterMovementBase : IDisposable
    {
        public virtual bool IsMoving { get; }
        public virtual Vector2 Direction { get; }
        public virtual CharacterSpeed Speed { get; }
        public virtual bool IsBlocked { get; }


        public virtual void Move()
        {

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

        public virtual void Dispose()
        {

        }
    }
}