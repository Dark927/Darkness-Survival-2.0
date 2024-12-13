using UnityEngine;

[RequireComponent (typeof(CharacterBody))]
public class Nero : MonoBehaviour, IPlayerLogic
{
    #region Fields

    private CharacterBody _body;
    private PlayerBasicAttack _attack;
    private PlayerInput _playerInput;
    private PlayerAnimatorController _animatorController;

    #endregion


    #region Methods

    #region Init

    private void Awake()
    {
        InitComponents();
        InitInput();
        InitBasicAttacks();
    }

    private void Start()
    {
        SetReferences();
    }

    private void InitComponents()
    {
        _body = GetComponent<CharacterBody>();
    }


    private void InitBasicAttacks()
    {
        _attack = new PlayerBasicAttack();
    }

    private void InitInput()
    {
        IControlLayout controlLayout = new DefaultControlLayout();
        InputHandler inputHandler = new InputHandler(controlLayout);
        _playerInput = new PlayerInput(inputHandler);
    }

    private void SetReferences()
    {
        _playerInput.SetMovement(_body.Movement);
        _animatorController = (_body as PlayerBody).AnimatorController;

        _playerInput.SetBasicAttacks(_attack);
        _attack.OnFastAttack += _animatorController.TriggerFastAttack;
        _attack.OnHeavyAttack += _animatorController.TriggerHeavyAttack;
    }

    #endregion

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    #endregion
}
