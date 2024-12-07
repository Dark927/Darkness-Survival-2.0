
using System;
using UnityEngine.InputSystem;
using UnityEngine;

public enum PlayerAttackType
{
    Reset = 0,
    Fast,
    Heavy
}

public class PlayerBasicAttack
{
    public event EventHandler OnFastAttack;
    public event EventHandler OnHeavyAttack;

    private bool _isAttacking = false;

    public PlayerBasicAttack()
    {
        // ----------------------------------
        // ToDo : Refactore and move this logic to another script which responsible for animations
        // ----------------------------------

        var animationEvents = GameObject.FindObjectOfType<NeroAnimationEvents>();

        if(animationEvents != null )
        {
            animationEvents.AttackFinished += SetAttackFinished;
        }
        else
        {
            Debug.Log("NeroAnimationEvents is null - " + this.ToString());
        }

        // ----------------------------------
    }

    public void AttackPerformedListener(InputAction.CallbackContext context)
    {
        if (_isAttacking)
        {
            return;
        }

        _isAttacking = true;

        int contextValue = (int)context.ReadValue<float>();

        // ToDo : Update attack types logic

        if (contextValue == (int)PlayerAttackType.Fast)
        {
            OnFastAttack?.Invoke(this, EventArgs.Empty);
        }
        else if (contextValue == (int)PlayerAttackType.Heavy)
        {
            OnHeavyAttack?.Invoke(this, EventArgs.Empty);
        }
    }

    public void SetAttackFinished(object sender, EventArgs args)
    {
        _isAttacking = false;
    }
}
