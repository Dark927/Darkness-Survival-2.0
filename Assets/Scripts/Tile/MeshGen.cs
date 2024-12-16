using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

[ExecuteAlways]
public class MeshGen : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2Int size = new Vector2Int(9, 9);
    public Vector2Int atlasSize = new Vector2Int(2, 2);
    [Header("set true for quad, it only mirrors list")]
    public bool xmirrorfix = false;
    public bool ymirrorfix = true;

    [Header("Tile Indices")]
    public List<string> grid = new List<string>(9){
        "000000000",
        "000000000",
        "000000000",
        "000000000",
        "000000000",
        "000000000",
        "000000000",
        "000000000",
        "000000000",
    };
    public string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private uint[] tileIndices;

    private MaterialPropertyBlock mpb;
    private MeshRenderer meshRenderer;
    private ComputeBuffer tilebuf;

    [ExecuteAlways]
    private void Awake()
    {
        InitializeTileIndices();
        meshRenderer = this.GetComponent<MeshRenderer>();
    }
    private void OnDrawGizmosSelected()
    {
        InitializeTileIndices();
        #if UNITY_EDITOR
        UnityEditor.Handles.color = Color.white;
        for(int x = 0; x < size.x; x++)
        for (int y = 0; y < size.y; y++)
        {
            uint idx = tileIndices[x+1 + (y+1) * (size.x+2)];
            Vector3 pos = new Vector3(x, y, 0) * (size.x/2) //text offset
                        - this.transform.localScale / 2f // center to corner (chunk)
                        + new Vector3(2f,2f,0.0f); // center to corner (tile)
            //apply rotation of transform to pos
            pos = transform.rotation * pos;
            pos += this.transform.position;



            UnityEditor.Handles.Label(pos, idx.ToString());
        }

        #endif

    }

    private void InitializeTileIndices()
    {
        bool badstr = grid.Any(row => row.Length != size.x);
        if (grid == null || grid.Count != size.y || badstr)
        {
            Debug.LogError("Invalid grid format.");
            if (badstr)
            {
                grid.Where(row => row.Length != size.y).ToList().ForEach(row => Debug.Log($"Row length mismatch: {grid.IndexOf(row)}"));
            }
            if (grid.Count != size.y)
            {
                Debug.LogError("Grid count mismatch.");
            }
            //return;
        }

        Dictionary<char, uint> charToUint = alphabet.Select((c, index) => new { c, index })
                                                    .ToDictionary(x => x.c, x => (uint)x.index);

        tileIndices = new uint[(size.x + 2) * (size.y + 2)];
        // 0 1 2 3         x=0 x<4
        // -1 0 1 2 3 4    x=-1 x<=4
        // 0 1 2 3 4 5     x=0 x<4+2
        // [0] 1 2 3 4 [5] x=1 x<=4

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                char currentChar = grid[ymirrorfix ? (size.y-1-y) : y][xmirrorfix ? (size.x-1-x) : x];

                if (!charToUint.TryGetValue(currentChar, out uint value))
                {
                    Debug.LogError($"Invalid character '{currentChar}' in grid. Defaulting to zero");
                    tileIndices[(y+1) * (size.x+2) + (x+1)] = 0;
                    continue;
                }

                tileIndices[(y+1) * (size.x+2) + (x+1)] = value;
            }
        }
        //Debug.Log("Tile indices generated successfully");



    }

    private void OnRenderObject()
    {
        try{
            mpb ??= new MaterialPropertyBlock();
            tilebuf ??= new ComputeBuffer((size.x+2) * (size.y+2), sizeof(uint), ComputeBufferType.Default);

            if (tileIndices == null)
            {
                InitializeTileIndices();
            }

            // Pass the tile indices to the shader
            tilebuf.SetData(tileIndices);
            mpb.SetVector("_GridSize", new Vector4(size.x, size.y, atlasSize.x, atlasSize.y));
            mpb.SetBuffer("_TileIndices", tilebuf);

            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.SetPropertyBlock(mpb);
        }
        catch (Exception e){
            tilebuf.Dispose();
            tilebuf = null;
            mpb = null;
            meshRenderer = null;
        }
    }

    private void OnDestroy()
    {
        if (tilebuf != null)
        {
            tilebuf.Dispose();
        }
    }
    private void OnDisable()
    {
        if (tilebuf != null)
        {
            tilebuf.Dispose();
        }
    }
}
