using UnityEngine;

// (Tests)
public enum AttackType
{
    Reset = 0,
    Fast,
    Heavy
}

public class Nero : MonoBehaviour, IPlayer
{
    private ICharacterMovement _movement;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _movement = new PlayerMovement(this, _animator);
    }

    private void Update()
    {
        Move();

        // Todo : Implement testing block

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("Die");
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _animator.SetTrigger("Attack");
            _animator.SetInteger("AttackType", (int)AttackType.Fast);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _animator.SetTrigger("Attack");
            _animator.SetInteger("AttackType", (int)AttackType.Heavy);
        }
    }

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

  

    public void Move()
    {
        _movement.Move();
    }
}
