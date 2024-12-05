using System;

public class SpeedChangedArgs : EventArgs
{
    #region Properties

    public float CurrentSpeed { get; }
    public float MaxSpeed { get; }

    #endregion


    #region Methods

    public SpeedChangedArgs(float currentSpeed, float maxSpeed)
    {
        CurrentSpeed = currentSpeed;
        MaxSpeed = maxSpeed;
    }

    #endregion
}
