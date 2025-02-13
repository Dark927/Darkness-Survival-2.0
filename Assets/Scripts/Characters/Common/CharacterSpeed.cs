using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CharacterSpeed
{
    #region Fields 

    public static readonly CharacterSpeed Zero = new CharacterSpeed()
    {
        MaxSpeedMultiplier = 0,
        CurrentSpeedMultiplier = 0,
    };

    private Vector2 _velocity;
    private Vector2 _direction;
    private float _currentSpeedMultiplier;
    private float _maxSpeedMultiplier;

    public event EventHandler<SpeedChangedArgs> OnActualSpeedChanged;
    public event EventHandler<Vector2> OnVelocityUpdate;

    #endregion


    #region Properties

    public float CurrentSpeedMultiplier { get => _currentSpeedMultiplier; set => _currentSpeedMultiplier = value; }
    public float MaxSpeedMultiplier { get => _maxSpeedMultiplier; set => _maxSpeedMultiplier = value; }
    public float ActualSpeed { get => Velocity.magnitude; }
    public Vector2 Velocity
    {
        get => _velocity;
        private set => _velocity = value;
    }

    public Vector2 Direction => _direction;

    #endregion


    #region Methods 

    public void Stop()
    {
        CurrentSpeedMultiplier = 0;
        UpdateVelocity(_direction);
    }

    public void ClearDirection()
    {
        _direction = Vector2.zero;
    }

    public void TryUpdateVelocity(Vector2 direction)
    {
        _direction = direction;
        UpdateVelocity(_direction);
    }

    public void UpdateVelocity(Vector2 direction)
    {
        Vector2 calculatedVelocity = direction * CurrentSpeedMultiplier;
        bool speedChanged = calculatedVelocity.sqrMagnitude != Velocity.sqrMagnitude;

        Velocity = calculatedVelocity;
        OnVelocityUpdate?.Invoke(this, Velocity);

        if (speedChanged)
        {
            OnActualSpeedChanged?.Invoke(this, new SpeedChangedArgs(ActualSpeed, MaxSpeedMultiplier));
        }
    }

    public void Set(CharacterSpeed speed)
    {
        CurrentSpeedMultiplier = speed.CurrentSpeedMultiplier;
        MaxSpeedMultiplier = speed.MaxSpeedMultiplier;
    }

    public void SetMaxSpeedMultiplier()
    {
        CurrentSpeedMultiplier = MaxSpeedMultiplier;
    }

    public async UniTask UpdateSpeedMultiplierLinear(float targetSpeedMultiplier, int timeInMs, CancellationToken token)
    {
        float startSpeedMultiplier = CurrentSpeedMultiplier;
        float endSpeedMultiplier = targetSpeedMultiplier;
        float elapsedTime = 0f;

        while (elapsedTime < timeInMs)
        {
            if (token.IsCancellationRequested)
            {
                CurrentSpeedMultiplier = endSpeedMultiplier;
                return;
            }

            CurrentSpeedMultiplier = Mathf.Lerp(startSpeedMultiplier, endSpeedMultiplier, elapsedTime / timeInMs);
            elapsedTime += Time.deltaTime * 1000f;
            await UniTask.Yield();
        }

        CurrentSpeedMultiplier = endSpeedMultiplier;
    }

    #endregion

}