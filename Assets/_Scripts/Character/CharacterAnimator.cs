using UnityEngine;

namespace _Scripts.Character
{
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Dance1 = Animator.StringToHash("Dance1");
        private static readonly int Dance2 = Animator.StringToHash("Dance2");
        private static readonly int Dance3 = Animator.StringToHash("Dance3");
        private static readonly int Dance4 = Animator.StringToHash("Dance4");
        private static readonly int Dance5 = Animator.StringToHash("Dance5");
        private static readonly int MoveTrigger = Animator.StringToHash("Move");

        public void Move(float speed)
        {
            _animator.SetFloat(Speed, speed);
        }

        public void StartMove()
        {
            _animator.SetTrigger(MoveTrigger);
        }

        public void Dance()
        {
            int animationNumber = Random.Range(1, 6);

            switch (animationNumber)
            {
                case 1:
                    _animator.SetTrigger(Dance1);
                    break;
                case 2:
                    _animator.SetTrigger(Dance2);
                    break;
                case 3:
                    _animator.SetTrigger(Dance3);
                    break;
                case 4:
                    _animator.SetTrigger(Dance4);
                    break;
                case 5:
                    _animator.SetTrigger(Dance5);
                    break;
            }
        }
    }
}