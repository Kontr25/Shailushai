using System;
using UnityEngine;

namespace _Scripts.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        
        [SerializeField] private AudioSource _victory;
        [SerializeField] private AudioSource _BG;
        [SerializeField] private AudioSource _changer;

        [SerializeField] private AudioClip _afterVictoryClip;
        [SerializeField] private AudioClip _defaultClip;

        private float _defaultVolumeBg;
        public AudioSource Bg
        {
            get => _BG;
            set => _BG = value;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                transform.SetParent(null);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _defaultVolumeBg = _BG.volume;
        }

        public void Victory()
        {
            _victory.Play();
        }
        
        public void AfterVictory()
        {
            _BG.Stop();
            _BG.clip = _afterVictoryClip;
            _BG.Play();
        }
        
        public void DefaultBG()
        {
            _BG.Stop();
            _BG.clip = _defaultClip;
            _BG.Play();
        }

        public void VolumeDown()
        {
            _BG.volume = 0.1f;
        }
        
        public void VolumeUp()
        {
            _BG.volume = _defaultVolumeBg;
        }

        public void ChangerSound()
        {
            _changer.Play();
        }
    }
}