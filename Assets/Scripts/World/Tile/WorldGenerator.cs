using System;
using UnityEngine;
using World.Components;
using World.Tile;
using Zenject;

namespace World.Generation
{
    public enum WorldGeneration
    {
        Static = 0,
        Scrolling,
    }

    public class WorldGenerator : MonoBehaviour
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
            bool canSetTarget = !_generationStrategy.HasTarget && (PlayerManager.Instance != null);

            if (canSetTarget)
            {
                _generationStrategy.SetTarget(PlayerManager.Instance.GetCharacterTransform());
            }

            _generationStrategy.UpdateTilesOnScreen();
        }

        #endregion

        private void Update()
        {
            _generationStrategy.TryUpdateTilesOnScreen();
        }

        #endregion
    }
}
