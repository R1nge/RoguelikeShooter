using Interactable;
using UnityEngine;

namespace Player
{
    public class InteractController : MonoBehaviour
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Camera playerCamera;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Raycast();
            }
        }

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