using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Dark.Tile
{
    [CreateAssetMenu(fileName = "DarkTileMap", menuName = "Dark/Tile Map Asset")]
    [Serializable]
    public class DarkTileMap : ScriptableObject
    {
        [SerializeField]
        public Texture2DArray TextureAtlas; //texture atlas to be used
        [SerializeField]
        public uint[] UniformBuffer; //the data about tiles in our tilemap
        [SerializeField]
        [HideInInspector]
        private Vector2Int size;

        [DoNotSerialize]
        public Vector2Int Size
        {
            get => size;
            set
            {
                //reinit array to avoid problems
                if (size.x != value.x || size.y != value.y) //if the size has changed
                {
                    //reinit array to avoid problems
                    UniformBuffer = new uint[(value.x + 2) * (value.y + 2)];
                }
                size = value;
            }
        }

        [DoNotSerialize]
        public Texture2D previewImage; //preview image for Unity

        [HideInInspector]
        [SerializeField]
        private byte[] previewImageData; //serialized preview image in JPEG


        public void SetData(uint[] buffer, Texture2DArray atlas)
        {
            UniformBuffer = buffer;
            TextureAtlas = atlas;
        }


        private void OnEnable()
        {
            if (previewImageData != null && previewImageData.Length > 0)
            {
                // Create a new Texture2D and load the JPEG data
                previewImage = new Texture2D(2, 2);
                previewImage.LoadImage(previewImageData);
            }
        }

        public void SavePreviewImage(Texture2D texture)
        {
            if (texture == null)
            {
                Debug.LogError("Texture is null!");
                return;
            }

            // Encode the Texture2D to PNG format
            byte[] pngData = texture.EncodeToJPG();
            if (pngData != null)
            {
                previewImageData = pngData;
                previewImage = texture;
                // Mark the ScriptableObject as dirty so changes are saved
                EditorUtility.SetDirty(this);
                // Save the asset to persist changes
                AssetDatabase.SaveAssets();
            }
            else
            {
                Debug.LogError("Failed to encode the texture to PNG!");
            }
        }
    }
}