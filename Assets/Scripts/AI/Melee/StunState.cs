using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class StunState : IState
    {
        private readonly float _duration;
        private readonly EnemyAI _enemyAI;
        private readonly NavMeshAgent _agent;
        private float _currentDuration;

        public StunState(float duration, EnemyAI enemyAI, NavMeshAgent agent)
        {
            _duration = duration;
            _enemyAI = enemyAI;
            _agent = agent;
        }

        public void Enter()
        {
            _currentDuration = _duration;
            _agent.isStopped = true;
        }

        public void Exit()
        {
        }

        public void Update()
        {
            _currentDuration -= Time.deltaTime;
            if (_currentDuration <= 0)
            {
                _agent.isStopped = false;
                _enemyAI.SetState(_enemyAI.GetState<RoamingState>());
            }
        }

        public void OnTriggerEnter(Collider other)
        {
        }

        public void OnTriggerExit(Collider other)
        {
        }

        public void OnTriggerStay(Collider other)
        {
        }
    }
}