using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Dark.Tile;
using UnityEditor;

namespace Dark.Tile
{
    [ExecuteAlways]
    public class MeshGen : MonoBehaviour
    {
        [Header("Dark Tile Asset")]
        public DarkTileMap tileMapAsset;
        public Vector2Int size => tileMapAsset.Size;
        public uint[] UBO => tileMapAsset.UniformBuffer;

        private MaterialPropertyBlock mpb;
        private Renderer meshRenderer;
        private ComputeBuffer tilebuf;

        [ExecuteAlways]
        private void Awake()
        {
            meshRenderer = this.GetComponent<Renderer>();
        }
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.white;
            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                {
                    var g = this.transform.localScale.x / size.x * 0.5f;
                    uint idx = UBO[x + 1 + (y + 1) * (size.x + 2)];

                    // scale * (normalized position with center offset - 0.5f for centering in grid coords)
                    Vector3 pos = transform.localScale * (new Vector2(x + 0.5f, y + 0.5f) / size - new Vector2(0.5f, 0.5f));
                    //apply rotation of transform to pos
                    pos = transform.rotation * pos;
                    pos += this.transform.position;

                    UnityEditor.Handles.Label(pos, idx.ToString());
                }

#endif

        }

        public void OnDarkTileEdit(RaycastHit hit, int textureIndex)
        {
            Vector3 localHitPoint = meshRenderer.transform.InverseTransformPoint(hit.point) + new Vector3(0.5f, 0.5f, 0.5f);
            // Determine the X, Y index in the tile buffer
            int xIndex = Mathf.FloorToInt(localHitPoint.x * size.x);
            int yIndex = Mathf.FloorToInt(localHitPoint.y * size.y);

            //Undo.RegisterCompleteObjectUndo(tileMapAsset, "Draw Tile");//enable ctrl-z
            Undo.RecordObject(tileMapAsset, "Draw Tile");//enable ctrl-z
            tileMapAsset.UniformBuffer[(yIndex + 1) * (size.x + 2) + (xIndex + 1)] = (uint)textureIndex;//TODO: replace with my span
            
            // Mark as dirty so Unity updates it
            EditorUtility.SetDirty(tileMapAsset);
            //PrefabUtility.RecordPrefabInstancePropertyModifications(tileMapAsset);
            //AssetDatabase.SaveAssets();
            //send to the GPU
            
            tilebuf.SetData(UBO);
        }

        private void OnRenderObject()
        {
            try
            {
                mpb ??= new MaterialPropertyBlock();
                tilebuf ??= new ComputeBuffer((size.x + 2) * (size.y + 2), sizeof(uint), ComputeBufferType.Default);

                // Pass the tile indices to the shader
                tilebuf.SetData(UBO);
                mpb.SetVector("_GridSize", new Vector4(size.x, size.y, 0, 0));//TODO:
                mpb.SetBuffer("_TileIndices", tilebuf);

                if (meshRenderer == null)
                    meshRenderer = GetComponent<Renderer>();
                meshRenderer.SetPropertyBlock(mpb);
            }
            catch (Exception e)
            {
                tilebuf.Dispose();
                tilebuf = null;
                mpb = null;
                meshRenderer = null;
                Debug.Log("Exception in OnRenderObject: " + e.Message);
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
}