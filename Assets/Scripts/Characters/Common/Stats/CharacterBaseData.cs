using UnityEngine;

namespace Characters.Stats
{
    [CreateAssetMenu(fileName = "NewCharacterData", menuName = "Game/Characters/Data/DefaultCharacterData")]
    public class CharacterBaseData : ScriptableObject
    {
        [Header("Stats")]
        public string Name;
        [SerializeField] private CharacterStats _stats;

        public CharacterStats Stats => _stats;
    }
}
