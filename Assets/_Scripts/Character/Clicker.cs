using System;
using System.Collections;
using _Scripts.PoolObject;
using _Scripts.Sounds;
using _Scripts.UI;
using _Scripts.Unsorted;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;
using CameraType = _Scripts.Unsorted.CameraType;
using Random = UnityEngine.Random;

namespace _Scripts.Character
{
    public class Clicker : MonoBehaviour
    {
        [SerializeField] private YandexGame _yaSDK;
        [SerializeField] private TMP_Text _neededClicksText;
        [SerializeField] private TMP_Text _currentClicksText;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private float _boostDuration;
        [SerializeField] private string _defaultTimerText;
        [SerializeField] private Image _progressBar;
        [SerializeField] private Button _watchAddButton;
        [SerializeField] private Transform _container;
        [SerializeField] private int _pollCapacity;
        [SerializeField] private Words _wordsPrefab;
        [SerializeField] private RectTransform canvasRect;
        [SerializeField] private TutorialHand _tutorialHand;
        [SerializeField] private RewardSetter _rewardSetter;
        
        private CharacterController _currentCharacter;
        private bool _isActive = false;
        private int _characterCount;
        private int _neededClicks = 25;
        private int _currentClicks = 0;
        private Coroutine _boostCoroutine;
        private WaitForSeconds _secondWait;
        private const float _second = 1f;
        private int _ckickValue = 1;
        private float _scaleValue;
        private Pool<Words> _pool;
        

        public CharacterController CurrentCharacter
        {
            get => _currentCharacter;
            set => _currentCharacter = value;
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value)
                {
                    _tutorialHand.Pulsate();
                    _characterCount = YandexGame.savesData.characterCount;
                    if (_characterCount > 0)
                    {
                        _neededClicks *= (_characterCount + 1);
                        
                        if (_neededClicks > 300) _neededClicks = 300;
                    }
                    _scaleValue = 0.7f / _neededClicks;
                    _neededClicksText.text = _neededClicks.ToString();
                }
                
                _isActive = value;
            }
        }

        public int CurrentClicks
        {
            get => _currentClicks;
            set
            {
                _currentClicksText.text = value.ToString();
                float currentClicks = value;
                _progressBar.fillAmount = currentClicks / _neededClicks;
                _currentClicks = value;
            }
        }

        private void Awake()
        {
            YandexGame.GetDataEvent += GetData;
        
            if (YandexGame.SDKEnabled == true)
            {
                GetData();
            }
        }

        private void OnDestroy()
        {
            YandexGame.GetDataEvent -= GetData;
            _watchAddButton.onClick.RemoveListener(WatchAdd);
        }

        public void GetData()
        {
            _levelText.text = (YandexGame.savesData.characterCount + 1).ToString();
            _pool = new Pool<Words>(_wordsPrefab, _pollCapacity, _container)
            {
                AutoExpand = true
            };
        }

        private void Start()
        {
            _watchAddButton.onClick.AddListener(WatchAdd);
            _secondWait = new WaitForSeconds(_second);
            _timerText.text = _defaultTimerText;
        }

        private void Update()
        {
            if(!_isActive) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (_tutorialHand.gameObject.activeInHierarchy)
                {
                    _tutorialHand.gameObject.SetActive(false);
                }
                _currentClicks += _ckickValue;
                YandexGame.savesData.lastClickCount = _currentClicks;
                float currentClicks = _currentClicks;
                float neededClicks = _neededClicks;
                _progressBar.fillAmount = currentClicks / neededClicks;
                
                _currentCharacter.ScaleUp(_scaleValue * _ckickValue);
                if (_currentClicks >= _neededClicks)
                {
                    _currentClicks = _neededClicks;
                    LevelComplete();
                }
                _currentClicksText.text = _currentClicks.ToString();
                WordShot();
                YandexGame.SaveProgress();
            }
        }

        private void WordShot()
        {
            var word = _pool.GetFreeElement();
            word.transform.SetParent(_container);
            word.SetPosition(GetRandomPosition());
        }
        
        private Vector2 GetRandomPosition()
        {
            if (canvasRect == null)
            {
                Debug.LogError("Не установлена ссылка на RectTransform канваса.");
                return Vector2.zero;
            }

            Vector2 canvasSize = canvasRect.rect.size;

            float randomX = Random.Range(-canvasSize.x / 2f, canvasSize.x / 2f);
            float randomY = Random.Range(-canvasSize.y / 2f, canvasSize.y / 2f);

            return new Vector2(randomX, randomY);
        }
        
        public void WatchAdd()
        {
            _rewardSetter.IsClickerReward = true;
            _yaSDK._RewardedShow(1);
        }

        private void LevelComplete()
        {
            _isActive = false;
            _currentCharacter.transform.localScale = Vector3.one;
            _currentCharacter.Dance();
            SoundManager.Instance.AfterVictory();
            YandexGame.savesData.characterCount++;
            YandexGame.savesData.lastCombination[0] = -1;
            YandexGame.savesData.lastCombination[1] = -1;
            YandexGame.savesData.lastCombination[2] = -1;
            YandexGame.savesData.lastClickCount = 0;
            YandexGame.SaveProgress();
            FinishAction.Finish.Invoke(FinishAction.FinishType.Win);
            CameraController.Instance.Activate(CameraType.Victory);
        }

        public void AddBoost()
        {
            if (_boostCoroutine != null)
            {
                StopCoroutine(_boostCoroutine);
                _boostCoroutine = null;
            }

            _boostCoroutine = StartCoroutine(BoostCoroutine());
        }
        
        private IEnumerator BoostCoroutine()
        {
            _ckickValue = 5;
            float currentTime = _boostDuration;
            
            while (currentTime > 0)
            {
                currentTime -= 1.0f;
                _timerText.text = "00:" + currentTime.ToString("F0");
                yield return _secondWait;
            }
            
            _timerText.text = _defaultTimerText;
            _ckickValue = 1;
        }
    }
}