
using UnityEngine;

namespace Characters.Player.Data
{
    [CreateAssetMenu(fileName = "NewCharacterLevelData", menuName = "Game/Characters/Player/Character Level Data")]
    public class CharacterLevelData : ScriptableObject
    {
        #region Fields 

        [Header("Level calculation parameters")]

        [SerializeField] private float _starter = 0f;
        [SerializeField] private float _multiplier = 20f;
        [SerializeField] private float _exp = 2.2f;

        #endregion


        #region Properties

        public float Starter => _starter;
        public float Multiplier => _multiplier;
        public float Exp => _exp;

        #endregion
    }
}
