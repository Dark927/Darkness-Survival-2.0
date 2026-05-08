using System;
using UnityEngine;

namespace Characters.Common.Combat.Weapons
{
    public abstract class AutonomousEntityBase : MonoBehaviour
    {
        protected float LifeTime;
        protected float TimeAlive;
        protected Action<AutonomousEntityBase> OnDieAction;

        // Events for external visual/audio components
        public event Action OnEntityActivated;
        public event Action OnEntityDied;

        /// <summary>
        /// Basic init before use
        /// </summary>
        public virtual void Activate(float lifeTime, Action<AutonomousEntityBase> onDieAction)
        {
            LifeTime = lifeTime;
            TimeAlive = 0f;
            OnDieAction = onDieAction;

            // OPTIMIZATION: enable Update only when we use this object
            this.enabled = true;

            OnEntityActivated?.Invoke();
        }

        protected virtual void Update()
        {
            TimeAlive += Time.deltaTime;

            if (TimeAlive >= LifeTime)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            // OPTIMIZATION: disable Update when we don't use this object
            this.enabled = false;

            OnEntityDied?.Invoke();
            OnDieAction?.Invoke(this);
        }
    }
}
