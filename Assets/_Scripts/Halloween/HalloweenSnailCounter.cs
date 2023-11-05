using System;
using System.Collections.Generic;
using _Scripts.UI;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _Scripts.Halloween
{
    public class HalloweenSnailCounter : MonoBehaviour
    {
        public static HalloweenSnailCounter Instance;

        [SerializeField] private GameObject _mainGO;
        [SerializeField] private TMP_Text _currentHalloweenSnailCountText;
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private UIMover _windowUiMover;
        [SerializeField] private GameObject _statuya;
        [SerializeField] private OverlayImage _overlayImage;
        [SerializeField] private List<UIMover> _allMovers;

        private bool _windowIsOpen = false;

        public bool WindowIsOpen
        {
            get => _windowIsOpen;
            set => _windowIsOpen = value;
        }

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

        private void Start()
        {
            _openButton.onClick.AddListener(OpenWindow);
            _closeButton.onClick.AddListener(CloseWindow);
            
            YandexGame.GetDataEvent += GetData;
            
            if (YandexGame.SDKEnabled == true)
            {
                GetData();
            }
        }

        private void OnDestroy()
        {
            _openButton.onClick.RemoveListener(OpenWindow);
            _closeButton.onClick.RemoveListener(CloseWindow);
            YandexGame.GetDataEvent += GetData;
        }

        private void GetData()
        {
            UpdateText();
            if (YandexGame.savesData.halloweenSnailCount >= 100)
            {
                _mainGO.SetActive(false);
            }
        }

        private void UpdateText()
        {
            if (YandexGame.savesData.halloweenSnailCount > 100)
            {
                YandexGame.savesData.halloweenSnailCount = 100;
            }
            _currentHalloweenSnailCountText.text = $"{YandexGame.savesData.halloweenSnailCount}/{100}";
        }

        private void OpenWindow()
        {
            _windowUiMover.Move();
            _overlayImage.FadeImage();
            for (int i = 0; i < _allMovers.Count; i++)
            {
                _allMovers[i].MoveBack();
            }

            _windowIsOpen = true;
        }
        
        public void CloseWindow()
        {
            _windowUiMover.MoveBack();
            _overlayImage.FadeImage();
            for (int i = 0; i < _allMovers.Count; i++)
            {
                _allMovers[i].Move();
            }
            _windowIsOpen = false;
        }
        
        public void CloseWindowWithoutButtons()
        {
            _windowUiMover.MoveBack();
            _overlayImage.FadeImage();
            _windowIsOpen = false;
        }

        public void AddSnail(int count)
        {
            YandexGame.savesData.halloweenSnailCount += count;
            UpdateText();
            YandexGame.SaveProgress();
            if (YandexGame.savesData.halloweenSnailCount >= 100)
            {
                YandexGame.savesData.isHasStatuya = true;
                YandexGame.SaveProgress();
                _statuya.SetActive(true);
            }
        }
    }
}