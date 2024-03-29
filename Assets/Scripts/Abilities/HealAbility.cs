﻿using System.Collections;
using Damageable;
using UnityEngine;

namespace Abilities
{
    public class HealAbility : AbilityBase
    {
        private readonly AbilityData _abilityData;
        private readonly Camera _playerCamera;
        private readonly float _rayDistance;
        private readonly LayerMask _layerMask;
        private readonly int _heal;

        public HealAbility(AbilityData abilityData, Camera playerCamera, float rayDistance, LayerMask layerMask,
            int heal) : base(abilityData)
        {
            _abilityData = abilityData;
            _playerCamera = playerCamera;
            _rayDistance = rayDistance;
            _layerMask = layerMask;
            _heal = heal;
        }

        public override IEnumerator Execute()
        {
            Heal();
            yield return new WaitForSeconds(_abilityData.GetCoolDown());
        }

        private void Heal()
        {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, _rayDistance, _layerMask))
            {
                if (hit.transform.TryGetComponent(out Health health))
                {
                    health.Heal(_heal);
                }
            }
        }
    }
}