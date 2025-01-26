using Dark.Utils;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

namespace World.Tile
{
    public abstract class GenerationStrategy
    {
        #region Fields 

        private PointGridIterator _iterator;
        private GenerationSettings _settings;

        private Transform _playerTransform;

        #endregion


        #region Properties

        public Vector2 PlayerPosition => _playerTransform.position;
        public GenerationSettings Settings => _settings;
        public PointGridIterator Iterator => _iterator;

        #endregion


        #region Methods

        #region Abstract

        abstract public void UpdateTilesOnScreen();
        abstract public void TryUpdateTilesOnScreen();

        #endregion

        #region Init

        public GenerationStrategy(GenerationSettings settings, ICharacterLogic player)
        {
            _settings = settings;
            _playerTransform = (player as MonoBehaviour).transform;
        }  
        
        public virtual void Init()
        {
            if (!Settings.UseRandomChunkLayout)
            {
                int subdivisionsSqr = Settings.Subdivisions * Settings.Subdivisions;

                if (subdivisionsSqr != Settings.TileChunkPrefabs.Count)
                {
                    throw new ArgumentException("# Can not generate world, because there are not enough prefabs for NOT random generation. - " + this.ToString());
                }
            }
            _iterator = new PointGridIterator(Settings.FieldOfVisionWidth / 2, Settings.FieldOfVisionHeight / 2, Settings.Subdivisions);
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Vector3 IndexToTilePosition(Vector2Int pos)
        {
            return new Vector3(pos.x, pos.y, 0f) * _settings.BlockSize;
        }

        #endregion
    }
}
