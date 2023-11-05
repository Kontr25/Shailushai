using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _Scripts.UI
{
    public class LeaderboardButton : MonoBehaviour
    {
        [SerializeField] private List<Button> _openWindowButtons;
        [SerializeField] private UIMover _window;
        [SerializeField] private OverlayImage _image;
        [SerializeField] private LeaderboardYG _leaderboardYg;
        [SerializeField] private List<UIMover> _allMovers;

        private bool _isOpen;

        public bool IsOpen => _isOpen;

        private void Start()
        {
            for (int i = 0; i < _openWindowButtons.Count; i++)
            {
                _openWindowButtons[i].onClick.AddListener(Switch);
            }
            YandexGame.GetDataEvent += GetData;
            
            if (YandexGame.SDKEnabled == true)
            {
                GetData();
            }
        }

        private void GetData()
        {
            _leaderboardYg.UpdateLB();
        }

        private void OnDestroy()
        {
            YandexGame.GetDataEvent -= GetData;
            for (int i = 0; i < _openWindowButtons.Count; i++)
            {
                _openWindowButtons[i].onClick.RemoveListener(Switch);
            }
        }

        public void Switch()
        {
            if (_isOpen == false)
            {
                _window.Move();
                _image.FadeImage();
                _leaderboardYg.UpdateLB();
                for (int i = 0; i < _allMovers.Count; i++)
                {
                    _allMovers[i].MoveBack();
                }
            }
            else
            {
                _window.MoveBack();
                _image.FadeImage();
                for (int i = 0; i < _allMovers.Count; i++)
                {
                    _allMovers[i].Move();
                }
            }

            _isOpen = !_isOpen;
        }
    }
}