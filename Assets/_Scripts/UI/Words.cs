using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace _Scripts.UI
{
    public class Words : MonoBehaviour
    {
        [SerializeField] private TMP_Text _word;
        [SerializeField] private List<string> _allWordsRu;
        [SerializeField] private List<string> _allWordsEn;
        [SerializeField] private float _fadingDuration;
        [SerializeField] private float _scalingDuration;
        [SerializeField] private Ease _scalingEase;
        [SerializeField] private Ease _fadingEase;
        [SerializeField] private float _startScale;
        [SerializeField] private bool _testEN = false;

        private Sequence _sequence;
        private List<string> _allWords = new List<string>();

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
        }

        private void GetData()
        {
            if (_testEN)
            {
                _allWords = _allWordsEn;
                return;
            }
            
            if (YandexGame.EnvironmentData.language == "ru" ||
                YandexGame.EnvironmentData.language == "be" ||
                YandexGame.EnvironmentData.language == "kk" ||
                YandexGame.EnvironmentData.language == "uk" ||
                YandexGame.EnvironmentData.language == "uz")
            {
                _allWords = _allWordsRu;
            }

            else
            {
                _allWords = _allWordsEn;
            }
        }

        private void OnEnable()
        {
            _word.text = _allWords[Random.Range(0, _allWords.Count)];
            _word.DOFade(1, 0);
            transform.localScale = new Vector3(_startScale, _startScale, _startScale);
            
            TryKilSequence();
            _sequence.Insert(0, transform.DOScale(1, _scalingDuration)).SetEase(_scalingEase);
            _sequence.Insert(0, _word.DOFade(0, _fadingDuration)).SetEase(_fadingEase).onComplete = () =>
            {
                gameObject.SetActive(false);
            };
        }

        private void TryKilSequence()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }

            _sequence = DOTween.Sequence();
        }

        public void SetPosition(Vector2 pos)
        {
            transform.localPosition = new Vector3(pos.x, pos.y, 0);
        }
    }
}