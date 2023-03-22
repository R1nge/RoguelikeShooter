using Player;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class RoamingState : State
    {
        private MeleeAI _meleeAI;
        private NavMeshAgent _navMeshAgent;
        private ChaseState _chaseState;

        private void Awake()
        {
            _meleeAI = GetComponent<MeleeAI>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _chaseState = GetComponent<ChaseState>();
        }

        public override void Enter()
        {
        }

        public override void MyUpdate()
        {
        }

        public override void MyOnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerCharacter player))
            {
                _chaseState.SetTarget(player.transform);
                _meleeAI.SetState(_chaseState);
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