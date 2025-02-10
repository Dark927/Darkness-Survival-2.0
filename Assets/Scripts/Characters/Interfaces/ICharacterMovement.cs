
using UnityEngine;

public interface ICharacterMovement
{
    public bool IsMoving { get; }
    public Vector2 Direction { get; }
    public ref CharacterSpeed Speed { get; }

    public void Move();

    /// <summary>
    /// Stop the character until the next movement
    /// </summary>
    public void Stop();

    /// <summary>
    /// Stop the character and block movement for certain time
    /// </summary>
    /// <param name="timeInSec">The amount of time the character has to stand</param>
    public void BlockMovement(int timeInMs);

}
