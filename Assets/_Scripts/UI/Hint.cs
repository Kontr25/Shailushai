using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class Hint : MonoBehaviour
    {
        [SerializeField] private List<Image> _images;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Image _BG;
        [SerializeField] private float _fadingTime;

        private float _BGAlphaDefault;
        private Coroutine _activeCoroutine;
        private WaitForSeconds _fadingDelay;

        private void Start()
        {
            _fadingDelay = new WaitForSeconds(_fadingTime + 0.1f);
            _BGAlphaDefault = _BG.color.a;
            _activeCoroutine = StartCoroutine(ActiveCoroutine());
        }

        private void Update()
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetMouseButtonDown(0))
            {
                StopCoroutine(_activeCoroutine);
                Destroy(gameObject);
            }
        }

        private IEnumerator ActiveCoroutine()
        {
            while (true)
            {
                for (int i = 0; i < _images.Count; i++)
                {
                    _images[i].DOFade(0, _fadingTime);
                }

                _text.DOFade(0, _fadingTime);
                _BG.DOFade(0, _fadingTime);

                yield return _fadingDelay;
                
                for (int i = 0; i < _images.Count; i++)
                {
                    _images[i].DOFade(1, _fadingTime);
                }

                _text.DOFade(1, _fadingTime);
                _BG.DOFade(_BGAlphaDefault, _fadingTime);
                yield return _fadingDelay;
            }
        }
    }
}