using Characters.Interfaces;
using UnityEngine;

public class CharacterStaticMovement : ICharacterMovement
{
    private CharacterSpeed _speed;
    public bool IsMoving => false;
    public Vector2 Direction => Vector2.zero;
    public ref CharacterSpeed Speed => ref _speed;

    public void BlockMovement(int timeInMs)
    {

    }

    public void Move()
    {

    }

    public void SetSpeed(CharacterSpeed speed)
    {

    }

    public void Stop()
    {

    }
}
