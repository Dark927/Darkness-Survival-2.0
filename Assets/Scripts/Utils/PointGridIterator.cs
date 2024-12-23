using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dark.Utils 
{
    public class PointGridIterator : IEnumerable<Vector2Int>
    {
        private readonly int minX, maxX, minY, maxY;
        public PointGridIterator(int minX, int maxX, int minY, int maxY)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
        }
        public PointGridIterator(int X, int Y)
        {
            this.minX = -X;
            this.maxX =  X;
            this.minY = -Y;
            this.maxY =  Y;
        }
        public IEnumerator<Vector2Int> GetEnumerator()
        {
            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                    yield return new Vector2Int(x, y);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}