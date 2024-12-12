using UnityEngine;

public interface ICharacterView 
{
    public bool IsLookingForward(Vector2 lookDirection);
    public void LookForward(Vector2 lookDirection);
}
