using UnityEngine;

namespace UI.Local
{
    public class Slider : VisualSpriteComponent
    {
        private float _currentValue = 100;
        private float _maxScale = 1f;

        public float ActualHpPercent => _currentValue;


        public void UpdateActualValue(float actualValuePercent)
        {
            transform.localScale = new Vector2((_maxScale / 100f) * actualValuePercent, transform.localScale.y);
            _currentValue = actualValuePercent;
        }
    }
}
