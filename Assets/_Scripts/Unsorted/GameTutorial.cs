using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Unsorted
{
    public class GameTutorial : MonoBehaviour
    {
        [SerializeField] private TutorialHand _tutorialHand;
        [SerializeField] private List<Transform> _orderPoints;
        [SerializeField] private List<Button> _disabledTutorialButtons;
        [SerializeField] private float _startDelay;

        private Coroutine _tutorialCoroutine;
        private WaitForSeconds _changePositionWait;
        private WaitForSeconds _startDelayWait;
        private int _currentPointNumber;

        private void Start()
        {
            _changePositionWait = new WaitForSeconds(_tutorialHand.ChangingPositionDelay());
            _startDelayWait = new WaitForSeconds(_startDelay);
        }

        private void OnEnable()
        {
            for (int i = 0; i < _disabledTutorialButtons.Count; i++)
            {
                _disabledTutorialButtons[i].onClick.AddListener(DisableTutorial);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < _disabledTutorialButtons.Count; i++)
            {
                _disabledTutorialButtons[i].onClick.RemoveListener(DisableTutorial);
            }
        }

        private void StopTutorial()
        {
            if (_tutorialCoroutine != null)
            {
                StopCoroutine(_tutorialCoroutine);
                _tutorialCoroutine = null;
            }
        }

        private void DisableTutorial()
        {
            StopTutorial();
            _tutorialHand.StopPulsate();
            gameObject.SetActive(false);
        }

        public void StartTutorial()
        {
            StopTutorial();
            
            _tutorialCoroutine = StartCoroutine(TutorialCoroutine());
        }

        private IEnumerator TutorialCoroutine()
        {
            yield return _startDelayWait;
            _tutorialHand.Pulsate();
            while (true)
            {
                _tutorialHand.transform.position = _orderPoints[_currentPointNumber].position;

                if (_currentPointNumber < _orderPoints.Count-1)
                {
                    _currentPointNumber++;
                }
                else
                {
                    _currentPointNumber = 0;
                }
                
                yield return _changePositionWait;
            }
        }
    }
}