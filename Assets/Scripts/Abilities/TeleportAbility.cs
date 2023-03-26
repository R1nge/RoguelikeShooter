using System.Collections;
using Player;
using UnityEngine;

namespace Abilities
{
    public class TeleportAbility : AbilityBase
    {
        private readonly AbilityData _abilityData;
        private readonly Transform _player;
        private readonly Camera _playerCamera;
        private readonly float _rayDistance;
        private readonly LayerMask _layerMask;

        public TeleportAbility(AbilityData abilityData, Transform player, Camera playerCamera, float rayDistance,
            LayerMask layerMask) : base(abilityData)
        {
            _abilityData = abilityData;
            _player = player;
            _playerCamera = playerCamera;
            _rayDistance = rayDistance;
            _layerMask = layerMask;
        }

        public override IEnumerator Execute()
        { 
            Teleport();
            yield return new WaitForSeconds(_abilityData.coolDown);
        }

        private void Teleport()
        {
            Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
            if (Physics.Raycast(ray, out var hit, _rayDistance, _layerMask))
            {
                _player.transform.position = hit.point;
            }
        }
    }
}