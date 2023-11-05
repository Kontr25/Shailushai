using System.Collections.Generic;
using _Scripts.Sounds;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Foods
{
    public class FoodChanger : ObjectChanger
    {
        [SerializeField] private List<Food> _foods;
        [SerializeField] private float _appearTime;
        [SerializeField] private float _disappearTime;

        private int _currentNumber = 0;
        private Food _currentFood;

        public Food CurrentFood
        {
            get => _currentFood;
            set => _currentFood = value;
        }

        public override void StartAction()
        {
            _currentNumber = Random.Range(0, _foods.Count);
            for (int i = 0; i < _foods.Count; i++)
            {
                if (i == _currentNumber)
                {
                    _foods[i].Appear(_appearTime);
                    _currentFood = _foods[i];
                }
                else
                {
                    _foods[i].transform.localScale = Vector3.zero;
                }
            }
        }

        public override void Next()
        {
            _foods[_currentNumber].Disappear(_disappearTime);

            if (_currentNumber >= _foods.Count - 1)
            {
                _currentNumber = 0;
            }
            else
            {
                _currentNumber++;
            }
        
            _foods[_currentNumber].Appear(_appearTime);
            _currentFood = _foods[_currentNumber];
            SoundManager.Instance.ChangerSound();
        }
    
        public override void Previous()
        {
            _foods[_currentNumber].Disappear(_disappearTime);

            if (_currentNumber <= 0)
            {
                _currentNumber = _foods.Count - 1;
            }
            else
            {
                _currentNumber--;
            }
        
            _foods[_currentNumber].Appear(_appearTime);
            _currentFood = _foods[_currentNumber];
            SoundManager.Instance.ChangerSound();
        }
    }
}