using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class NeroAnimationEvents : MonoBehaviour
{
    public event EventHandler AttackFinished;

    public void OnAttackFinished()
    {
        AttackFinished?.Invoke(this, EventArgs.Empty);
    }
}
