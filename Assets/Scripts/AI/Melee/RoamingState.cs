using Player;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RoamingState : IState
    {
        private Waypoint[] _waypoints;
        private int _currentWaypointIndex;
        private readonly EnemyAI _enemyAI;
        private readonly NavMeshAgent _navMeshAgent;
        private readonly Transform _transform;

        public RoamingState(EnemyAI enemyAI, NavMeshAgent agent, Waypoint[] waypoints)
        {
            _enemyAI = enemyAI;
            _navMeshAgent = agent;
            _transform = agent.transform;
            _waypoints = waypoints;
        }


        public void Enter()
        {
        }

        public void Exit()
        {
            
        }

        public void Update()
        {
            Roaming();
        }

        private void Roaming()
        {
            var waypointPosition = _waypoints[_currentWaypointIndex].transform.position;
            if (Vector3.Distance(_transform.position,waypointPosition) <= .1f)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else
            {
                _navMeshAgent.SetDestination(waypointPosition);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                _enemyAI.SetState(new ChaseState(_enemyAI, _navMeshAgent, player.transform));
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                _enemyAI.SetState(new ChaseState(_enemyAI, _navMeshAgent, player.transform));
            }
        }

        public void OnTriggerExit(Collider other)
        {
        }
    }
}