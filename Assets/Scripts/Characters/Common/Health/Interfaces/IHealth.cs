using Characters.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Characters.Health
{
    public interface IHealth : IDamageable
    {
        #region Properties

        public event Action<float> OnCurrentHpPercentChanged;
        public float MaxHp { get; }
        public float CurrentHp { get; }
        public float CurrentHpPercent { get; }
        public bool IsEmpty { get; }

        #endregion


        #region Methods

        public void Heal(float amount);

        #endregion
    }
}
