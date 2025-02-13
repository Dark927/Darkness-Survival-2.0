using System.Collections.Generic;
using UnityEngine;

namespace World.Tile
{
    [CreateAssetMenu(fileName = "NewWorldGenerationSettings", menuName = "Game/World/Tile/Generation Settings")]
    public class GenerationSettings : ScriptableObject
    {
        [Header("Main Settings")]

        public List<GameObject> TileChunkPrefabs = new();

        public float BlockSize = 36;

        public int FieldOfVisionHeight = 3;
        public int FieldOfVisionWidth = 3;
        public int Subdivisions = 3;

        [Space, Header("Extra Settings")]
        public bool UseRandomChunkLayout = true;
    }
}
