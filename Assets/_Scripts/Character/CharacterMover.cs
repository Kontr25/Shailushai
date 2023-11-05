using UnityEngine;

namespace _Scripts.Character
{
    public class CharacterMover : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _mainCollider;
        [SerializeField] private float _speed;
        [SerializeField] private float _rotationSpeed = 10;
        [SerializeField] private CharacterAnimator _characterAnimator;

        private bool _isCanMove = false;
        private FloatingJoystick _joystick;

        public bool IsCanMove
        {
            get => _isCanMove;
            set
            {
                if (_joystick != null)
                {
                    _joystick.gameObject.SetActive(value);
                }
                if (value)
                {
                    _mainCollider.enabled = true;
                    _rigidbody.isKinematic = false;
                }
                else
                {
                    _mainCollider.enabled = false;
                    _rigidbody.isKinematic = true;
                }
                _isCanMove = value;
            }
        }

        public void SetMoverParam(FloatingJoystick joystick)
        {
            _joystick = joystick;
        }

        private void FixedUpdate()
        {
            if (_isCanMove)
            {
                // Проверяем ввод с клавиатуры (WASD)
                float horizontalInput = -Input.GetAxis("Horizontal");
                float verticalInput = -Input.GetAxis("Vertical");

                // Если джойстик активен и клавиша не нажата, используем ввод с джойстика
                if (_joystick != null && (_joystick.Horizontal != 0 || _joystick.Vertical != 0))
                {
                    horizontalInput = -_joystick.Horizontal;
                    verticalInput = -_joystick.Vertical;
                }

                Vector3 inputDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
                _rigidbody.velocity = inputDirection * _speed;
                _characterAnimator.Move(_rigidbody.velocity.magnitude);

                if (inputDirection != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(inputDirection),
                        Time.deltaTime * _rotationSpeed);
                }
            }
        }
    }
}