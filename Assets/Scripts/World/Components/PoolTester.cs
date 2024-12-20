using Characters.Enemy.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World.Components;
using World.Data;

public class PoolTester : MonoBehaviour
{
    private ObjectPoolBase<GameObject> _enemyPool;
    [SerializeField] private List<EnemySpawnData> _enemySpawnDataList;
    private GameObject _container;
    private EnemyFactory _enemyFactory;

    private void Start()
    {
        _container = new GameObject("EnemyContainer");
        List<EnemyData> enemyDataList = new List<EnemyData>();

        foreach(var enemySpawnData in _enemySpawnDataList)
        {
            enemyDataList.Add(enemySpawnData.EnemyData);
        }

        _enemyFactory = new EnemyFactory(enemyDataList);
        StartCoroutine(PoolRoutine());
    }

    IEnumerator PoolRoutine()
    {
        GameObject requested;
        int randomIndex;

        while (true)
        {
            randomIndex = Random.Range(0, _enemySpawnDataList.Count);
            requested = _enemyFactory.GetEnemy(_enemySpawnDataList[randomIndex].EnemyData.ID);
            Debug.Log($"Requested enemy is not null -> {requested != null}");
            yield return new WaitForSeconds(1.5f);
        }
    }


}
