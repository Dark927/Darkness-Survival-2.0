using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GlobalEnemyData : MonoBehaviour
{
    private static GlobalEnemyData _instance;
    private EnemySettings _enemySettings;

    public static GlobalEnemyData Instance => _instance;
    public EnemySettings EnemySettings => _enemySettings;

    [Inject]
    public void Construct(EnemySettings settings)
    {
        _enemySettings = settings;
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
