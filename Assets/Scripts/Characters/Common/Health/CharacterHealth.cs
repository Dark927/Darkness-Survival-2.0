using System;
using UnityEngine;

namespace Characters.Health
{
    public class CharacterHealth : IHealth
    {
        #region Fields 

        private float _maxHp;
        private float _currentHp;

        #endregion

        #region Properties

        public event Action<float> OnCurrentHpPercentChanged;
        public float MaxHp => _maxHp;
        public float CurrentHp => _currentHp;
        public float CurrentHpPercent => CurrentHp / MaxHp * 100;
        public bool IsEmpty => (_currentHp <= 0);

        #endregion

        #region Methods 

        #region Init

        public CharacterHealth(float maxHp)
        {
            _maxHp = maxHp;
            _currentHp = maxHp;
        }

        public CharacterHealth(float maxHp, float currentHp)
        {
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
                Debug.LogWarning($"# {nameof(currentHp)} is greater than {nameof(maxHp)}! {nameof(currentHp)} was set to given max value!");
            }
            _maxHp = maxHp;
            _currentHp = currentHp;
        }

        #endregion

        public void TakeDamage(float damage)
        {
            if (!IsEmpty && (damage > 0))
            {
                UpdateCurrentHp(-damage);
            }
        }

        public void Heal(float amount)
        {
            if ((amount > 0) && (_currentHp < _maxHp))
            {
                UpdateCurrentHp(amount);
            }
        }

        private void UpdateCurrentHp(float amount)
        {
            _currentHp += amount;
            _currentHp = Mathf.Clamp(_currentHp, 0, MaxHp);

            OnCurrentHpPercentChanged?.Invoke(CurrentHpPercent);
        }

        public void ResetState()
        {
            Heal(MaxHp);
        }

        #endregion
    }
}
