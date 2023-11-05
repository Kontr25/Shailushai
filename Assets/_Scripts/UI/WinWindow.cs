using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Snail;
using _Scripts.Sounds;
using _Scripts.UI;
using _Scripts.Unsorted;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;
using CameraType = _Scripts.Unsorted.CameraType;
using CharacterController = _Scripts.Character.CharacterController;

public class WinWindow : MonoBehaviour, IFinishable
{
    [SerializeField] private UIMover[] _uiMovers;
    [SerializeField] private float _delayBetweenMove;
    [SerializeField] private GameObject[] _objectForDisable;
    [SerializeField] private UIMover _clickerMover;
    [SerializeField] private List<Button> _nextLevelButtons;
    [SerializeField] private Button _playerControlButton;
    [SerializeField] private SnailSpawner _snailSpawner;
    [SerializeField] private SnailFindTimer _snailFindTimer;
    [SerializeField] private Hint _desktopHint;
    [SerializeField] private Hint _mobileHint;

    private CharacterController _currentCharacter;
    private WaitForSeconds _delay;

    public CharacterController CurrentCharacter
    {
        get => _currentCharacter;
        set => _currentCharacter = value;
    }

    private void Start()
    {
        _delay = new WaitForSeconds(_delayBetweenMove);
        for (int i = 0; i < _nextLevelButtons.Count; i++)
        {
            _nextLevelButtons[i].onClick.AddListener(Restart);
        }
        _playerControlButton.onClick.AddListener(PlayerControl);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _nextLevelButtons.Count; i++)
        {
            _nextLevelButtons[i].onClick.RemoveListener(Restart);
        }
        _playerControlButton.onClick.RemoveListener(PlayerControl);
    }

    private void Restart()
    {
        SoundManager.Instance.DefaultBG();
        SceneManager.LoadScene(0);
    }

    public void StartActionOnWin()
    {
        _clickerMover.MoveBack();
        SoundManager.Instance.Victory();
        StartCoroutine(MoveUI());
    }

    public void StartActionOnLose()
    {
        
    }

    private IEnumerator MoveUI()
    {
        for (int i = 0; i < _objectForDisable.Length; i++)
        {
            _objectForDisable[i].SetActive(false);
        }
        for (int i = 0; i < _uiMovers.Length; i++)
        {
            _uiMovers[i].Move();
            yield return _delay;
        }
    }

    private void PlayerControl()
    {
        CameraController.Instance.Activate(CameraType.Mover);
        _snailFindTimer.StartTimer();
        
        for (int i = 0; i < _uiMovers.Length; i++)
        {
            _uiMovers[i].MoveBack();
        }

        if (YandexGame.EnvironmentData.deviceType == "desktop")
        {
            _desktopHint.gameObject.SetActive(true);
        }else if (YandexGame.EnvironmentData.deviceType == "mobile")
        {
            _mobileHint.gameObject.SetActive(true);
        }
        
        SoundManager.Instance.DefaultBG();
        
        _snailSpawner.SpawnSnails();

        _currentCharacter.PlayerControl();
    }
}
