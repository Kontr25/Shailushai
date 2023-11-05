using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.EggColors;
using _Scripts.Foods;
using _Scripts.Mushrooms;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using YG;
using Random = UnityEngine.Random;

namespace _Scripts.Character
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private ColorName _colorName;
        [SerializeField] private List<GameObject> _mushrooms;
        [SerializeField] private List<GameObject> _foods;
        [SerializeField] private float _smallScale;
        [SerializeField] private float _appearDuration;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private List<Transform> _patrolPoints;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _updateDelay;
        [SerializeField] private CharacterAnimator _characterAnimator;
        [SerializeField] private float _agentSpeed;
        [SerializeField] private float _rotateToCameraDuration = 0.5f;
        [SerializeField] private CharacterMover _characterMover;
        
        private FloatingJoystick _joystick;
        private Transform _cameraTransform;
        private int _currentPatrolPoinNumber;
        private int _lastPatrolPoinNumber;
        private Coroutine _walkCoroutine;
        private WaitForSeconds _updateWait;
        private bool _isRestored = false;

        public ColorName ColorName => _colorName;

        public List<Transform> PatrolPoints
        {
            get => _patrolPoints;
            set => _patrolPoints = value;
        }

        public Transform CameraTransform
        {
            get => _cameraTransform;
            set => _cameraTransform = value;
        }

        public bool IsRestored
        {
            get => _isRestored;
            set => _isRestored = value;
        }

        public FloatingJoystick Joystick
        {
            get => _joystick;
            set => _joystick = value;
        }

        private void Start()
        {
            _updateWait = new WaitForSeconds(_updateDelay);
            transform.localScale = Vector3.zero;
        }

        public void SetScale()
        {
            if (_isRestored)
            {
                int neededClicks = 25;
                int lastClickCount = YandexGame.savesData.lastClickCount;
                neededClicks *= (YandexGame.savesData.characterCount + 1);
                float scaleValue = 0.7f / neededClicks;
                for (int i = 0; i <lastClickCount; i++)
                {
                    _smallScale += scaleValue;
                }
            }
            
            transform.DOScale(_smallScale, _appearDuration).SetEase(_animationCurve).onComplete = () =>
            {
                transform.localScale = new Vector3(_smallScale, _smallScale, _smallScale);
            };
        }

        private void FixedUpdate()
        {
            UpdateAnimationSpeed();
            _characterAnimator.Move(_agent.velocity.magnitude);
        }

        public void SetMushroom(MushRoomName mashroomName)
        {
            _mushrooms[(int) mashroomName - 1].SetActive(true);
        }

        public void SetFood(FoodName foodName)
        {
            _foods[(int) foodName - 1].SetActive(true);
        }

        public void GoWalk()
        {
            StopWalk();
            _walkCoroutine = StartCoroutine(PatrolCoroutine());
        }

        private void StopWalk()
        {
            if (_agent != null && _agent.enabled)
            {
                _agent.ResetPath();    
            }
            
            if (_walkCoroutine != null)
            {
                StopCoroutine(_walkCoroutine);
                _walkCoroutine = null;
            }
        }
        
        private IEnumerator PatrolCoroutine()
        {
            _currentPatrolPoinNumber = Random.Range(0, _patrolPoints.Count);
            while (true)
            {
                if (_currentPatrolPoinNumber < _patrolPoints.Count)
                {
                    _agent.SetDestination(_patrolPoints[_currentPatrolPoinNumber].position);
                }

                _lastPatrolPoinNumber = _currentPatrolPoinNumber;

                while (_currentPatrolPoinNumber == _lastPatrolPoinNumber)
                {
                    _currentPatrolPoinNumber = Random.Range(0, _patrolPoints.Count);
                }
                yield return _updateWait;
            }
        }

        public void UpdateAnimationSpeed()
        {
            _agent.speed = transform.localScale.x * _agentSpeed;
        }

        public void ScaleUp(float scaleValue)
        {
            float currentScale = transform.localScale.x;
            Vector3 targetScale = new Vector3(currentScale + scaleValue, currentScale + scaleValue,
                currentScale + scaleValue);
            transform.localScale = targetScale;
        }

        public void Dance()
        {
            _characterMover.IsCanMove = false;
            StopWalk();
            _characterAnimator.Dance();
            transform.DOLookAt(
                new Vector3(_cameraTransform.position.x, transform.position.y, _cameraTransform.position.z),
                _rotateToCameraDuration);
        }

        public void PlayerControl()
        {
            StopWalk();
            _agent.enabled = false;
            _characterAnimator.StartMove();
            _characterMover.SetMoverParam(_joystick);
            _characterMover.IsCanMove = true;
        }
    }
}