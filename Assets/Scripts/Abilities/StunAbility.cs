using System.Collections;
using AI;
using Damageable;
using UnityEngine;

namespace Abilities
{
    public class StunAbility : AbilityBase
    {
        private readonly AbilityData _abilityData;
        private readonly Camera _playerCamera;
        private readonly float _rayDistance;
        private readonly LayerMask _layerMask;

        public StunAbility(AbilityData abilityData, Camera playerCamera, float rayDistance, LayerMask layerMask) :
            base(abilityData)
        {
            _abilityData = abilityData;
            _playerCamera = playerCamera;
            _rayDistance = rayDistance;
            _layerMask = layerMask;
        }

        public override IEnumerator Execute()
        {
            Stun();
            yield return new WaitForSeconds(_abilityData.GetCoolDown());
        }

        private void Stun()
        {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, _rayDistance, _layerMask))
            {
                if (hit.transform.TryGetComponent(out EnemyAI enemyAI))
                {
                    enemyAI.SetState(enemyAI.GetState<StunState>());
                }
            }
        }
    }
}