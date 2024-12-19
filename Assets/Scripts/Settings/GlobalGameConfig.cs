
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "NewGlobalGameConfig", menuName = "Game/Settings/Global Game Config")]
    public class GlobalGameConfig : ScriptableObject
    {
        [Header("Gameplay Settings")]
        [SerializeField] private ObjectPoolSettings _poolSettings;

        public ObjectPoolSettings PoolsSettings => _poolSettings;
    }
}