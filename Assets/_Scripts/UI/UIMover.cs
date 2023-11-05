using DG.Tweening;
using UnityEngine;

namespace _Scripts.UI
{
    public class UIMover : MonoBehaviour
    {
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private float _moveDuration;
        [SerializeField] private Transform _outBorderPoint;
        [SerializeField] private Transform _defaultPositionPoint;
        [SerializeField] private bool _cantMoveOnStart = false;

        private Vector3 _defaultPosition;
        private Sequence _sequence;

        private void Start()
        {
            if (_defaultPositionPoint == null)
            {
                _defaultPosition = transform.position;
            }
            else
            {
                _defaultPosition = _defaultPositionPoint.position;
            }
            
            if (!_cantMoveOnStart)
            {
                transform.position = _outBorderPoint.position;
            }
        }

        public void Move()
        {
            TryKillSequence();
            
            transform.position = _defaultPosition;
            _sequence.Append( transform.DOMove(_targetPoint.position, _moveDuration));
        }
        
        public void MoveBack()
        {
            TryKillSequence();
            
            _sequence.Append(transform.DOMove(_defaultPosition, _moveDuration));
            _sequence.Append(transform.DOMove(_outBorderPoint.position, 0));
        }

        private void TryKillSequence()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }
            
            _sequence = DOTween.Sequence();
        }
    }
}