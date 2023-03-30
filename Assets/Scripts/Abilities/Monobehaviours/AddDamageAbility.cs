using Abilities;
using Player;
using UnityEngine;

public class AddDamageAbility : AddAbilityBase
{
    [SerializeField] private int damage;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AbilitiesController abilitiesController))
        {
            var camera = abilitiesController.GetComponentInChildren<Camera>();
            abilitiesController.SetAbility(new DamageAbility(abilityData, camera, rayDistance, layerMask, damage));
            Destroy(gameObject);
        }
    }
}