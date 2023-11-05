using DG.Tweening;
using UnityEngine;

namespace _Scripts.Snail
{
    public class SnailController : MonoBehaviour
    {
        [SerializeField] private float _jumpPower;
        [SerializeField] private float _fallTime;
        [SerializeField] private GameObject _mesh;
        
        private Vector3 _defaultScale;
        private Vector3 _smallScale = new Vector3(0.01f,0.01f,0.01f);
        private bool _isRised = false;
        
        public void Get(Transform targetTransform)
        {
            if(_isRised) return;
            _isRised = true;
            SnailCounter.Instance.RemoveSnailFromList(this);
            transform.SetParent(targetTransform);
            transform.DORotateQuaternion(Random.rotation, _fallTime);
            transform.DOLocalJump(Vector3.zero, _jumpPower, 1, _fallTime);
            Disappear();
        }
        
        private void Disappear()
        {
            transform.DOScale(_smallScale, 1f).onComplete = () =>
            {
                _mesh.SetActive(false);
            };
        }
    }
}