using System.Collections.Generic;
using _Scripts.Sounds;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Mushrooms
{
    public class MushroomChanger : ObjectChanger
    {
        [SerializeField] private List<MushRoom> _mushRooms;
        [SerializeField] private float _appearTime;
        [SerializeField] private float _disappearTime;

        private int _currentNumber = 0;
        private MushRoom _currentMushRoom;

        public MushRoom CurrentMushRoom
        {
            get => _currentMushRoom;
            set => _currentMushRoom = value;
        }

        public override void StartAction()
        {
            _currentNumber = Random.Range(0, _mushRooms.Count);
            for (int i = 0; i < _mushRooms.Count; i++)
            {
                if (i == _currentNumber)
                {
                    _mushRooms[i].Appear(_appearTime);
                    _currentMushRoom = _mushRooms[i];
                }
                else
                {
                    _mushRooms[i].transform.localScale = Vector3.zero;
                }
            }
        }

        public override void Next()
        {
            _mushRooms[_currentNumber].Disappear(_disappearTime);

            if (_currentNumber >= _mushRooms.Count - 1)
            {
                _currentNumber = 0;
            }
            else
            {
                _currentNumber++;
            }
        
            _mushRooms[_currentNumber].Appear(_appearTime);
            _currentMushRoom = _mushRooms[_currentNumber];
            SoundManager.Instance.ChangerSound();
        }
    
        public override void Previous()
        {
            _mushRooms[_currentNumber].Disappear(_disappearTime);

            if (_currentNumber <= 0)
            {
                _currentNumber = _mushRooms.Count - 1;
            }
            else
            {
                _currentNumber--;
            }
        
            _mushRooms[_currentNumber].Appear(_appearTime);
            _currentMushRoom = _mushRooms[_currentNumber];
            SoundManager.Instance.ChangerSound();
        }
    }
}
