using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float lookSpeed = 2f;
        [SerializeField] private float lookXLimit = 90f;
        [SerializeField] private InputActionAsset actions;
        private InputAction _lookAction;
        private bool _canMove = true;
        private float _rotationX;

        private void OnEnable() => actions.Enable();

        private void OnDisable() => actions.Disable();

        private void Awake() => _lookAction = actions.FindActionMap("Player").FindAction("Look");

        private void Update()
        {
            if (!_canMove) return;
            Rotate();
        }

        private void Rotate()
        {
            if (_canMove)
            {
                _rotationX += -_lookAction.ReadValue<Vector2>().y * lookSpeed;
                _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, _lookAction.ReadValue<Vector2>().x * lookSpeed, 0);
            }
        }
    }
}