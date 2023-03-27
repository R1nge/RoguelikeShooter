using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class StunState : State
    {
        [SerializeField] private float duration;
        private EnemyAI _enemyAI;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _enemyAI = GetComponent<EnemyAI>();
            _agent = GetComponent<NavMeshAgent>();
        }

        public override void MyUpdate()
        {
        }

        public override void Enter()
        {
            _agent.SetDestination(transform.position);
            Invoke(nameof(ReturnState), duration);
        }

        private void ReturnState()
        {
            _enemyAI.SetState(GetComponent<RoamingState>());
        }

        public override void Exit()
        {
        }

        public override void MyOnTriggerEnter(Collider other)
        {
        }

        public override void MyOnTriggerExit(Collider other)
        {
        }
    }
}