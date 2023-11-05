using System.Collections;
using System.Collections.Generic;
using _Scripts.Sounds;
using _Scripts.UI;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.EggColors
{
    public class EggsChanger : ObjectChanger
    {
        [SerializeField] private List<Egg> _eggs;
        [SerializeField] private float _appearTime;
        [SerializeField] private float _disappearTime;
        
        private int _currentNumber = 0;
        private Egg _currentEgg;

        public Egg CurrentEgg
        {
            get => _currentEgg;
            set => _currentEgg = value;
        }

        public override void StartAction()
        {
            _currentNumber = Random.Range(0, _eggs.Count);
            for (int i = 0; i < _eggs.Count; i++)
            {
                if (i == _currentNumber)
                {
                    _eggs[i].Appear(_appearTime);
                    _currentEgg = _eggs[i];
                }
                else
                {
                    _eggs[i].transform.localScale = Vector3.zero;
                }
            }
        }

        public override void Next()
        {
            _eggs[_currentNumber].Disappear(_disappearTime);

            if (_currentNumber >= _eggs.Count - 1)
            {
                _currentNumber = 0;
            }
            else
            {
                _currentNumber++;
            }
        
            _eggs[_currentNumber].Appear(_appearTime);
            _currentEgg = _eggs[_currentNumber];
            SoundManager.Instance.ChangerSound();
        }
    
        public override void Previous()
        {
            _eggs[_currentNumber].Disappear(_disappearTime);

            if (_currentNumber <= 0)
            {
                _currentNumber = _eggs.Count - 1;
            }
            else
            {
                _currentNumber--;
            }
        
            _eggs[_currentNumber].Appear(_appearTime);
            _currentEgg = _eggs[_currentNumber];
            SoundManager.Instance.ChangerSound();
        }

        public void DestroyEgg()
        {
            _currentEgg.DestroyEgg();
        }
    }
}