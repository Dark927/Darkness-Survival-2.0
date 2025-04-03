using Gameplay.Components;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewDefaultPoolSettings", menuName = "Game/Settings/Pool/Default Pool Data")]
    public class ObjectPoolData : ScriptableObject
    {
        [SerializeField] private ObjectPoolSettings _poolSettings;

        public ObjectPoolSettings Settings => _poolSettings;
    }
}
