using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefaultEnemy : MonoBehaviour, IEnemy
{
    private ICharacterMovement _movement;
    private CharacterLookDirection _lookDirection;

    private void Awake()
    {
        _movement = new EnemyMovement(this, FindObjectOfType<Nero>());
        _lookDirection = new CharacterLookDirection(transform);
    }

    private void FixedUpdate()
    {
        _movement.Move();
        _lookDirection.LookForward(_movement.Direction);
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }
}
