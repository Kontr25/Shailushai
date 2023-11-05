using System;
using System.Collections.Generic;
using _Scripts.Halloween;
using _Scripts.Sounds;
using _Scripts.UI;
using _Scripts.Unsorted;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using CharacterController = _Scripts.Character.CharacterController;

namespace _Scripts.Snail
{
    public class SnailCounter : MonoBehaviour
    {
        public static SnailCounter Instance;

        [SerializeField] private TMP_Text _remainedSnails;
        [SerializeField] private TMP_Text _allSnailCountText;
        [SerializeField] private TMP_Text _currentSnailCountText;
        [SerializeField] private Button _adsButton;
        [SerializeField] private YandexGame _yaSDK;
        [SerializeField] private RewardSetter _rewardSetter;
        [SerializeField] private UIMover _raminingMover;
        [SerializeField] private UIMover _secondVictoryWindowMover;
        
        private int _currentSnailCount;
        private int _maxSnailCount;
        private bool _isWasVictory = false;
        private CharacterController _currentCharacter;
        [SerializeField] private List<SnailController> _allSnails = new List<SnailController>();

        public CharacterController CurrentCharacter
        {
            get => _currentCharacter;
            set => _currentCharacter = value;
        }

        private void Start()
        {
            _adsButton.onClick.AddListener(WatchAdd);
            YandexGame.GetDataEvent += GetData;
            
            if (YandexGame.SDKEnabled == true)
            {
                GetData();
            }
        }

        public void SnailSpawned()
        {
            _raminingMover.Move();
        }

        private void OnDestroy()
        {
            YandexGame.GetDataEvent -= GetData;
            _adsButton.onClick.RemoveListener(WatchAdd);
        }

        private void GetData()
        {
            UpdateAllSnailCount();
        }
        
        private void UpdateAllSnailCount()
        {
            _allSnailCountText.text = YandexGame.savesData.allSnailCount.ToString();
        }

        public void AddSnail()
        {
            YandexGame.savesData.allSnailCount++;
            YandexGame.NewLeaderboardScores("SnailBord", YandexGame.savesData.allSnailCount);
            YandexGame.SaveProgress();
            EnterGame.Instance.UpdateAllSnailsText(); 
            //HalloweenSnailCounter.Instance.AddSnail(1);
            UpdateRemainingSnailCount();
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

        public void AddSnailToList(SnailController snail)
        {
            _allSnails.Add(snail);
            _maxSnailCount = _allSnails.Count;
            UpdateRemainingSnailCount();
        }

        public void RemoveSnailFromList(SnailController snail)
        {
            if (_allSnails.Contains(snail))
            {
                _allSnails.Remove(snail);
                AddSnail();
            }
        }

        private void UpdateRemainingSnailCount()
        {
            _currentSnailCount = _maxSnailCount - _allSnails.Count;
            _remainedSnails.text = $"{_currentSnailCount}/{_maxSnailCount}";
            _currentSnailCountText.text = $"{_currentSnailCount}";
            if (_currentSnailCount == _maxSnailCount && !_isWasVictory)
            {
                _isWasVictory = true;
                _raminingMover.MoveBack();
                _secondVictoryWindowMover.Move();
                _currentCharacter.Dance();
                SoundManager.Instance.AfterVictory();
                return;
            }
        }

        public void AddBonusSnailCount()
        {
            YandexGame.savesData.allSnailCount += _currentSnailCount; 
            //HalloweenSnailCounter.Instance.AddSnail(_currentSnailCount);
            YandexGame.NewLeaderboardScores("SnailBord", YandexGame.savesData.allSnailCount);
            YandexGame.SaveProgress();
            EnterGame.Instance.UpdateAllSnailsText();
            _adsButton.gameObject.SetActive(false);
            _currentSnailCountText.text = $"{_currentSnailCount * 2}";
        }
        
        public void WatchAdd()
        {
            _rewardSetter.IsCounterReward = true;
            _yaSDK._RewardedShow(1);
        }
    }
}