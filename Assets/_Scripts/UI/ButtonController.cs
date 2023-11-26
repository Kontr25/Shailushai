using System;
using _Scripts.Mushrooms;
using _Scripts.Unsorted;
using UnityEngine;
using UnityEngine.UI;
using CameraType = _Scripts.Unsorted.CameraType;

namespace _Scripts.UI
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _previousButton;
        [SerializeField] private Button _mushRoomButton;
        [SerializeField] private Button _eggButton;
        [SerializeField] private Button _foodButton;
        [SerializeField] private Button _nextLevelButton;
        [SerializeField] private Image _targetImage;
        [SerializeField] private ObjectChanger _mushroomChanger;
        [SerializeField] private ObjectChanger _eggChanger;
        [SerializeField] private ObjectChanger _foodChanger;

        private ObjectChanger _lastObjectChanger;

        private void Start()
        {
            _mushRoomButton.onClick.AddListener(() => SetObjectChanger(_mushroomChanger));
            _eggButton.onClick.AddListener(() => SetObjectChanger(_eggChanger));
            _foodButton.onClick.AddListener(() => SetObjectChanger(_foodChanger));
            SetObjectChanger(_mushroomChanger);
        }

        private void OnDestroy()
        {
            _mushRoomButton.onClick.RemoveAllListeners();
            _eggButton.onClick.RemoveAllListeners();
            _foodButton.onClick.RemoveAllListeners();
            _nextButton.onClick.RemoveAllListeners();
            _previousButton.onClick.RemoveAllListeners();
        }

        private void SetObjectChanger(ObjectChanger objectChanger)
        {
            if (_lastObjectChanger)
            {
                _nextButton.onClick.RemoveListener(_lastObjectChanger.Next);
                _previousButton.onClick.RemoveListener(_lastObjectChanger.Previous);
            }
            _nextButton.onClick.AddListener(objectChanger.Next);
            _previousButton.onClick.AddListener(objectChanger.Previous);
            _lastObjectChanger = objectChanger;

            switch (objectChanger.ChangerType)
            {
                case ObjectChangerType.Mushroom:
                    CameraController.Instance.Activate(CameraType.Merge);
                    _targetImage.transform.SetParent(_mushRoomButton.transform);
                    break;
                case ObjectChangerType.Egg:
                    CameraController.Instance.Activate(CameraType.EggColor);
                    _targetImage.transform.SetParent(_eggButton.transform);
                    break;
                case ObjectChangerType.Food:
                    CameraController.Instance.Activate(CameraType.Food);
                    _targetImage.transform.SetParent(_foodButton.transform);
                    break;
            }
            
            _targetImage.transform.localPosition = Vector3.zero;
        }
    }
}