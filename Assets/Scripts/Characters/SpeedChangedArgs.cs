using System;

public class SpeedChangedArgs : EventArgs
{
    public float CurrentSpeed { get; }
    public float MaxSpeed { get; }

    public SpeedChangedArgs(float currentSpeed, float maxSpeed)
    {
        CurrentSpeed = currentSpeed;
        MaxSpeed = maxSpeed;
    }
}
