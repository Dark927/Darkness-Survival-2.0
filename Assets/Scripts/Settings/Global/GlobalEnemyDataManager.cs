using Characters.Enemy.Data;
using Settings;
using Settings.Abstract;
using Zenject;

public sealed class GlobalEnemyDataManager : LazySingletonMono<GlobalEnemyDataManager>
{
    private EnemyGlobalData _enemySettings;

    public EnemyGlobalData EnemySettings => _enemySettings;

    [Inject]
    public void Construct(EnemyGlobalData settings)
    {
        _enemySettings = settings;
    }
    public EnemyBodyStats RequestDefaultBodyStats()
    {
        return EnemySettings.BodyStats;
    }
}
