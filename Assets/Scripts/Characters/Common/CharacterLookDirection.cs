
using System;
using Characters.Interfaces;
using UnityEngine;

public class CharacterLookDirection : ICharacterView
{
    #region Fields

    private Transform _characterTransform;
    public event EventHandler OnSideSwitch;

    #endregion


    #region Methods

    #region Init

    public CharacterLookDirection(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    #endregion

    public bool IsLookingForward(Vector2 lookDirection)
    {
        float scaleSign = Mathf.Sign(_characterTransform.localScale.x);
        float directionSign = Mathf.Sign(lookDirection.x);

        bool previousLookSide = (lookDirection.x == 0);

        return previousLookSide || (scaleSign == directionSign);
    }


    public void LookForward(Vector2 lookDirection)
    {
        if (IsLookingForward(lookDirection))
        {
            return;
        }

        Vector3 newScale = _characterTransform.localScale;
        newScale.x *= -1;
        _characterTransform.localScale = newScale;

        OnSideSwitch?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}
