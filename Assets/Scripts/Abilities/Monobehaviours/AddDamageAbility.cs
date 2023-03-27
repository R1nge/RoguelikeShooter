using Abilities;
using Player;
using UnityEngine;

public class AddDamageAbility : AddAbilityBase
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int damage;

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