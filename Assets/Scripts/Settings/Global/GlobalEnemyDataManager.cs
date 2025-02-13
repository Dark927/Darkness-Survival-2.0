using Characters.Enemy.Data;
using Settings;
using Settings.Abstract;
using Zenject;

public sealed class GlobalEnemyDataManager : LazySingleton<GlobalEnemyDataManager>
{
    private EnemySettings _enemySettings;

    public EnemySettings EnemySettings => _enemySettings;

    [Inject]
    public void Construct(EnemySettings settings)
    {
        _enemySettings = settings;
    }
    public EnemyBodyStats RequestDefaultBodyStats()
    {
        return EnemySettings.BodyStats;
    }
}
