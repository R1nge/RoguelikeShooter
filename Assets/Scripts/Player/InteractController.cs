using Interactable;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private InputActionAsset actions;

        private void OnEnable() => actions.Enable();

        private void OnDisable() => actions.Disable();

        private void Awake() => actions.FindActionMap("Player").FindAction("Interact").performed += Interact;

        private void Interact(InputAction.CallbackContext context) => Raycast();

        private void Raycast()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance, layerMask))
            {
                if (hit.transform.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact();
                }
            }
        }
    }
}