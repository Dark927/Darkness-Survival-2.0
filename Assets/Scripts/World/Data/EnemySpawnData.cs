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

        public EnemyData Obj;
        public int Count = 0;

        [Header("Time Settings")]

        [Tooltip("The time when enemies should be spawned.")]
        public StageTime SpawnTime;

        [Tooltip("The time during which the enemies have to spawn.")]
        public StageTime SpawnInterval = new StageTime() { Minutes = 0, Seconds = 1 };
    }
}
