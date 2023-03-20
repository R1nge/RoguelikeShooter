using Player;
using UnityEngine;

namespace Spawners
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter player;
        [SerializeField] private Transform spawnPoint;

        private void Awake() => SpawnPlayer();

        private void SpawnPlayer() => Instantiate(player, spawnPoint.position, Quaternion.identity);
    }
}