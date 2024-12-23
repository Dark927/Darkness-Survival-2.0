using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dark.Tile
{
    public class PreviewStage : PreviewSceneStage
    {
        private GameObject previewQuad;
        private DarkTileMap tileMapData;
        private Camera previewCamera;

        public void SetTileMap(DarkTileMap tileMap)
        {
            tileMapData = tileMap;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            tileMapData = Selection.activeObject as DarkTileMap;
            if (tileMapData != null)
            {
                //CreatePreviewQuad();
            }
            StageUtility.GoToStage(this, true);
        }

        public void CreatePreviewQuad()
        {
            tileMapData = Selection.activeObject as DarkTileMap;
            //Undo.RegisterCompleteObjectUndo(tileMapData, "Draw Tile");//enable ctrl-z
            // Create a quad for preview
            previewQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            previewQuad.name = "Tile Map Preview Quad";

            // Apply material with the tile map shader
            Shader shader = Shader.Find("Darkness/tile_chunk_unlit"); // Replace with your shader name
            Material previewMaterial = new Material(shader);
            previewMaterial.SetTexture("_MainTex", tileMapData.TextureAtlas); // Replace "_MainTex" with your shader property
            previewMaterial.SetVector("_GridSize", new Vector4(9, 9, 8, 8));
            ComputeBuffer buffer = new ComputeBuffer(121, sizeof(uint), ComputeBufferType.Default);
            previewMaterial.SetBuffer("_TileIndices", buffer);
            previewQuad.GetComponent<MeshRenderer>().sharedMaterial = previewMaterial;

            // Center and scale the quad
            previewQuad.transform.position = Vector3.zero;
            previewQuad.transform.localScale = Vector3.one * 10f;

            MeshGen meshGen = previewQuad.AddComponent<MeshGen>();
            meshGen.tileMapAsset = tileMapData;
            //add quad to scene
            SceneManager.MoveGameObjectToScene(previewQuad, scene);
            addCamera();
            //CaptureScreenshot();
        }

        private void addCamera()
        {
            GameObject cameraObject = new GameObject("Preview Camera (for thumbnails)");
            SceneManager.MoveGameObjectToScene(cameraObject, scene);
            previewCamera = cameraObject.AddComponent<Camera>();
            previewCamera.scene = scene;
            previewCamera.transform.position = new Vector3(0, 0, -5); // Position the camera to see the quad
            previewCamera.orthographic = true;
            previewCamera.aspect = 1;
            previewCamera.orthographicSize = 5; // half of tile's localScale
            previewCamera.clearFlags = CameraClearFlags.SolidColor;
            previewCamera.backgroundColor = Color.gray;
        }

        // Call this method to create a preview and assign it to the ScriptableObject
        public void CaptureScreenshot()
        {
            if (tileMapData == null) return;
            if (previewCamera == null) return;
            // Create a RenderTexture
            RenderTexture renderTexture = new RenderTexture(512, 512, 16);
            previewCamera.targetTexture = renderTexture;

            // Render the camera's view to the RenderTexture
            previewCamera.Render();

            // Create a Texture2D and read pixels from the RenderTexture
            tileMapData.previewImage = new Texture2D(512, 512, TextureFormat.RGB24, false);
            RenderTexture.active = renderTexture;
            tileMapData.previewImage.ReadPixels(new Rect(0, 0, 512, 512), 0, 0);
            tileMapData.previewImage.Apply();

            tileMapData.SavePreviewImage(tileMapData.previewImage);

            // Clean up
            previewCamera.targetTexture = null;
            RenderTexture.active = null;
            Object.DestroyImmediate(renderTexture);
        }

        protected override void OnCloseStage()
        {
            CaptureScreenshot();

            base.OnCloseStage();

            if (previewQuad != null)
            {
                DestroyImmediate(previewQuad);
            }

        }

        protected override void OnDisable()
        {
            // Clean up the quad

            base.OnDisable();
        }

        protected override GUIContent CreateHeaderContent()
        {
            return new GUIContent("Use \"Window/Dark Tile Editor\" to modify this asset");
        }
    }
}