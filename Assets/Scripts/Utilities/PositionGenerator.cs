using UnityEngine;

namespace Utilities.World
{
    public class PositionGenerator
    {
        public static Vector2 GetRandomPositionSquarePattern(Vector2 area)
        {
            return GetRandomPositionSquarePattern(area, Vector2.zero, Vector2.zero);
        }

        public static Vector2 GetRandomPositionSquarePattern(Vector2 area, Vector2 range, Vector2 offset)
        {
            Vector2 position;

            int axisSide = Random.value > 0.5f ? -1 : 1;
            Vector2 halfArea = area / 2f;

            if (Random.value > 0.5f)
            {
                position.x = Random.Range(-area.x, area.x);
                position.y = ((halfArea.y + offset.y) * axisSide) + Random.Range(0, range.y * axisSide);
            }
            else
            {
                position.x = ((halfArea.x + offset.x) * axisSide) + Random.Range(0, range.x * axisSide);
                position.y = Random.Range(-area.y, area.y);
            }

            return position;
        }        
        

        public static Vector2 GetRandomPositionOutsideCamera(Camera camera)
        {
            return GetRandomPositionOutsideCamera(camera, Vector2.zero, Vector2.zero);
        }
        
        public static Vector2 GetRandomPositionOutsideCamera(Camera camera, Vector2 range, Vector2 offset)
        {
            Vector2 cameraPosition = camera.transform.position;

            Vector2 cameraArea;
            cameraArea.y = camera.orthographicSize * 2;
            cameraArea.x = cameraArea.y * camera.aspect;

            Vector2 position = GetRandomPositionSquarePattern(cameraArea, range, offset);

            position += cameraPosition;

            return position;
        }
    }
}
