using Player;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class ChaseState : IState
    {
        private readonly EnemyAI _enemyAI;
        private readonly NavMeshAgent _navMeshAgent;
        private Transform _target;

        public ChaseState(EnemyAI enemyAI, NavMeshAgent navMeshAgent, Transform target)
        {
            _enemyAI = enemyAI;
            _navMeshAgent = navMeshAgent;
            _target = target;
        }

        public void Enter()
        {
            _navMeshAgent.SetDestination(_target.position);
        }

        public void Exit()
        {
            _target = null;
        }

        public void Update()
        {
            _navMeshAgent.SetDestination(_target.position);
        }

        public void OnTriggerEnter(Collider other)
        {
        }

        public void OnTriggerStay(Collider other)
        {
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                if (_target == player.transform)
                {
                    _enemyAI.SetState(_enemyAI.GetState<RoamingState>());
                }
            }
        }
    }
}