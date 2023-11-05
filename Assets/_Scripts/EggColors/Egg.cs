using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Mushrooms;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.EggColors
{
    public class Egg : MonoBehaviour
    {
        [SerializeField] private ColorName _color;
        [SerializeField] private Ease _ease = Ease.OutBounce;
        [SerializeField] private Rigidbody _eggPart;
        [SerializeField] private float _explosionForce;
        [SerializeField] private float _explosionRadius;
        [SerializeField] private Transform _explosionPoint;
        [SerializeField] private float _disappearDelay;
        [SerializeField] private List<Collider> _eggPartColliders;
        [SerializeField] private List<Rigidbody> _eggPartRigidbodies;
        [SerializeField] private GameObject _meshGO;

        private Sequence _sequence;
        private Coroutine _destroyEggCoroutine;
        private WaitForSeconds _disapearDelayWait;

        public ColorName Color => _color;

        private void Start()
        {
            _disapearDelayWait = new WaitForSeconds(_disappearDelay);
        }

        public void Appear(float appearTime)
        {
            _meshGO.SetActive(true);
            TryKillSequence();
            _sequence = DOTween.Sequence();
            
            _sequence.Insert(0,transform.DOScale(Vector3.one, appearTime)).SetEase(_ease);
        }

        public void Disappear(float disappearTime)
        {
            TryKillSequence();
            _sequence = DOTween.Sequence();

            _sequence.Insert(0, transform.DOScale(Vector3.zero, disappearTime)).onComplete = () =>
            {
                _meshGO.SetActive(false);
            };
        }

        private void TryKillSequence()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }
        }
        
        public void DestroyEgg()
        {
            _destroyEggCoroutine = StartCoroutine(DestroyEggCoroutine());
        }

        private IEnumerator DestroyEggCoroutine()
        {
            transform.DOShakeRotation(3, 90, 10);
            yield return new WaitForSeconds(3);
            _eggPart.isKinematic = false;
            _eggPart.AddExplosionForce(_explosionForce, _explosionPoint.position, _explosionRadius);
            yield return _disapearDelayWait;
            for (int i = 0; i < _eggPartRigidbodies.Count; i++)
            {
                _eggPartColliders[i].isTrigger = true;
                _eggPartRigidbodies[i].isKinematic = false;
                Destroy(_eggPartColliders[i].gameObject, 2f); 
            }
        }
    }
}