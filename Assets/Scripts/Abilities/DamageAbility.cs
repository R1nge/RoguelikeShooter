using System.Collections;
using Damageable;
using UnityEngine;

namespace Abilities
{
    public class DamageAbility : AbilityBase
    {
        private readonly AbilityData _abilityData;
        private readonly Camera _playerCamera;
        private readonly float _rayDistance;
        private readonly LayerMask _layerMask;
        private readonly int _damage;

        public DamageAbility(AbilityData abilityData, Camera playerCamera, float rayDistance, LayerMask layerMask,
            int damage) :
            base(abilityData)
        {
            _abilityData = abilityData;
            _playerCamera = playerCamera;
            _rayDistance = rayDistance;
            _layerMask = layerMask;
            _damage = damage;
        }

        public override IEnumerator Execute()
        {
            Damage();
            yield return new WaitForSeconds(_abilityData.GetCoolDown());
        }

        private void Damage()
        {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, _rayDistance, _layerMask))
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_damage);
                }
            }
        }
    }
}