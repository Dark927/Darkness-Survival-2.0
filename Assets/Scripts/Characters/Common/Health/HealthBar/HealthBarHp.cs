using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.Health.HealthBar
{
    public class HealthBarHp : HealthBarVisual
    {
        private float _actualHpPercent = 100;
        private float _maxHpScale = 1f;

        public float ActualHpPercent => _actualHpPercent;


        public void UpdateActualHp(float actualHpPercent)
        {
            transform.localScale = new Vector2((_maxHpScale / 100f) * actualHpPercent, transform.localScale.y);
            _actualHpPercent = actualHpPercent;
        }
    }
}
