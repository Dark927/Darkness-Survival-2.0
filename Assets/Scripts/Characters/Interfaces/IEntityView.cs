using System;
using UnityEngine;

namespace Characters.Common
{
    public interface IEntityView
    {
        public event EventHandler OnSideSwitch;

        public bool IsLookingForward(Vector2 lookDirection);
        public void LookForward(Vector2 lookDirection);
    }
}
