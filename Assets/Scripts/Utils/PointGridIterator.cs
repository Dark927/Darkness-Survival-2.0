using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dark.Utils 
{
    public class PointGridIterator : IEnumerable<Vector2Int>
    {
        private readonly float minX, maxX, minY, maxY, subdivisions;
        public PointGridIterator(float X, float Y, int subdivisions)
        {
            this.minX = -X;
            this.maxX =  X;
            this.minY = -Y;
            this.maxY =  Y;
            this.subdivisions = subdivisions;
        }

        public PointGridIterator(float minX, float maxX, float minY, float maxY, int subdivisions)
        {
            this.minX = minX;
            this.maxX = maxX;
            this.minY = minY;
            this.maxY = maxY;
            this.subdivisions = subdivisions;
        }

        public IEnumerator<Vector2Int> GetEnumerator()
        {
            for (int x = 0; x <= subdivisions; x++)
            {
                float xOffset = (x / (float)subdivisions) * (maxX - minX);
                for (int y = 0; y <= subdivisions; y++)
                {
                    float yOffset = (y / (float)subdivisions) * (maxY - minY);
                    yield return new Vector2Int((int)Mathf.Round(minX + xOffset), (int)Mathf.Round(minY + yOffset));//int round makes it snap
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}