using Abilities;
using Player;
using UnityEngine;

public class AddTeleportAbility : AddAbilityBase
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter player))
        {
            if (player.TryGetComponent(out AbilitiesController abilitiesController))
            {
                if (player.TryGetComponent(out CharacterController characterController))
                {
                    var camera = player.GetComponentInChildren<Camera>();
                    abilitiesController.SetAbility(new TeleportAbility(abilityData, characterController, camera,
                        rayDistance, layerMask));
                    Destroy(gameObject);
                }
            }
        }
    }
}