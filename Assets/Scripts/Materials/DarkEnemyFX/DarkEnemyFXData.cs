using System;
using UnityEngine;

namespace Materials.DarkEnemyFX
{
    [Serializable]
    [CreateAssetMenu(fileName = "NewDarkEnemyFXData", menuName = "Game/Material/DarkEnemyFXData")]
    public class DarkEnemyFXData : ScriptableMaterialPropsBase
    {
        public MaterialProps props;

        public override void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            props.UpdateAllProperties(mpb);
        }
    }
}
