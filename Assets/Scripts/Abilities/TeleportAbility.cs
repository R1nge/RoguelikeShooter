﻿using System.Collections;
using UnityEngine;

namespace Abilities
{
    public class TeleportAbility : AbilityBase
    {
        private readonly AbilityData _abilityData;
        private readonly CharacterController _characterController;
        private readonly Camera _playerCamera;
        private readonly float _rayDistance;
        private readonly LayerMask _layerMask;

        public TeleportAbility(AbilityData abilityData, CharacterController characterController, Camera playerCamera,
            float rayDistance,
            LayerMask layerMask) : base(abilityData)
        {
            _abilityData = abilityData;
            _characterController = characterController;
            _playerCamera = playerCamera;
            _rayDistance = rayDistance;
            _layerMask = layerMask;
        }

        public override IEnumerator Execute()
        {
            Teleport();
            yield return new WaitForSeconds(_abilityData.GetCoolDown());
        }

        private void Teleport()
        {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, _rayDistance, _layerMask))
            {
                _characterController.enabled = false;
                _characterController.transform.position = new Vector3(hit.point.x,
                    hit.point.y + _characterController.height / 2f, hit.point.z);
                _characterController.enabled = true;
            }
        }
    }
}