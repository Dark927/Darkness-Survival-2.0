using Characters.Interfaces;
using UnityEngine;

public class CharacterStaticMovement : CharacterMovementBase
{
    public override bool IsMoving => false;
    public override Vector2 Direction => Vector2.zero;

}
