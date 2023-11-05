using System;
using System.Collections;
using _Scripts.Sounds;
using _Scripts.UI;
using TMPro;
using UnityEngine;
using CharacterController = _Scripts.Character.CharacterController;

namespace _Scripts.Snail
{
    public class SnailFindTimer : MonoBehaviour
    {
        [SerializeField] private float _maxTime;
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private UIMover _secondWinWindow;
        
        private Coroutine _timerCoroutine;
        private WaitForSeconds _secondWait;
        private CharacterController _currentCharacterController;

        public CharacterController CurrentCharacterController
        {
            get => _currentCharacterController;
            set => _currentCharacterController = value;
        }

        private void Start()
        {
            _secondWait = new WaitForSeconds(1);
        }

        public void StartTimer()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }

            _timerCoroutine = StartCoroutine(TimerCoroutine());
        }
        
        private IEnumerator TimerCoroutine()
        {
            float currentTime = _maxTime;
            
            while (currentTime > 0)
            {
                currentTime -= 1.0f;
                _timerText.text = "00:" + currentTime.ToString("F0");
                yield return _secondWait;
            }
            
            _currentCharacterController.Dance();
            SoundManager.Instance.AfterVictory();
            _timerText.text = 0.ToString();
            _secondWinWindow.Move();
        }
    }
}