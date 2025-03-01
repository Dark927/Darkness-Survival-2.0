

using UnityEngine;

namespace Gameplay.Components.Items
{
    [System.Serializable]
    public class ItemXpParameters : ItemParametersBase
    {
        [SerializeField] private int _xpAmount;

        public int XpAmount => _xpAmount;

        public override void Set(IItemParameters parameters)
        {
            base.Set(parameters);

            if (parameters is ItemXpParameters xpParameters)
            {
                _xpAmount = xpParameters.XpAmount;
            }
        }
    }
}
