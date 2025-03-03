using UnityEngine;

namespace World.Data
{
    [CreateAssetMenu(fileName = "NewShadowSetting", menuName = "Game/World/Shadow Settings")]
    public class ShadowSettings : ScriptableObject
    {
        public bool _shadowsEnabled = true;
        public bool _changeSunColor = true;
        public Material _shadowMaterial;
    }
}
