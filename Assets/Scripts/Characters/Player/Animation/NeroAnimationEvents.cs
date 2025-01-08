using System;
using UnityEngine;

public class NeroAnimationEvents : MonoBehaviour
{
    public event EventHandler AttackFinished;

    public void OnAttackFinished()
    {
        AttackFinished?.Invoke(this, EventArgs.Empty);
    }
}
