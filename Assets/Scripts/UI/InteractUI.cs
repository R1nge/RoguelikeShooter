using Interactable;
using TMPro;
using UnityEngine;

namespace UI
{
    public class InteractUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Camera playerCamera;

        private void Update() => Raycast();

        private void Raycast()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, rayDistance, layerMask))
            {
                if (hit.transform.TryGetComponent(out IInteractable _))
                {
                    interactText.SetText("Interact");
                    interactText.color = Color.white;
                }
            }
        }
    }
}