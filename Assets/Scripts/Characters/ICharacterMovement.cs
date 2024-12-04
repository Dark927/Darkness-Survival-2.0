
using UnityEngine;

public interface ICharacterMovement
{
    public bool CanMove { get; }
    public float CurrentSpeed { get; }
    public void Move();
    public void Stop();
}
