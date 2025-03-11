using System;
using UnityEngine;

namespace Materials.DarkMainFX
{
    [Serializable]
    [CreateAssetMenu(fileName = "NewDarkMainFXData", menuName = "Game/Material/DarkMainFXData")]
    public class DarkMainFXData : ScriptableMaterialPropsBase
    {
        public MaterialProps props;

        public override void UpdateAllProperties(MaterialPropertyBlock mpb)
        {
            props.UpdateAllProperties(mpb);
        }
    }
}
