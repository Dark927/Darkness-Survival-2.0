
using System;
using UnityEngine;

public interface ICharacterMovement
{
    public bool IsAFK { get; }
    public float CurrentSpeed { get; }
    public Vector2 Direction { get; }
    public void Move();
    public void Stop();

    public event EventHandler<SpeedChangedArgs> OnSpeedChanged;
}
