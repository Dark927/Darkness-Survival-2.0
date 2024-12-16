using UnityEngine;

[RequireComponent(typeof(CharacterBody))]
public class Nero : MonoBehaviour, IPlayerLogic
{
    #region Fields

    private PlayerData _stats;

    private bool _started = false;
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
        _started = true;
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
        _playerInput.SetBasicAttacks(_attack);

        // ToDo : Move this logic to the another place.
        _animatorController = _body.Visual.GetAnimatorController() as PlayerAnimatorController;
        _attack.OnFastAttack += _animatorController.TriggerFastAttack;
        _attack.OnHeavyAttack += _animatorController.TriggerHeavyAttack;
    }

    #endregion

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    private void OnEnable()
    {
        // ToDo : Implement another logic to avoid OnEnable before Start executed.

        if (_started)
        {
            SetReferences();
        }
    }

    private void OnDisable()
    {
        ClearReferences();
    }

    private void ClearReferences()
    {
        _playerInput.RemoveReferences();
        _attack.OnFastAttack -= _animatorController.TriggerFastAttack;
        _attack.OnHeavyAttack -= _animatorController.TriggerHeavyAttack;
        _animatorController = null;
    }

    #endregion
}
