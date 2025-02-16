using System;
using UnityEngine;

namespace Characters.Interfaces
{
    public interface IEntityView
    {
        public event EventHandler OnSideSwitch;

        public bool IsLookingForward(Vector2 lookDirection);
        public void LookForward(Vector2 lookDirection);
    }
}