using System.Collections;
using System.Collections.Generic;
using _Scripts.EggColors;
using _Scripts.Foods;
using _Scripts.Halloween;
using _Scripts.Mushrooms;
using _Scripts.Snail;
using _Scripts.Sounds;
using _Scripts.UI;
using _Scripts.Unsorted;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YG;
using CameraType = _Scripts.Unsorted.CameraType;

namespace _Scripts.Character
{
    public class CharacterCreator : MonoBehaviour
    {
        [SerializeField] private Button _mergeButton;
        [SerializeField] private List<CharacterController> _characterControllers;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private ParticleSystem _mergeEffect;
        [SerializeField] private AudioSource _mergeSound;
        [SerializeField] private MushroomChanger _mushroomChanger;
        [SerializeField] private EggsChanger _eggsChanger;
        [SerializeField] private FoodChanger _foodChanger;
        [SerializeField] private Transform _mergePoinTransform;
        [SerializeField] private float _mergeMoveDuration;
        [SerializeField] private float _mergeScalingDuration;
        [SerializeField] private List<Transform> _patrolPoints;
        [SerializeField] private UIMover _mergeWindow;
        [SerializeField] private UIMover _openMyGamesWindowButton;
        [SerializeField] private UIMover _openLeaderboardButton;
        [SerializeField] private UIMover _clickerWindow;
        [SerializeField] private LeaderboardButton _leaderboardWindow;
        [SerializeField] private Clicker _clicker;
        [SerializeField] private WinWindow _winWindow;
        [SerializeField] private FloatingJoystick _joystick;
        [SerializeField] private SnailFindTimer _snailFindTimer;

        private CharacterController _currentCharacter;
        private Coroutine _mergeCoroutine;
        private WaitForSeconds _movingWait;
        private WaitForSeconds _scalingWait;
        private bool _isCreated = false;

        private void Start()
        {
            _mergeWindow.Move();
            _movingWait = new WaitForSeconds(_mergeMoveDuration);
            _scalingWait = new WaitForSeconds(_mergeScalingDuration);
            _mergeButton.onClick.AddListener(Merge);
        }

        private void OnDestroy()
        {
            _mergeButton.onClick.RemoveListener(Merge);
        }

        private void Merge()
        {
            if(_isCreated) return;

            _isCreated = true;
            _mergeCoroutine = StartCoroutine(MergeCoroutine());
        }

        private IEnumerator MergeCoroutine()
        {
            CameraController.Instance.Activate(CameraType.Merge);
            _mushroomChanger.transform.DOMove(_mergePoinTransform.position, _mergeMoveDuration);
            _eggsChanger.transform.DOMove(_mergePoinTransform.position, _mergeMoveDuration);
            _foodChanger.transform.DOMove(_mergePoinTransform.position, _mergeMoveDuration);
            
            _mushroomChanger.transform.DOScale(0, _mergeMoveDuration);
            _foodChanger.transform.DOScale(0, _mergeMoveDuration);

            yield return _movingWait;

            CameraController.Instance.Activate(CameraType.Egg);
            _eggsChanger.DestroyEgg();
            SoundManager.Instance.VolumeDown();

            yield return new WaitForSeconds(3f);
            _mergeEffect.Play();
            _mergeSound.Play();
            CreateCharacter(_eggsChanger.CurrentEgg.Color, _foodChanger.CurrentFood.FoodName, _mushroomChanger.CurrentMushRoom.Name, false);
            SoundManager.Instance.VolumeUp();
            
            yield return new WaitForSeconds(3f);

            CameraController.Instance.Activate(CameraType.Character);
            _currentCharacter.GoWalk();
        }

        public void CreateCharacter(ColorName colorName, FoodName foodName, MushRoomName mushRoomName, bool isRestored)
        {
            YandexGame.savesData.lastCombination[0] = (int)colorName;
            YandexGame.savesData.lastCombination[1] = (int)foodName;
            YandexGame.savesData.lastCombination[2] = (int)mushRoomName;
            YandexGame.SaveProgress();
            
            foreach (var character in _characterControllers)
            {
                if (character.ColorName == colorName)
                {
                    _currentCharacter = Instantiate(character, _spawnPoint.position, Quaternion.identity);
                }
            }

            _currentCharacter.IsRestored = isRestored;
            _currentCharacter.SetScale();
            _currentCharacter.PatrolPoints = _patrolPoints;
            _currentCharacter.SetMushroom(mushRoomName);
            _currentCharacter.SetFood(foodName);
            _currentCharacter.Joystick = _joystick;
            _winWindow.CurrentCharacter = _currentCharacter;
            _snailFindTimer.CurrentCharacterController = _currentCharacter;
            SnailCounter.Instance.CurrentCharacter = _currentCharacter;
            
            CinemachineVirtualCamera characterCamera = CameraController.Instance.Camera(CameraType.Character);
            characterCamera.LookAt = _currentCharacter.transform;
            
            CinemachineVirtualCamera victoryCamera = CameraController.Instance.Camera(CameraType.Victory);
            victoryCamera.LookAt = _currentCharacter.transform;
            victoryCamera.Follow = _currentCharacter.transform;
            
            CinemachineVirtualCamera moverCamera = CameraController.Instance.Camera(CameraType.Mover);
            moverCamera.LookAt = _currentCharacter.transform;
            moverCamera.Follow = _currentCharacter.transform;
            
            _currentCharacter.CameraTransform = characterCamera.transform;
            _mergeWindow.MoveBack();
            _openMyGamesWindowButton.MoveBack();
            _openLeaderboardButton.MoveBack();
            if (HalloweenSnailCounter.Instance.WindowIsOpen)
            {
                HalloweenSnailCounter.Instance.CloseWindowWithoutButtons();
            }
            if (_leaderboardWindow.IsOpen)
            {
                _leaderboardWindow.Switch();
            }
            _clickerWindow.Move();
            _clicker.CurrentCharacter = _currentCharacter;
            _clicker.IsActive = true;
        }
    }
}