﻿using System;
using Characters.Interfaces;
using Settings.Global;

namespace Characters.Health
{
    public interface IHealth : IDamageable, IResetable
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
        /// always heal an entity until canceled.
        /// </summary>
        /// <param name="hpPerStep">hp to heal per step</param>
        /// <param name="stepInSec">heal rate in seconds, if rate is 0, hp will be regenerated instantly</param>
        /// <param name="additive">true - can be used with active regenerations, false - disables active regenerations</param>
        public void RegenerateHp(float amountPerStep, float stepInSec = 1f, bool additive = false);


        /// <summary>
        /// heal an entity during given amount of time (or until canceled)
        /// </summary>
        /// <param name="hpPerStep">hp to heal per step</param>
        /// <param name="durationInSec">if duration is 0, hp will be regenerated instantly</param>
        /// <param name="stepInSec">heal rate in seconds</param>
        /// <param name="additive">true - can be used with active regenerations, false - disables active regenerations</param>
        public void RegenerateHp(float amountPerStep, float durationInSec, float stepInSec, bool additive = false);


        /// <summary>
        /// Cancel all active hp regenerations
        /// </summary>
        public void CancelHpRegeneration();

        #endregion
    }
}
