using UnityEngine;

namespace Characters.Health
{
    public interface IInvincibility
    {
        public Color EffectColor { get; }
        public bool IsActive { get; }

        public void EnableWithVisual();
        public void EnableWithVisual(Color effectColor);
        public void EnableWithVisual(float time, Color effectColor);
        public void Enable();
        public void Enable(float time);
        public void Disable();
    }
}
