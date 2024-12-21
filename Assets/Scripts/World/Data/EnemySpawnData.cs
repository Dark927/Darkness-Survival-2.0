using Characters.Enemy.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World.Components;


namespace World.Data
{
    [CreateAssetMenu(fileName = "NewEnemySpawnData", menuName = "Game/World/Data/Enemy Spawn Data")]
    public class EnemySpawnData : ScriptableObject
    {
        [Header("Enemy Settings")]

        public EnemyData EnemyData;
        public int Count = 0;

        [Header("Time Settings")]

        [Tooltip("The time when enemies should be spawned.")]
        public StageTime SpawnTime;

        [Space]
        [Tooltip("The time during which the enemies have to spawn.")]
        public StageTime SpawnDuration = new StageTime() { Minutes = 0, Seconds = 1 };

        public static implicit operator EnemyData(EnemySpawnData data)
        {
            return data.EnemyData;
        }
    }
}
