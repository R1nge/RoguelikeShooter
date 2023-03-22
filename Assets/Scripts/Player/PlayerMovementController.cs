using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float walkingSpeed = 7.5f;
        [SerializeField] private float runningSpeed = 11.5f;
        [SerializeField] private float jumpSpeed = 8f;
        [SerializeField] private float gravity = 20f;
        [SerializeField] private int maxJumps;
        private CharacterController _characterController;
        private Vector3 _moveDirection = Vector3.zero;
        private bool _canMove = true;
        private int _jumpCount;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (!_canMove) return;
            Move();
        }

        private void Move()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = _canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
            float curSpeedY = _canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = _moveDirection.y;
            _moveDirection = forward * curSpeedX + right * curSpeedY;

            if (_characterController.isGrounded)
            {
                _jumpCount = 0;
            }
            else
            {
                movementDirectionY -= gravity;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (_characterController.isGrounded)
                {
                    movementDirectionY = jumpSpeed;
                }
                else
                {
                    if (_jumpCount < maxJumps - 1)
                    {
                        _jumpCount++;
                        movementDirectionY = jumpSpeed;
                    }
                }
            }

            _moveDirection.y = movementDirectionY;
            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}