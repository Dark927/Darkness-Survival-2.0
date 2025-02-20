using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace Characters.Health
{
    public class EntityHealth : IHealth
    {
        #region Fields 

        private float _maxHp;
        private float _currentHp;

        private CancellationTokenSource _regenerationCts;

        #endregion

        #region Properties

        public event Action<float> OnCurrentHpPercentChanged;
        public float MaxHp => _maxHp;
        public float CurrentHp => _currentHp;
        public float CurrentHpPercent => CurrentHp / MaxHp * 100;
        public bool IsEmpty => (_currentHp <= 0);
        public bool CanAcceptDamage => !IsEmpty;

        #endregion

        #region Methods 

        #region Init

        public EntityHealth(float maxHp)
        {
            SetMaxHpLimit(maxHp);
            _currentHp = maxHp;
        }

        public EntityHealth(float maxHp, float currentHp)
        {
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
                Debug.LogWarning($"# {nameof(currentHp)} is greater than {nameof(maxHp)}! {nameof(currentHp)} was set to given max value!");
            }
            _maxHp = maxHp;
            _currentHp = currentHp;
        }

        public void ResetState()
        {
            CancelHpRegeneration();
            Heal(MaxHp);
        }

        #endregion

        public void TakeDamage(float damage)
        {
            if (CanAcceptDamage && (damage > 0))
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

        public void SetMaxHpLimit(float amount)
        {
            _maxHp = amount;
        }

        private void UpdateCurrentHp(float amount)
        {
            _currentHp += amount;
            _currentHp = Mathf.Clamp(_currentHp, 0, MaxHp);

            OnCurrentHpPercentChanged?.Invoke(CurrentHpPercent);
        }


        // Hp regeneration

        public void RegenerateHp(float hpPerStep, float stepInSec, bool additive = false)
        {
            RegenerateHp(hpPerStep, Mathf.Infinity, stepInSec, additive);
        }

        public void RegenerateHp(float hpPerStep, float duration, float stepInSec, bool additive = false)
        {
            if (additive)
            {
                _regenerationCts = _regenerationCts == null
                    ? new CancellationTokenSource() 
                    : _regenerationCts;
            }
            else
            {
                CancelHpRegeneration();
                _regenerationCts = new CancellationTokenSource();
            }

            RegenerateHpAsync(hpPerStep, duration, stepInSec, _regenerationCts.Token).Forget();
        }

        public void CancelHpRegeneration()
        {
            _regenerationCts?.Cancel();
            _regenerationCts?.Dispose();
            _regenerationCts = null;
        }


        private async UniTaskVoid RegenerateHpAsync(float amountPerRate, float duration, float rateSec, CancellationToken token = default)
        {
            UniTask targetRegenerationTask = float.IsPositiveInfinity(duration)
                ? RegenerateHpInfinite(amountPerRate, rateSec, token)
                : RegenerateHpDuringTime(amountPerRate, duration, rateSec, token);

            await targetRegenerationTask;
        }

        private async UniTask RegenerateHpInfinite(float amountPerRate, float rateSec, CancellationToken token)
        {
            while (true)
            {
                await UniTask.WaitForSeconds(rateSec, cancellationToken: token);

                if (token.IsCancellationRequested)
                {
                    break;
                }

                Heal(amountPerRate);
            }
        }

        private async UniTask RegenerateHpDuringTime(float amountPerRate, float duration, float rateSec, CancellationToken token)
        {
            float elapsedTime = 0f;

            while (true)
            {
                await UniTask.WaitForSeconds(rateSec, cancellationToken: token);

                if (token.IsCancellationRequested || elapsedTime > duration)
                {
                    break;
                }

                elapsedTime += rateSec;
                Heal(amountPerRate);
            }
        }


        #endregion
    }
}
