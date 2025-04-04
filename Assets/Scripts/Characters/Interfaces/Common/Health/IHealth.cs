using System;
using Settings.Global;

namespace Characters.Health
{
    public interface IHealth : IResetable, IDisposable
    {
        #region Properties

        public event Action<float> OnCurrentHpPercentChanged;
        public float MaxHp { get; }
        public float CurrentHp { get; }
        public float CurrentHpPercent { get; }
        public bool IsEmpty { get; }

        #endregion


        #region Methods

        /// <summary>
        /// set current max hp limit to a different value.
        /// </summary>
        /// <param name="amount">new MaxHp amount</param>
        public void SetMaxHpLimit(float amount);

        /// <summary>
        /// instantly heal a certain amount of hp, taking into account the max hp limit
        /// </summary>
        public void Heal(float amount);


        /// <summary>
        /// instantly reduce a certain amount of hp
        /// </summary>
        public void ReduceCurrentHp(float amount);


        /// <summary>
        /// always heal an entity until canceled.
        /// </summary>
        /// <param name="hpPerStep">hp to heal per step</param>
        /// <param name="stepInSec">heal rate in seconds, if rate is 0, hp will be regenerated instantly</param>
        /// <param name="additive">true - can be used with active regenerations, false - disables active regenerations</param>
        public void RegenerateHpAlways(float hpPercentPerStep, float stepInSec = 1f, bool additive = false);


        /// <summary>
        /// heal an entity during given amount of time (or until canceled)
        /// </summary>
        /// <param name="hpPerStep">hp to heal per step</param>
        /// <param name="durationInSec">if duration is 0, hp will be regenerated instantly</param>
        /// <param name="stepInSec">heal rate in seconds</param>
        /// <param name="additive">true - can be used with active regenerations, false - disables active regenerations</param>
        public void RegenerateHpDuringTime(float hpPercentPerStep, float durationInSec, float stepInSec, bool additive = false);


        /// <summary>
        /// reduce permanent hp regeneration amount/sec if it is active 
        /// </summary>
        /// <param name="hpPercentPerSec">amount to reduce, if it is higher than the current permanent one, the HP regeneration will stop</param>
        public void ReducePermanentHpRegeneration(float hpPercentPerSec);

        /// <summary>
        /// Cancel all active temporary hp regenerations
        /// </summary>
        public void CancelTemporaryHpRegeneration();


        /// <summary>
        /// Cancel all permanent hp regenerations
        /// </summary>
        public void CancelPermanentHpRegeneration();

        public void CancelAllHpRegenerations();


        #endregion
    }
}
