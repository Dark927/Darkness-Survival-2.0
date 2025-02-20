using Cysharp.Threading.Tasks;
using Settings.Global;
using System;
using UnityEngine;

namespace Characters.Common.Movement
{
    public abstract class EntityMovementBase : IEntityMovement
    {
        public virtual bool IsMoving { get; }
        public virtual Vector2 Direction { get; }
        public virtual EntitySpeed Speed { get; }
        public virtual bool IsBlocked { get; }

        public event Action<Vector2> OnMovementPerformed;

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

        public virtual void Block(int timeInMs)
        {

        }

        public virtual void Block()
        {

        }

        public virtual void Unblock()
        {

        }


        public virtual void ConfigureEventLinks()
        {

        }

        public virtual void RemoveEventLinks()
        {

        }

        protected void RaiseMovementPerformed()
        {
            OnMovementPerformed?.Invoke(Direction);
        }
    }
}
