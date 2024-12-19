
using UnityEngine;

namespace Characters.Enemy.Data
{
    public enum EnemyType
    {
        Default = 0,
        Toxin = 1,
        Bloody = 2,
        Golden = 3,
    }

    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Game/Characters/Enemy/MainData")]
    public class EnemyData : ScriptableObject
    {
        [Header("Main Info")]

        public string Name;
        public EnemyType Type;
        public GameObject Prefab;

        [Space]
        [Header("Stats")]

        public EnemyStats Stats;
    }
}
