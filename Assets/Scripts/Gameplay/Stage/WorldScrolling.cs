using Dark.Utils;
using Gameplay.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Gameplay.Tile
{
    public class WorldScrolling : GenerationStrategy
    {
        #region Fields

        private Dictionary<Vector2Int, DarkTileMapDraw> _loadedTilesDict;

        private Vector2Int _currentPlayerTilePosition = new Vector2Int(0, 0);
        private Vector2Int _playerTilePosition;

        private GameObjectsContainer _tilesContainer;

        // Ordered generation

        private List<GameObject> _prefabsToCreate;
        private Func<GameObject> _getChunkPrefab;

        #endregion


        #region Methods

        #region Init

        [Inject]
        public WorldScrolling(WorldGenerationData settings, Transform targetTransform = null, GameObjectsContainer container = null) : base(settings, targetTransform)
        {
            InitContainer(container);
        }

        public override void Init()
        {
            base.Init();

            if (Settings.TileChunkPrefabs.Count == 0)
            {
                Debug.LogWarning($"There are no prefabs assigned to the {nameof(WorldScrolling)}.");
                return;
            }

            //Vector2ShortEqualityComparer is important for performance and accuracy
            _loadedTilesDict = new Dictionary<Vector2Int, DarkTileMapDraw>(new Vector2ShortEqualityComparer());
            _currentPlayerTilePosition = _playerTilePosition;

            ConfigureScrolling();
        }

        public void InitContainer(GameObjectsContainer container)
        {
            _tilesContainer = container;

            if (_tilesContainer == null)
            {
                GameObject containerObj = new GameObject("World_Chunk_Container", typeof(GameObjectsContainer));
                _tilesContainer = containerObj.GetComponent<GameObjectsContainer>();
            }
        }

        private void ConfigureScrolling()
        {
            _getChunkPrefab = Settings.UseRandomChunkLayout ? GetRandomChunkPrefab : GetNextAvailableChunkPrefab;

            if (!Settings.UseRandomChunkLayout)
            {
                _prefabsToCreate = new List<GameObject>(Settings.TileChunkPrefabs.Count);
                _prefabsToCreate.AddRange(Settings.TileChunkPrefabs);
            }
        }

        #endregion

        public override void TryUpdateTilesOnScreen()
        {
            if (!HasTarget)
            {
                return;
            }

            _playerTilePosition = Vector2Int.RoundToInt(PlayerPosition / Settings.BlockSize);

            if (_currentPlayerTilePosition != _playerTilePosition)
            {
                _currentPlayerTilePosition = _playerTilePosition;
                UpdateTilesOnScreen();
            }
        }

        public override void UpdateTilesOnScreen()
        {
            Vector2Int pos;

            //1st pass - load chunks
            foreach (Vector2Int pov in Iterator)
            {
                pos = _playerTilePosition + pov;

                DarkTileMapDraw tile = ChunkLoad(pos);
                Vector3 newPosition = IndexToTilePosition(pos);

                if (newPosition != tile.transform.position)
                {
                    tile.gameObject.transform.position = newPosition;
                }
            }

            pos = Vector2Int.zero;

            //2nd pass - set edges
            foreach (Vector2Int pov in Iterator)
            {
                pos = _playerTilePosition + pov;

                DarkTileMapDraw tile = ChunkLoad(pos);
                LoadNeighbors(tile, pos);
            }
        }

        /// <summary>
        /// Requests a chunk on indexed integer coord
        /// </summary>
        /// <returns>GameObject (instantiated prefab or reused old object</returns>
        private DarkTileMapDraw ChunkLoad(Vector2Int pos)
        {
            pos = WrapTilePosition(pos);

            //get from the dict
            if (_loadedTilesDict.TryGetValue(pos, out var tileObj))
            {
                return tileObj;
            }

            // Create new chunk 

            DarkTileMapDraw chunkTileMap = null;
            GameObject chunkPrefab = _getChunkPrefab();

            chunkTileMap = CreateChunk(chunkPrefab);
            if (chunkTileMap != null)
            {
                _loadedTilesDict.Add(pos, chunkTileMap);
            }

            return chunkTileMap;
        }

        private DarkTileMapDraw CreateChunk(GameObject chunkPrefab)
        {
            if (chunkPrefab == null)
            {
                return null;
            }

            var obj = GameObject.Instantiate(chunkPrefab, _tilesContainer.transform);
            obj.name = chunkPrefab.name;

            var chunkTileMap = obj.GetComponent<DarkTileMapDraw>() ?? throw new Exception("There's a prefab without renderer script");

            return chunkTileMap;
        }

        private GameObject GetRandomChunkPrefab()
        {
            return Settings.TileChunkPrefabs[UnityEngine.Random.Range(0, Settings.TileChunkPrefabs.Count)];
        }

        private GameObject GetNextAvailableChunkPrefab()
        {
            var chunkPrefab = _prefabsToCreate.FirstOrDefault();
            _prefabsToCreate.Remove(chunkPrefab);

            return chunkPrefab;
        }


        private void LoadNeighbors(DarkTileMapDraw center, Vector2Int pos)
        {
            var bezel = center.BezelData;
            var state = center.bezelState;

            var directions = new Dictionary<BezelState, Vector2Int>
        {
            { BezelState.Top, new Vector2Int(0, -1) },
            { BezelState.Bottom, new Vector2Int(0, 1) },
            { BezelState.Right, new Vector2Int(1, 0) },
            { BezelState.Left, new Vector2Int(-1, 0) },

            // Corners
            { BezelState.CornerLT, new Vector2Int(-1, -1) },
            { BezelState.CornerRT, new Vector2Int(1, -1) },
            { BezelState.CornerLB, new Vector2Int(-1, 1) },
            { BezelState.CornerRB, new Vector2Int(1, 1) }
        };

            Vector2Int wrappedSidePos;

            // Top
            wrappedSidePos = WrapTilePosition(pos + directions[BezelState.Top]);
            if (!state.HasFlag(BezelState.Top) && _loadedTilesDict.TryGetValue(wrappedSidePos, out var top))
            {
                state |= BezelState.Top;
                top.BezelData.Center.GetBottom().CopyTo(bezel.Top);
            }

            // Bottom
            wrappedSidePos = WrapTilePosition(pos + directions[BezelState.Bottom]);
            if (!state.HasFlag(BezelState.Bottom) && _loadedTilesDict.TryGetValue(wrappedSidePos, out var bottom))
            {
                state |= BezelState.Bottom;
                bottom.BezelData.Center.GetTop().CopyTo(bezel.Bottom);
            }

            // Right
            wrappedSidePos = WrapTilePosition(pos + directions[BezelState.Right]);
            if (!state.HasFlag(BezelState.Right) && _loadedTilesDict.TryGetValue(wrappedSidePos, out var right))
            {
                state |= BezelState.Right;
                right.BezelData.Center.GetLeft().CopyTo(bezel.Right);
            }

            // Left
            wrappedSidePos = WrapTilePosition(pos + directions[BezelState.Left]);
            if (!state.HasFlag(BezelState.Left) && _loadedTilesDict.TryGetValue(wrappedSidePos, out var left))
            {
                state |= BezelState.Left;
                left.BezelData.Center.GetRight().CopyTo(bezel.Left);
            }

            // LT Corner
            wrappedSidePos = WrapTilePosition(pos + directions[BezelState.CornerLT]);
            if (!state.HasFlag(BezelState.CornerLT) && _loadedTilesDict.TryGetValue(wrappedSidePos, out var LT))
            {
                state |= BezelState.CornerLT;
                bezel.CornerLT = LT.BezelData.Center[^1, ^1];
            }

            // RT Corner
            wrappedSidePos = WrapTilePosition(pos + directions[BezelState.CornerRT]);
            if (!state.HasFlag(BezelState.CornerRT) && _loadedTilesDict.TryGetValue(wrappedSidePos, out var RT))
            {
                state |= BezelState.CornerRT;
                bezel.CornerRT = RT.BezelData.Center[^1, 0];
            }

            // LB Corner
            wrappedSidePos = WrapTilePosition(pos + directions[BezelState.CornerLB]);
            if (!state.HasFlag(BezelState.CornerLB) && _loadedTilesDict.TryGetValue(wrappedSidePos, out var LB))
            {
                state |= BezelState.CornerLB;
                bezel.CornerLB = LB.BezelData.Center[0, ^1];
            }

            // RB Corner
            wrappedSidePos = WrapTilePosition(pos + directions[BezelState.CornerRB]);
            if (!state.HasFlag(BezelState.CornerRB) && _loadedTilesDict.TryGetValue(wrappedSidePos, out var RB))
            {
                state |= BezelState.CornerRB;
                bezel.CornerRB = RB.BezelData.Center[0, 0];
            }

            center.UpdateGPUData();
        }


        private Vector2Int WrapTilePosition(Vector2Int pos)
        {
            static int WrapValue(int value, int max) => (value % max + max) % max;

            pos.x = WrapValue(pos.x, Settings.Subdivisions);
            pos.y = WrapValue(pos.y, Settings.Subdivisions);

            return pos;
        }
    }

    #endregion
}
