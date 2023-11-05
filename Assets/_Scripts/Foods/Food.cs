using DG.Tweening;
using UnityEngine;

namespace _Scripts.Foods
{
    public class Food : MonoBehaviour
    {
        [SerializeField] private FoodName _name;
        [SerializeField] private Ease _ease = Ease.OutBounce;
        [SerializeField] private GameObject _meshGO;

        private Sequence _sequence;

        public FoodName FoodName => _name;

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
    }
}