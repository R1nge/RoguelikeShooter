using Player;
using UnityEngine;

namespace Abilities.Monobehaviours
{
    public class AddStunAbility : AddAbilityBase
    {
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layerMask;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out AbilitiesController abilitiesController))
            {
                var camera = abilitiesController.GetComponentInChildren<Camera>();
                abilitiesController.SetAbility(new StunAbility(abilityData, camera, rayDistance, layerMask));
                Destroy(gameObject);
            }
        }
    }
}