using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dark.Tile;
using Dark.Utils;
using Unity.Collections;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

public class WorldScrolling : MonoBehaviour
{
    public Transform playerTransform;
    Vector2Int currentPlayerTilePosition = new Vector2Int(0, 0);
    [SerializeField] Vector2Int playerTilePosition;
    public List<GameObject> prefabs = new();

    [SerializeField] float blockSize = 36;
    Dictionary<Vector2Int, DarkTileMapDraw> loadedTiles;

    [Header("unused")]
    [SerializeField] int blockTileHorizontalCount;
    [Header("unused")]
    [SerializeField] int blockTileVerticalCount;

    [SerializeField] int fieldOfVisionHeight = 3;
    [SerializeField] int fieldOfVisionWidth = 3;
    [SerializeField] int subdivisions = 3;
    private PointGridIterator _iterator;

    private void Awake()
    {
        //Vector2ShortEqualityComparer is important for performance and accuracy
        loadedTiles = new Dictionary<Vector2Int, DarkTileMapDraw>(new Vector2ShortEqualityComparer());
        _iterator = new PointGridIterator(fieldOfVisionWidth / 2, fieldOfVisionHeight / 2, subdivisions);
        currentPlayerTilePosition = playerTilePosition;
    }
    private void Start()
    {
        if (prefabs.Count == 0)
        {
            Debug.LogWarning("There are no prefabs assigned to the WorldScrolling component.");
            return;
        }
        //playerTransform = GameManager.instance.playerTransform;
        UpdateTilesOnScreen();

    }

    private void Update()
    {
        playerTilePosition = Vector2Int.RoundToInt(playerTransform.position / blockSize);

        if (currentPlayerTilePosition != playerTilePosition)
        {
            currentPlayerTilePosition = playerTilePosition;
            UpdateTilesOnScreen();
        }
    }

    /// <summary>
    /// Requests a chunk on indexed integer coord
    /// </summary>
    /// <returns>GameObject (instantiated prefab or reused old object</returns>
    private DarkTileMapDraw ChunkLoad(Vector2Int pos)
    {
        //get from the dict
        if (loadedTiles.TryGetValue(pos, out var tileObj))
            return tileObj;

        //random choice and save
        var randomPrefab = prefabs[UnityEngine.Random.Range(0, prefabs.Count)];
        var obj = Instantiate(randomPrefab);

        var script = obj.GetComponent<DarkTileMapDraw>() ?? throw new Exception("There's a prefab without renderer script");
        loadedTiles.Add(pos, script);
        return script;
    }
    private void LoadNeighbors(DarkTileMapDraw center, Vector2Int pos)
    {
        var bezel = center.BezelData;
        var state = center.bezelState;
        if (!state.HasFlag(BezelState.Top) && loadedTiles.TryGetValue(pos + new Vector2Int(0, -1), out var top))
        {
            state |= BezelState.Top;
            top.BezelData.Center.GetBottom().CopyTo(bezel.Top);
        };
        if (!state.HasFlag(BezelState.Bottom) && loadedTiles.TryGetValue(pos + new Vector2Int(0, 1), out var bottom))
        {
            state |= BezelState.Bottom;
            bottom.BezelData.Center.GetTop().CopyTo(bezel.Bottom);
        }
        if (!state.HasFlag(BezelState.Right) && loadedTiles.TryGetValue(pos + new Vector2Int(1, 0), out var right))
        {
            state |= BezelState.Right;
            right.BezelData.Center.GetLeft().CopyTo(bezel.Right);
        }
        if (!state.HasFlag(BezelState.Left) && loadedTiles.TryGetValue(pos + new Vector2Int(-1, 0), out var left))
        {
            state |= BezelState.Left;
            left.BezelData.Center.GetRight().CopyTo(bezel.Left);
        }
        //CORNERS
        if (!state.HasFlag(BezelState.CornerLT) && loadedTiles.TryGetValue(pos + new Vector2Int(-1, -1), out var LT))
        {
            state |= BezelState.CornerLT;
            bezel.CornerLT = LT.BezelData.Center[^1, ^1];
        };
        if (!state.HasFlag(BezelState.CornerRT) && loadedTiles.TryGetValue(pos + new Vector2Int(1, -1), out var RT))
        {
            state |= BezelState.CornerRT;
            bezel.CornerRT = RT.BezelData.Center[^1, 0];
        }
        if (!state.HasFlag(BezelState.CornerLB) && loadedTiles.TryGetValue(pos + new Vector2Int(-1, 1), out var LB))
        {
            state |= BezelState.CornerLB;
            bezel.CornerLB = LB.BezelData.Center[0, ^1];
        }
        if (!state.HasFlag(BezelState.CornerRB) && loadedTiles.TryGetValue(pos + new Vector2Int(1, 1), out var RB))
        {
            state |= BezelState.CornerRB;
            bezel.CornerRB = RB.BezelData.Center[0, 0];
        }
    }

    private void UpdateTilesOnScreen()
    {
        //1st pass - load chunks
        foreach (Vector2Int pov in _iterator)
        {
            DarkTileMapDraw tile = ChunkLoad(playerTilePosition + pov);
            Vector3 newPosition = IndexToTilePosition(playerTilePosition + pov);

            if (newPosition != tile.transform.position)
            {
                tile.gameObject.transform.position = newPosition;
            }
        }
        //2nd pass - set edges
        foreach (Vector2Int pov in _iterator)
        {
            DarkTileMapDraw tile = ChunkLoad(playerTilePosition + pov);
            LoadNeighbors(tile, playerTilePosition + pov);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector3 IndexToTilePosition(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y, 0f) * blockSize;
    }

    private Vector2Int WrapTilePosition(Vector2Int pos)
    {
        pos.x %= blockTileHorizontalCount;
        if (pos.x < 0)
            pos.x += blockTileHorizontalCount;

        pos.y %= blockTileVerticalCount;
        if (pos.y < 0)
            pos.y += blockTileVerticalCount;

        return pos;//vectors are structs, need return
    }

}
