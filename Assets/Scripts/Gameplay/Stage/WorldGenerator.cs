using System;
using Characters.Player;
using Gameplay.Components;
using Gameplay.Tile;
using Settings.Global;
using UnityEngine;
using Zenject;

namespace Gameplay.Generation
{
    public enum WorldGeneration
    {
        Static = 0,
        Scrolling,
    }

    public class WorldGenerator : MonoBehaviour, IDisposable
    {
        #region Fields 

        private GenerationStrategy _generationStrategy;

        #endregion


        #region Methods

        public static Type GetGenerationStrategyType(WorldGeneration generationType)
        {
            return generationType switch
            {
                WorldGeneration.Scrolling => typeof(WorldScrolling),
                _ => throw new NotImplementedException(),
            };
        }

        #region Init

        [Inject]
        public void Construct(GenerationStrategy generationStrategy)
        {
            _generationStrategy = generationStrategy;
        }

        private void Awake()
        {
            try
            {
                _generationStrategy.Init();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                gameObject.SetActive(false);
            }
        }

        private void Start()
        {
            TryFindTarget();

            if (!_generationStrategy.HasTarget)
            {
                ServiceLocator.Current.Get<PlayerService>().OnPlayerReady += PlayerReadyListener;
            }

            _generationStrategy.UpdateTilesOnScreen();
        }

        public void Dispose()
        {
        }

        #endregion

        private void Update()
        {
            _generationStrategy.TryUpdateTilesOnScreen();
        }

        private void PlayerReadyListener(PlayerCharacterController player)
        {
            bool canSetTarget = !_generationStrategy.HasTarget && (player != null);

            if (canSetTarget)
            {
                _generationStrategy.SetTarget(player.CharacterLogic.Body.Transform);
                ServiceLocator.Current.Get<PlayerService>().OnPlayerReady -= PlayerReadyListener;
            }
        }

        private void TryFindTarget()
        {
            PlayerService playerManager = ServiceLocator.Current.Get<PlayerService>();
            bool canSetTarget = !_generationStrategy.HasTarget && (playerManager != null);

            if (canSetTarget)
            {
                _generationStrategy.SetTarget(playerManager.GetCharacterTransform());
            }
        }

        private void OnDestroy()
        {
            Dispose();
        }

        #endregion
    }
}
