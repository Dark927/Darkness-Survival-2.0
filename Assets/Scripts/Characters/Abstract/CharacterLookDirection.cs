
using UnityEngine;

public class CharacterLookDirection
{
    private Transform _characterTransform;

    public CharacterLookDirection(Transform characterTransform)
    {
        _characterTransform = characterTransform;
    }

    public bool IsLookingForward(Vector2 lookDirection)
    {
        float scaleX = _characterTransform.localScale.x;

        bool correctLeftLookSide = (lookDirection.x < 0) && (scaleX < 0);
        bool correctRightLookSide = (lookDirection.x > 0) && (scaleX > 0);
        bool previousLookSide = (lookDirection.x == 0);

        return previousLookSide || (correctLeftLookSide || correctRightLookSide);
    }

    public void LookForward(Vector2 lookDirection)
    {
        int targetScaleX = IsLookingForward(lookDirection) ? 1 : -1;
        Vector3 newScale = _characterTransform.localScale;
        newScale.x *= targetScaleX;
        _characterTransform.localScale = newScale;
    }
}
