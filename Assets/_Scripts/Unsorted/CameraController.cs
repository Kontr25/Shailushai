using System;
using Cinemachine;
using UnityEngine;

namespace _Scripts.Unsorted
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;
        
        [SerializeField] private CinemachineVirtualCamera _characterCamera;
        [SerializeField] private CinemachineVirtualCamera _victoryCamera;
        [SerializeField] private CinemachineVirtualCamera _mergeCamera;
        [SerializeField] private CinemachineVirtualCamera _eggColorCamera;
        [SerializeField] private CinemachineVirtualCamera _foodCamera;
        [SerializeField] private CinemachineVirtualCamera _eggCamera;
        [SerializeField] private CinemachineVirtualCamera _moverCamera;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Activate(CameraType type)
        {
            DisableAllCameras();
            
            switch (type)
            {
                case CameraType.Merge:
                    _mergeCamera.Priority = 10;
                    break;
                case CameraType.Victory:
                    _victoryCamera.Priority = 10;
                    break;
                case CameraType.Egg:
                    _eggCamera.Priority = 10;
                    break;
                case CameraType.Character:
                    _characterCamera.Priority = 10;
                    break;
                case CameraType.Mover:
                    _moverCamera.Priority = 10;
                    break;
                case CameraType.EggColor:
                    _eggColorCamera.Priority = 10;
                    break;
                case CameraType.Food:
                    _foodCamera.Priority = 10;
                    break;
            }
        }

        private void DisableAllCameras()
        {
            _characterCamera.Priority = 0;
            _victoryCamera.Priority = 0;
            _mergeCamera.Priority = 0;
            _eggCamera.Priority = 0;
            _moverCamera.Priority = 0;
            _eggColorCamera.Priority = 0;
            _foodCamera.Priority = 0;
        }

        public CinemachineVirtualCamera Camera(CameraType type)
        {
            switch (type)
            {
                case CameraType.Merge:
                    return _mergeCamera;
                case CameraType.Victory:
                    return _victoryCamera;
                case CameraType.Egg:
                    return _eggCamera;
                case CameraType.Character:
                    return _characterCamera;
                case CameraType.Mover:
                    return _moverCamera;
                case CameraType.EggColor:
                    return _eggColorCamera;
                case CameraType.Food:
                    return _foodCamera;
            }

            return null;
        }
    }
}