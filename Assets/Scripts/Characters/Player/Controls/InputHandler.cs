using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public enum InputType
{
    Movement = 1,
    Attack = 2,
}

public class InputHandler
{
    public enum ActionState
    {
        Started = 1,
        Performed = 2,
        Canceled = 3,
    }


    #region Fields

    private IControlLayout _controlLayout;
    private Dictionary<InputType, InputAction> _actions;

    #endregion


    #region Properties


    #endregion


    #region Methods

    public InputHandler(IControlLayout controlLayout)
    {
        _controlLayout = controlLayout;
        InitActionsCollection();
    }

    private void InitActionsCollection()
    {
        _actions = new Dictionary<InputType, InputAction>()
        {
            [InputType.Movement] = _controlLayout.PlayerMovement,
            [InputType.Attack] = _controlLayout.PlayerBasicAttacks,
        };
    }

    public void Disable()
    {
        _controlLayout.DisableInputs();
    }


    public T RequestValueFromAction<T>(InputType inputType) where T : struct
    {
        return GetInputAction(inputType).ReadValue<T>();
    }


    private InputAction GetInputAction(InputType type)
    {
        if (_actions.TryGetValue(type, out InputAction value))
        {
            return value;
        }

        Debug.LogError($"InputAction for input type : {type} does not exist! - {this.ToString()}");
        return null;
    }

    #region Add Subscribers

    public void SubscribeOnActionStarted(InputType inputType, Action<InputAction.CallbackContext> subscriber)
    {
        GetInputAction(inputType).started += subscriber;
    }

    public void SubscribeOnActionPerformed(InputType inputType, Action<InputAction.CallbackContext> subscriber)
    {
        GetInputAction(inputType).performed += subscriber;
    }

    public void SubscribeOnActionCanceled(InputType inputType, Action<InputAction.CallbackContext> subscriber)
    {
        GetInputAction(inputType).canceled += subscriber;
    }

    #endregion


    #region Remove Subscribers

    public void RemoveSubscriber(InputType inputType, ActionState state, Action<InputAction.CallbackContext> subscriber)
    {
        Action<InputType, Action<InputAction.CallbackContext>> removingMethod;

        switch (state)
        {
            case ActionState.Started:
                removingMethod = RemoveSubscriberStarted;
                break;

            case ActionState.Performed:
                removingMethod = RemoveSubscriberPerformed;
                break;

            case ActionState.Canceled:
                removingMethod = RemoveSubscriberCanceled;
                break;

            default:
                removingMethod = null;
                break;
        }

        removingMethod(inputType, subscriber);
    }

    private void RemoveSubscriberStarted(InputType inputType, Action<InputAction.CallbackContext> subscriber)
    {
        GetInputAction(inputType).started -= subscriber;
    }

    private void RemoveSubscriberPerformed(InputType inputType, Action<InputAction.CallbackContext> subscriber)
    {
        GetInputAction(inputType).performed -= subscriber;
    }

    private void RemoveSubscriberCanceled(InputType inputType, Action<InputAction.CallbackContext> subscriber)
    {
        GetInputAction(inputType).canceled -= subscriber;
    }

    #endregion

    #endregion
}
