using Characters.Stats;
using UnityEngine;

namespace Characters.Player.Data
{
    [CreateAssetMenu(fileName = "NewPlayerData", menuName = "Game/Characters/Player/PlayerData")]
    public class PlayerCharacterData : AttackableCharacterData
    {
        [Header("Level Data")]

        [SerializeField] private CharacterLevelData _characterLevelData;
        public CharacterLevelData CharacterLevelData => _characterLevelData;
    }
}