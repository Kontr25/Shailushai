using _Scripts.Character;
using _Scripts.Snail;
using UnityEngine;

namespace _Scripts.Unsorted
{
    public class RewardSetter : MonoBehaviour
    {
        [SerializeField] private Clicker _clicker;
        [SerializeField] private SnailCounter _snailCounter;

        private bool _isClickerReward = false;
        private bool _isCounterReward = false;

        public bool IsClickerReward
        {
            get => _isClickerReward;
            set
            {
                _isCounterReward = !value;
                _isClickerReward = value;
            }
        }

        public bool IsCounterReward
        {
            get => _isCounterReward;
            set
            {
                _isClickerReward = !value;
                _isCounterReward = value;
            }
        }

        public void SetReward()
        {
            if (_isClickerReward)
            {
                _clicker.AddBoost();
            }else if (_isCounterReward)
            {
                _snailCounter.AddBonusSnailCount();
            }
        }
    }
}