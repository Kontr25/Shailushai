using _Scripts.Snail;
using UnityEngine;

namespace _Scripts.Character
{
    public class CharacterTrigger : MonoBehaviour
    {
        [SerializeField] private AudioSource _eatSound;
        [SerializeField] private ParticleSystem _eatEffect;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out SnailController snail))
            {
                snail.Get(transform);
                _eatSound.Play();
                _eatEffect.Play();
            }
        }
    }
}