using System;
using System.Collections.Generic;
using _Scripts.UI;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _Scripts.MyGames
{
    public class MyGamesWindowController : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _openButton;
        [SerializeField] private UIMover _windowUIMover;
        [SerializeField] private UIMover _openButtonUIMover;
        [SerializeField] private UIMover _leaderboardButtonUIMover;
        [SerializeField] private List<GoingToTheWebSite> _allButtons;

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
            _openButton.onClick.AddListener(Open);
        }
        
        private void Awake()
        {
            YandexGame.GetDataEvent += GetData;
        
            if (YandexGame.SDKEnabled == true)
            {
                GetData();
            }
        }

        private void GetData()
        {
            if (YandexGame.EnvironmentData.language == "ru" ||
                YandexGame.EnvironmentData.language == "be" ||
                YandexGame.EnvironmentData.language == "kk" ||
                YandexGame.EnvironmentData.language == "uk" ||
                YandexGame.EnvironmentData.language == "uz")
            {
                
            }

            else
            {
                for (int i = 0; i < _allButtons.Count; i++)
                {
                    if (!_allButtons[i].IsHasLocalization)
                    {
                        _allButtons[i].gameObject.SetActive(false);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
            _openButton.onClick.RemoveListener(Open);
            YandexGame.GetDataEvent -= GetData;
        }

        private void Open()
        {
            _windowUIMover.Move();
            _openButtonUIMover.MoveBack();
            _leaderboardButtonUIMover.MoveBack();
        }

        private void Close()
        {
            _windowUIMover.MoveBack();
            _openButtonUIMover.Move();
            _leaderboardButtonUIMover.Move();
        }
    }
}