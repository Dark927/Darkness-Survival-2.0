

using UnityEngine;

namespace Gameplay.Components.Items
{
    [System.Serializable]
    public class ItemHpParameters : ItemParametersBase
    {
        [Tooltip("Hp amount to regenerate.")]
        [SerializeField] private float _hpAmount = 1;

        [Header("<color=green>Tip : It is better to use the HP amount" +
            "\nthat can be divided by duration without the remainder</color>")]


        [Tooltip("If the duration is greater than 0, the character will regenerate hp slowly, otherwise - instantly.")]
        [SerializeField] private float _durationInSec = 0f;

        [Header("<color=yellow>The permanent regen does not depend on the duration, " +
            "\nit will regenerate the same hp amount every second</color>" +
            "\n<color=red>until canceled</color>")]
        [SerializeField] private bool _permanentRegeneration = false;


        public float HpAmount => _hpAmount;
        public float DurationInSec => _durationInSec;
        public bool PermanentRegeneration => _permanentRegeneration;


        public override void Set(IItemParameters parameters)
        {
            base.Set(parameters);

            if (parameters is ItemHpParameters xpParameters)
            {
                _hpAmount = xpParameters.HpAmount;
                _durationInSec = xpParameters.DurationInSec;
                _permanentRegeneration = xpParameters.PermanentRegeneration;
            }
        }
    }
}
