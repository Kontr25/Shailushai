using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace _Scripts.UI
{
    public class OverlayImage : MonoBehaviour
    {
        [SerializeField] private Image _overlayImage;
        [SerializeField] private float _fadeInDuration;

        private bool _isFade;

        public float FadeInDuration => _fadeInDuration;
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

        public void GetData()
        {
            FadeImage();
        }

        public void FadeImage()
        {
            if (_isFade)
            {
                _isFade = false;
                _overlayImage.DOFade(.4f, _fadeInDuration).onComplete = () =>
                {
                    _overlayImage.raycastTarget = true;
                };
            }
            else
            {
                _isFade = true;
                _overlayImage.DOFade(0f, _fadeInDuration);
                _overlayImage.raycastTarget = false;
            }
        }
    }
}