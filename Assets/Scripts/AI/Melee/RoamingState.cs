using Player;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RoamingState : State
    {
        [SerializeField] private Waypoint[] waypoints;
        private int _currentWaypointIndex;
        private EnemyAI _enemyAI;
        private NavMeshAgent _navMeshAgent;
        private ChaseState _chaseState;

        private void Awake()
        {
            _enemyAI = GetComponent<EnemyAI>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _chaseState = GetComponent<ChaseState>();
        }

        public override void Enter()
        {
        }

        public override void MyUpdate()
        {
            Roaming();
        }

        private void Roaming()
        {
            var waypointPosition = waypoints[_currentWaypointIndex].transform.position;
            if (Vector3.Distance(transform.position,waypointPosition) <= .1f)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
            }
            else
            {
                _navMeshAgent.SetDestination(waypointPosition);
            }
        }

        public override void MyOnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                _chaseState.SetTarget(player.transform);
                _enemyAI.SetState(_chaseState);
            }
        }

        public override void MyOnTriggerExit(Collider other)
        {
        }

        public override void Exit()
        {
        }
    }
}