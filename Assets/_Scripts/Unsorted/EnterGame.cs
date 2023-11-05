using System;
using System.Collections.Generic;
using _Scripts.Character;
using _Scripts.EggColors;
using _Scripts.Foods;
using _Scripts.Mushrooms;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using YG;

namespace _Scripts.Unsorted
{
    public class EnterGame : MonoBehaviour
    {
        public static EnterGame Instance;
        
        [SerializeField] private List<ObjectChanger> _allChangers;
        [SerializeField] private Clicker _clicker;
        [SerializeField] private CharacterCreator _characterCreator;
        [SerializeField] private UIMover _mergeWindow;
        [SerializeField] private UIMover _clickerWindow;
        [SerializeField] private GameTutorial _gameTutorial;
        [SerializeField] private GameObject _statuya;
        [SerializeField] private GameObject _authorizedGO;
        [SerializeField] private GameObject _unauthorized;
        [SerializeField] private List<TMP_Text> _allSnailsTexts;

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
            
            YandexGame.GetDataEvent += GetData;
        
            if (YandexGame.SDKEnabled == true)
            {
                GetData();
            }
        }

        private void OnDestroy()
        {
            YandexGame.GetDataEvent -= GetData;
        }

        public void GetData()
        {
            _authorizedGO.SetActive(YandexGame.auth);
            _unauthorized.SetActive(!YandexGame.auth);
            UpdateAllSnailsText();
            
            if (YandexGame.savesData.characterCount < 2)
            {
                _gameTutorial.StartTutorial();
            }
                _statuya.SetActive(YandexGame.savesData.isHasStatuya);
            
            
            if (YandexGame.savesData.lastCombination[0] > 0)
            {
                _characterCreator.CreateCharacter(
                    (ColorName) YandexGame.savesData.lastCombination[0],
                    (FoodName) YandexGame.savesData.lastCombination[1],
                    (MushRoomName) YandexGame.savesData.lastCombination[2],
                    true);

                _clicker.CurrentClicks = YandexGame.savesData.lastClickCount;

                _clickerWindow.Move();
                _clicker.CurrentCharacter.GoWalk();
                CameraController.Instance.Activate(CameraType.Character);
            }
            else
            {
                _mergeWindow.Move();
                ActivateChangers();
            }
        }

        private void ActivateChangers()
        {
            for (int i = 0; i < _allChangers.Count; i++)
            {
                _allChangers[i].StartAction();
            }
        }

        public void UpdateAllSnailsText()
        {
            for (int i = 0; i < _allSnailsTexts.Count; i++)
            {
                _allSnailsTexts[i].text = YandexGame.savesData.allSnailCount.ToString();
            }
        }
    }
}