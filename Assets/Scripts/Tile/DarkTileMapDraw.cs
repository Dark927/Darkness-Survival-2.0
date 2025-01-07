using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Dark.Tile;
using UnityEditor;
using System.Runtime.InteropServices;

namespace Dark.Tile
{
    [ExecuteAlways]
    public class DarkTileMapDraw : MonoBehaviour
    {
        [Header("Dark Tile Asset")]
        public DarkTileMap tileMapAsset;
        public Vector2Int Size => tileMapAsset.Size;
        [HideInInspector]
        private uint[] gavno;
        [HideInInspector]
        public uint[] UBO {
            get {
                if(gavno==null)
                    gavno = (uint[])tileMapAsset.UniformBuffer.Clone();
                return gavno;
            }
        }
        private MaterialPropertyBlock mpb;
        private Renderer meshRenderer;
        private ComputeBuffer tilebuf;
        //its a view of the buffer, so we always create it
        public BezelData<uint> BezelData => new(UBO, Size);
        //[HideInInspector]
        public BezelState bezelState = BezelState.None;
    
        [ExecuteAlways]
        private void Awake()
        {
            //UBO = (uint[])tileMapAsset.UniformBuffer.Clone();
            meshRenderer = this.GetComponent<Renderer>();
        }
        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            var BufferUsed = Application.isPlaying ? UBO : tileMapAsset.UniformBuffer;
            GUIStyle playingStyle = new();
            playingStyle.normal.textColor = Color.yellow;

            for (int x = -1; x <= Size.x; x++)
                for (int y = -1; y <= Size.y; y++)
                {
                    var g = this.transform.localScale.x / Size.x * 0.5f;
                    uint idx = BufferUsed[x + 1 + (y + 1) * (Size.x + 2)];

                    // scale * (normalized position with center offset - 0.5f for centering in grid coords)
                    Vector3 pos = transform.localScale * (new Vector2(x + 0.5f, y + 0.5f) / Size - new Vector2(0.5f, 0.5f));
                    //apply rotation of transform to pos
                    pos = transform.rotation * pos;
                    pos += this.transform.position;
                    if(Application.isPlaying)
                        UnityEditor.Handles.Label(pos, idx.ToString(), playingStyle);
                    else
                        UnityEditor.Handles.Label(pos, idx.ToString());
                }
#endif
        }

        public void OnDarkTileEdit(RaycastHit hit, int textureIndex)
        {
            Vector3 localHitPoint = meshRenderer.transform.InverseTransformPoint(hit.point) + new Vector3(0.5f, 0.5f, 0.5f);
            // Determine the X, Y index in the tile buffer
            int xIndex = Mathf.FloorToInt(localHitPoint.x * Size.x);
            int yIndex = Mathf.FloorToInt(localHitPoint.y * Size.y);

            //Undo.RegisterCompleteObjectUndo(tileMapAsset, "Draw Tile");//enable ctrl-z
            Undo.RecordObject(tileMapAsset, "Draw Tile");//enable ctrl-z
            tileMapAsset.UniformBuffer[(yIndex + 1) * (Size.x + 2) + (xIndex + 1)] = (uint)textureIndex;//TODO: replace with my span

            // Mark as dirty so Unity updates it
            EditorUtility.SetDirty(tileMapAsset);
            //PrefabUtility.RecordPrefabInstancePropertyModifications(tileMapAsset);
            //AssetDatabase.SaveAssets();
            //send to the GPU
            gavno = null;
            tilebuf.SetData(UBO);
        }

        private void OnRenderObject()
        {
            try
            {
                mpb ??= new MaterialPropertyBlock();
                tilebuf ??= new ComputeBuffer((Size.x + 2) * (Size.y + 2), sizeof(uint), ComputeBufferType.Default);

                // Pass the tile indices to the shader
                tilebuf.SetData(UBO);
                mpb.SetVector("_GridSize", new Vector4(Size.x, Size.y, 0, 0));//TODO:
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
            tilebuf?.Dispose();
        }
        private void OnDisable()
        {
            tilebuf?.Dispose();
        }
    }
}