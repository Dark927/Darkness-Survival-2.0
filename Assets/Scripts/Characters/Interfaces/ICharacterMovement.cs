
using System;
using UnityEngine;

public interface ICharacterMovement
{
    public bool IsMoving { get; }
    public float SpeedMultiplier { get; }
    public Vector2 Direction { get; }
    public void Move();
    public void Stop();

    public event EventHandler<SpeedChangedArgs> OnSpeedChanged;
}
