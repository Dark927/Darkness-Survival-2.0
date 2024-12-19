using System.Collections;
using UnityEngine;
using World.Components;
using World.Data;

public class PoolTester : MonoBehaviour
{
    private ObjectPoolBase<GameObject> _enemyPool;
    [SerializeField] private EnemySpawnData _enemySpawnData;
    GameObject container;

    private void Start()
    {
        container = new GameObject("EnemyContainer");
        _enemyPool = new ObjectPoolBase<GameObject>(PreloadFunc, RequestAction, ReturnAction);
        StartCoroutine(PoolRoutine());
    }

    IEnumerator PoolRoutine()
    {
        GameObject requested;

        while (true)
        {
            requested = _enemyPool.RequestObject();
            Debug.Log(requested != null);
            yield return new WaitForSeconds(1.5f);
        }
    }

    private GameObject PreloadFunc()
    {
        GameObject createdObj = Instantiate(_enemySpawnData.Obj.Prefab);
        createdObj.transform.parent = container.transform;

        return createdObj;
    }

    private void RequestAction(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void ReturnAction(GameObject obj)
    {
        obj.SetActive(false);
    }
}
