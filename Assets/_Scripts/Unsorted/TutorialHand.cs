using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Unsorted
{
    public class TutorialHand : MonoBehaviour
    {
        [SerializeField] private Image _handImage;
        [SerializeField] private float _scalingDuration;
        [SerializeField] private float _clickScale;
        [SerializeField] private float _fadingDuration;

        private Coroutine _pulsateCoroutine;
        private WaitForSeconds _scalingWait;
        private WaitForSeconds _fadingWait;

        private void Start()
        {
            _scalingWait = new WaitForSeconds(_scalingDuration);
            _fadingWait = new WaitForSeconds(_fadingDuration);
        }

        public void Pulsate()
        {
            StopPulsate();
            _pulsateCoroutine = StartCoroutine(PulsateCoroutine());
        }

        public void StopPulsate()
        {
            if (_pulsateCoroutine != null)
            {
                StopCoroutine(_pulsateCoroutine);
                _pulsateCoroutine = null;
            }
        }

        private IEnumerator PulsateCoroutine()
        {
            while (true)
            {
                if (_fadingDuration > 0)
                {
                    _handImage.DOFade(1, _fadingDuration);
                    yield return _fadingWait;
                }
                
                _handImage.transform.DOScale(_clickScale, _scalingDuration);
                yield return _scalingWait;
                
                _handImage.transform.DOScale(1f, _scalingDuration);
                yield return _scalingWait;
                
                if (_fadingDuration > 0)
                {
                     _handImage.DOFade(0, _fadingDuration);
                    yield return _fadingWait;
                }
            }
        }

        public float ChangingPositionDelay()
        {
            return _fadingDuration * 2 + _scalingDuration * 2;
        }
    }
}