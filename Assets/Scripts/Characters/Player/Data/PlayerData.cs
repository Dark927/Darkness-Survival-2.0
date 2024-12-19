using UnityEngine;

namespace Characters.Player.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game/Characters/Player/MainData")]
    public class PlayerData : ScriptableObject
    {
        public PlayerStats stats;
    }
}