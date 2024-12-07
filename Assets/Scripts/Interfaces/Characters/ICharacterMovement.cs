
using System;

public interface ICharacterMovement
{
    public bool IsAFK { get; }
    public float CurrentSpeed { get; }
    public void Move();
    public void Stop();

    public event EventHandler<SpeedChangedArgs> OnSpeedChanged;
}
