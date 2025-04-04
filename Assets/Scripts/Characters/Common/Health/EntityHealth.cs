using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Characters.Health
{
    public class EntityHealth : IHealth
    {
        #region Fields 

        private float _maxHp;
        private float _currentHp;

        private CancellationTokenSource _temporaryRegenerationCts;
        private float _permanentHpRegenPercent = 0f;

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
            CancelTemporaryHpRegeneration();
            Heal(MaxHp);
        }

        public void Dispose()
        {
            CancelAllHpRegenerations();
        }

        #endregion

        public void ReduceCurrentHp(float amount)
        {
            if (!IsEmpty && (amount > 0))
            {
                UpdateCurrentHp(-amount);
            }
        }

        public void SetMaxHpLimit(float amount)
        {
            if (amount == _maxHp)
            {
                return;
            }
            _maxHp = amount;
            OnCurrentHpPercentChanged?.Invoke(CurrentHpPercent);
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


        // Hp regeneration

        public void RegenerateHpAlways(float hpPercentPerStep, float stepInSec = 1f, bool additive = false)
        {
            if (hpPercentPerStep < 0f || 1f < hpPercentPerStep)
            {
                return;
            }

            if (!additive)
            {
                CancelPermanentHpRegeneration();
            }

            RegenerateHpInfinite(hpPercentPerStep, 1f).Forget();
        }

        public void RegenerateHpDuringTime(float hpPercentPerStep, float durationInSec, float stepInSec = 1f, bool additive = false)
        {
            if (hpPercentPerStep < 0f || 1f < hpPercentPerStep)
            {
                return;
            }

            if (Mathf.Approximately(durationInSec, 0f))
            {
                Heal(hpPercentPerStep);
                return;
            }

            if (!additive)
            {
                CancelTemporaryHpRegeneration();
            }

            _temporaryRegenerationCts ??= new CancellationTokenSource();
            RegenerateHpDuringTime(hpPercentPerStep, durationInSec, stepInSec, _temporaryRegenerationCts.Token).Forget();
        }

        public void CancelTemporaryHpRegeneration()
        {
            if (_temporaryRegenerationCts == null)
            {
                return;
            }

            _temporaryRegenerationCts.Cancel();
            _temporaryRegenerationCts.Dispose();
            _temporaryRegenerationCts = null;
        }

        public void CancelPermanentHpRegeneration()
        {
            _permanentHpRegenPercent = 0f;
        }

        public void CancelAllHpRegenerations()
        {
            CancelPermanentHpRegeneration();
            CancelTemporaryHpRegeneration();
        }

        public void ReducePermanentHpRegeneration(float hpPercentPerSec)
        {
            if (_permanentHpRegenPercent <= 0f || (hpPercentPerSec <= 0f))
            {
                return;
            }

            if (hpPercentPerSec > _permanentHpRegenPercent)
            {
                CancelPermanentHpRegeneration();
            }
            else
            {
                _permanentHpRegenPercent -= hpPercentPerSec;
            }
        }

        private async UniTaskVoid RegenerateHpInfinite(float hpPercentPerRate, float rateSec, CancellationToken token = default)
        {
            if (_permanentHpRegenPercent > 0f)
            {
                _permanentHpRegenPercent += hpPercentPerRate;
                Mathf.Clamp01(_permanentHpRegenPercent);
                return;
            }

            _permanentHpRegenPercent += hpPercentPerRate;
            Mathf.Clamp01(_permanentHpRegenPercent);

            while (_permanentHpRegenPercent > 0f)
            {
                await UniTask.WaitForSeconds(rateSec, cancellationToken: token);

                if (token.IsCancellationRequested)
                {
                    break;
                }

                Heal(MaxHp * _permanentHpRegenPercent);
            }
        }

        private async UniTaskVoid RegenerateHpDuringTime(float hpPercentPerRate, float duration, float rateSec, CancellationToken token = default)
        {
            float elapsedTime = 0f;

            while (true)
            {
                await UniTask.WaitForSeconds(rateSec, cancellationToken: token);

                if (token.IsCancellationRequested || elapsedTime > duration)
                {
                    break;
                }

                Heal(MaxHp * hpPercentPerRate);
                elapsedTime += rateSec;
            }
        }


        #endregion
    }
}
