using System;
using Abilities;
using Player;
using UnityEngine;

public class AddTeleportAbility : AddAbilityBase
{
    [SerializeField] private float teleportDistance;
    [SerializeField] private LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacter player))
        {
            if (player.TryGetComponent(out AbilitiesController abilitiesController))
            {
                var camera = player.GetComponentInChildren<Camera>();
                //SO for ability image 
                abilitiesController.SetAbility(new TeleportAbility(abilityData, player.transform, camera, teleportDistance, layerMask));

                Destroy(gameObject);
            }
        }
    }
}