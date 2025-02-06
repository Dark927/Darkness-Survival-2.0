using Characters.Enemy.Data;
using Settings;
using Settings.Abstract;
using Zenject;

public sealed class GlobalEnemyData : SingletonBase<GlobalEnemyData>
{
    private EnemySettings _enemySettings;
    public EnemySettings EnemySettings => _enemySettings;

    [Inject]
    public void Construct(EnemySettings settings)
    {
        _enemySettings = settings;
    }

    private void Awake()
    {
        InitInstance();
    }

    public EnemyBodyStats RequestDefaultBodyStats()
    {
        return EnemySettings.BodyStats;
    }
}
