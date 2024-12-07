using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IControlLayout
{
    public InputAction PlayerMovement { get; }
    public InputAction PlayerBasicAttacks { get; }
}
