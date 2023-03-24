using UnityEngine;
using UnityEngine.InputSystem;

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
        [SerializeField] private InputActionAsset actions;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private CharacterController _characterController;
        private Vector3 _moveDirection = Vector3.zero;
        private bool _canMove = true;
        private int _jumpCount;
        private float _movementDirectionY;

        private void OnEnable() => actions.Enable();

        private void OnDisable() => actions.Disable();

        private void Awake()
        {
            _moveAction = actions.FindActionMap("Player").FindAction("Move");
            _jumpAction = actions.FindActionMap("Player").FindAction("Jump");
            _sprintAction = actions.FindActionMap("Player").FindAction("Sprint");

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
            bool isRunning = _sprintAction.IsPressed();
            float curSpeedX =
                _canMove ? (isRunning ? runningSpeed : walkingSpeed) * _moveAction.ReadValue<Vector2>().y : 0;
            float curSpeedY =
                _canMove ? (isRunning ? runningSpeed : walkingSpeed) * _moveAction.ReadValue<Vector2>().x : 0;
            _movementDirectionY = _moveDirection.y;
            _moveDirection = forward * curSpeedX + right * curSpeedY;

            if (_characterController.isGrounded)
            {
                _jumpCount = 0;
            }
            else
            {
                _movementDirectionY -= gravity;
            }

            if (_jumpAction.WasPressedThisFrame())
            {
                if (_characterController.isGrounded)
                {
                    _movementDirectionY = jumpSpeed;
                }
                else
                {
                    if (_jumpCount < maxJumps - 1)
                    {
                        _jumpCount++;
                        _movementDirectionY = jumpSpeed;
                    }
                }
            }

            _moveDirection.y = _movementDirectionY;
            _characterController.Move(_moveDirection * Time.deltaTime);
        }
    }
}