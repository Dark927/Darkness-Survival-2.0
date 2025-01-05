using System.Collections.Generic;
using UnityEngine;

namespace Dark.Utils {
    class Vector2ShortEqualityComparer : IEqualityComparer<Vector2Int>
    {
        public static readonly Vector2ShortEqualityComparer Default = new();

        public bool Equals(Vector2Int x, Vector2Int y)
        {
            return x.x == y.x && x.y == y.y;
        }

        public int GetHashCode(Vector2Int v)
        {
            return (v.x << 16) ^ v.y;//returns something like 0xXXXXYYYY
        }
    }
}