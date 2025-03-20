using System;
using System.Collections.Generic;
using System.Globalization;
using Characters.Common.Combat;
using Characters.Interfaces;
using Gameplay.Data;
using Gameplay.Stage;
using Gameplay.Visual;
using Settings.Global;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Gameplay.Components.Enemy
{
    public class EnemyManagementService : IService, IInitializable
    {
        #region Fields 

        private EnemyManagementData _data;

        private List<Action<IEntityPhysicsBody, Damage>> _enemyDamagedActions = new List<Action<IEntityPhysicsBody, Damage>>();
        private GameplayIndicatorsService _gameplayIndicatorsService;
        private StageProgressService _stageProgressService;

        #endregion


        #region Methods 

        #region Init

        public EnemyManagementService(EnemyManagementData data)
        {
            _data = data;
        }

        public void Initialize()
        {
            if (_data.UseDamageIndicators)
            {
                AddDamageIndicatorsDisplay();
                _stageProgressService = ServiceLocator.Current.Get<StageProgressService>();
            }
        }

        #endregion


        public void EnemyDamagedListener(object sender, Damage receivedDamage)
        {
            IEntityPhysicsBody enemyBody = (IEntityPhysicsBody)sender;
            _enemyDamagedActions?.ForEach(action => action?.Invoke(enemyBody, receivedDamage));
        }

        public void EnemyKilledListener()
        {
            _stageProgressService.IncrementKills();
        }

        public void AddDamageIndicatorsDisplay()
        {
            _gameplayIndicatorsService = ServiceLocator.Current.Get<GameplayIndicatorsService>();

            if (_gameplayIndicatorsService != null)
            {
                _enemyDamagedActions.Add(DisplayDamageIndicator);
            }
            else
            {
                ErrorLogger.LogComponentIsNull(this.ToString(), nameof(GameplayIndicatorsService));
            }
        }

        private void DisplayDamageIndicator(IEntityPhysicsBody enemyBody, Damage receivedDamage)
        {
            Collider2D bodyCollider = enemyBody.Physics.Collider;
            Vector2 position = new Vector2(bodyCollider.bounds.center.x, bodyCollider.bounds.max.y);

            _gameplayIndicatorsService.DisplayIndicator(
                receivedDamage.Amount.ToString("N1", CultureInfo.InvariantCulture),
                position,
                receivedDamage.NegativeStatus.VisualColor);
        }

        #endregion
    }
}
