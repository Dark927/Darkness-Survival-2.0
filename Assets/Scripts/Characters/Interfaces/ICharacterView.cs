using System;
using UnityEngine;

public interface ICharacterView
{
    public event EventHandler OnSideSwitch;

    public bool IsLookingForward(Vector2 lookDirection);
    public void LookForward(Vector2 lookDirection);
}
