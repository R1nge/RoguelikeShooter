using Player;
using UnityEngine;

namespace Abilities.Monobehaviours
{
    public class AddHealAbility : AddAbilityBase
    {
        [SerializeField] private int healAmount;
        [SerializeField] private float rayDistance;
        [SerializeField] private LayerMask layerMask;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                if (player.TryGetComponent(out AbilitiesController abilitiesController))
                {
                    var camera = player.GetComponentInChildren<Camera>();
                    abilitiesController.SetAbility(new HealAbility(abilityData, camera, rayDistance, layerMask, healAmount));
                    Destroy(gameObject);
                }
            }
        }
    }
}