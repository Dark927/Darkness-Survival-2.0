using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputType
{
    Movement = 1,
}

public class InputHandler
{
    IControlLayout _controlLayout;
    private Dictionary<InputType, InputAction> _actions;

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
        };
    }

    public T RequestValueFromAction<T>(InputType inputType) where T : struct
    {
        return _actions[inputType].ReadValue<T>();
    }

    public void SubscribeOnActionPerformed(InputType inputType, Action<InputAction.CallbackContext> subscriber)
    {
        _actions[inputType].performed += subscriber;
    }

    public void SubscribeOnActionCanceled(InputType inputType, Action<InputAction.CallbackContext> subscriber)
    {
        _actions[inputType].canceled += subscriber;
    }

}
