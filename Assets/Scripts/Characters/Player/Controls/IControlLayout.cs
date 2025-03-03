using UnityEngine.InputSystem;

public interface IControlLayout
{
    public InputAction PlayerMovement { get; }
    public InputAction PlayerBasicAttacks { get; }

    public void DisableInputs();
}
