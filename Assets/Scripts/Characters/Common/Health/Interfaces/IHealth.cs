using System;
using Characters.Interfaces;

namespace Characters.Health
{
    public interface IHealth : IDamageable
    {
        #region Properties

        public event Action<float> OnCurrentHpPercentChanged;
        public float MaxHp { get; }
        public float CurrentHp { get; }
        public float CurrentHpPercent { get; }
        public bool IsEmpty { get; }

        #endregion


        #region Methods

        public void Heal(float amount);

        #endregion
    }
}
