using Player;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class ChaseState : State
    {
        private EnemyAI _enemyAI;
        private NavMeshAgent _navMeshAgent;
        private Transform _target;
        private RoamingState _roamingState;

        private void Awake()
        {
            _enemyAI = GetComponent<EnemyAI>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _roamingState = GetComponent<RoamingState>();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public override void Enter()
        {
        }

        public override void MyUpdate()
        {
            _navMeshAgent.SetDestination(_target.position);
        }

        public override void MyOnTriggerEnter(Collider other)
        {
        }

        public override void MyOnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                if (_target == player.transform)
                {
                    _enemyAI.SetState(_roamingState);
                }
            }
        }

        public override void Exit()
        {
            _target = null;
        }
    }
}