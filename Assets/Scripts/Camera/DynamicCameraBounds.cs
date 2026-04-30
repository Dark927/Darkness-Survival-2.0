using System;
using Cinemachine;
using Gameplay.Components;
using Settings.Global;
using UnityEngine;
using Utilities.ErrorHandling;

namespace Settings.CameraManagement
{
    public class DynamicCameraBounds : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _wallThickness = 2f;

        [Header("Boundary Scaling")]
        [Tooltip("1 = Exact camera size. 1.25 = 25% larger. 0.75 = 25% smaller.")]
        [SerializeField] private Vector2 _commonScale = Vector2.one;

        private CameraService _cameraService;
        private CinemachineVirtualCamera _mainCamera;

        private GameObject[] _activeWalls = null;

        #region Init 

        private void Start()
        {
            _cameraService = ServiceLocator.Current.Get<CameraService>();

            if (_cameraService == null || _cameraService.MainCamera == null)
            {
                ErrorLogger.LogError("DynamicCameraBounds needs a valid CameraService and MainCamera to calculate screen edges!");
                return;
            }

            _mainCamera = _cameraService.MainCamera;
            _cameraService.OnCameraStartFollowPlayer += DestroyAllActiveWalls;
            _cameraService.OnCameraReset += GenerateAllWalls;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            _cameraService.OnCameraStartFollowPlayer -= DestroyAllActiveWalls;
            _cameraService.OnCameraReset -= GenerateAllWalls;
        }

        #endregion

        private void GenerateAllWalls()
        {
            // Get the true screen dimensions based on the camera lens
            float trueScreenHeight = _mainCamera.m_Lens.OrthographicSize * 2f;
            float trueScreenWidth = trueScreenHeight * ((float)Screen.width / Screen.height);

            // Apply the scale multiplier to the dimensions
            float scaledWidth = trueScreenWidth * _commonScale.x;
            float scaledHeight = trueScreenHeight * _commonScale.y;

            // Create 4 BoxCollider2Ds dynamically using the scaled dimensions

            _activeWalls = new GameObject[]
            {
                CreateWall("TopWall", new Vector2(0, (scaledHeight / 2) + (_wallThickness / 2)), new Vector2(scaledWidth + _wallThickness * 2, _wallThickness)),
                CreateWall("BottomWall", new Vector2(0, -(scaledHeight / 2) - (_wallThickness / 2)), new Vector2(scaledWidth + _wallThickness * 2, _wallThickness)),
                CreateWall("LeftWall", new Vector2(-(scaledWidth / 2) - (_wallThickness / 2), 0), new Vector2(_wallThickness, scaledHeight)),
                CreateWall("RightWall", new Vector2((scaledWidth / 2) + (_wallThickness / 2), 0), new Vector2(_wallThickness, scaledHeight))
            };
        }

        private GameObject CreateWall(string wallName, Vector2 localPosition, Vector2 size)
        {
            GameObject wallObj = new GameObject(wallName);
            wallObj.transform.SetParent(transform);

            // Snap it relative to the Virtual Camera's current center position
            wallObj.transform.position = (Vector2)_mainCamera.transform.position + localPosition;

            BoxCollider2D collider = wallObj.AddComponent<BoxCollider2D>();
            collider.size = size;

            return wallObj;
        }

        private void DestroyAllActiveWalls()
        {
            // Prevent double-execution if the array is already cleared
            if (_activeWalls == null)
            {
                return;
            }

            foreach (var wall in _activeWalls)
            {
                if (wall != null)
                {
                    DestroyWall(wall);
                }
            }

            _activeWalls = null;
        }

        private void DestroyWall(GameObject wallObj)
        {
            Destroy(wallObj);
        }
    }
}
