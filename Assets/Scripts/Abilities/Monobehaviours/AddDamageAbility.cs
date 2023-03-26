using Abilities;
using Player;
using UnityEngine;

public class AddDamageAbility : AddAbilityBase
{
    [SerializeField] private float damageDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out AbilitiesController abilitiesController))
        {
            var camera = abilitiesController.GetComponentInChildren<Camera>();
            //SO for ability image 
            abilitiesController.SetAbility(new DamageAbility(abilityData, camera, damageDistance, layerMask, damage));
            Destroy(gameObject);
        }
    }
}