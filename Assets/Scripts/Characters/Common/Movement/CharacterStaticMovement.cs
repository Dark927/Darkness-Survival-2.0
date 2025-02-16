using Characters.Common.Movement;
using UnityEngine;

public class CharacterStaticMovement : EntityMovementBase
{
    public override bool IsMoving => false;
    public override Vector2 Direction => Vector2.zero;
}
