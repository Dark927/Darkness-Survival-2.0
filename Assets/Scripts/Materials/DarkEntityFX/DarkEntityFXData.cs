using System;
using UnityEngine;

namespace Materials.DarkEntityFX
{
    [Serializable]
    [CreateAssetMenu(fileName = "NewDarkEnemyFXData", menuName = "Game/Material/DarkEnemyFXData")]
    public class DarkEntityFXData : ScriptableMaterialPropsBase
    {
        public MaterialProps props;

        public override void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            props.UpdateAllProperties(mpb);
        }
    }
}
