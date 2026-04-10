using UnityEngine.InputSystem;

public interface IControlLayout
{
    public class PlayerBasicAttacks
    {
        public PlayerBasicAttacks(InputAction fastAttack, InputAction heavyAttack)
        {
            FastAttack = fastAttack;
            HeavyAttack = heavyAttack;
        }

        public InputAction FastAttack;
        public InputAction HeavyAttack;
    }

    public InputAction PlayerMovement { get; }
    public PlayerBasicAttacks PlayerAttacks { get; }

    public void DisableInputs();
    public void EnableInputs();
}
