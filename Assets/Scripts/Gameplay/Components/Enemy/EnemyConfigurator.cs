﻿using Characters.Interfaces;
using Settings.Global;
using UnityEngine;
using Utilities.World;

namespace Gameplay.Components.Enemy
{
    public class EnemyConfigurator : ICharacterConfigurator<EnemyController>
    {
        private PlayerService _playerService;
        private EnemyManagementService _managementService;
        private GameStateService _gameStateService;

        private Vector2 _spawnPositionOffset = Vector2.zero;
        private Vector2 _spawnPositionRange = Vector2.zero;

        public EnemyConfigurator() { }

        public EnemyConfigurator(Vector2 spawnPositionRange, Vector2 spawnPositionOffset)
        {
            SetSpawnPositionRange(spawnPositionRange);
            SetSpawnPositionOffset(spawnPositionOffset);
        }

        public void SetSpawnPositionOffset(Vector2 spawnPositionOffset)
        {
            _spawnPositionOffset = spawnPositionOffset;
        }

        public void SetSpawnPositionRange(Vector2 spawnPositionRange)
        {
            _spawnPositionRange = spawnPositionRange;
        }

        public void Configure(EnemyController enemy, Transform target = null)
        {
            _managementService ??= ServiceLocator.Current.Get<EnemyManagementService>();
            _playerService ??= ServiceLocator.Current.Get<PlayerService>();
            _gameStateService ??= ServiceLocator.Current.Get<GameStateService>();

            enemy.transform.position = PositionGenerator.GetRandomPositionOutsideCamera(Camera.main, _spawnPositionRange, _spawnPositionOffset);

            if (target == null)
            {
                return;
            }

            IEnemyLogic enemyLogic = enemy.GetComponentInChildren<IEnemyLogic>();

            if (enemyLogic != null)
            {
                enemyLogic.SetTarget(target);

                enemyLogic.Body.OnBodyDamagedWithArgs += _managementService.EnemyDamagedListener;
                enemyLogic.Body.OnBodyDies += _managementService.EnemyKilledListener;
                _gameStateService?.GameEvent.Subscribe(enemy);
            }
            enemy.ConfigureEventLinks();
        }

        public void Deconfigure(EnemyController enemy)
        {
            enemy.ResetCharacter();
            enemy.EntityLogic.Body.OnBodyDamagedWithArgs -= _managementService.EnemyDamagedListener;
            enemy.EntityLogic.Body.OnBodyDies -= _managementService.EnemyKilledListener;
            _gameStateService?.GameEvent.Unsubscribe(enemy);
        }
    }
}
