using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private Waypoint[] waypoints;
        private Dictionary<Type, IState> _states;
        private IState _currentState;
        private NavMeshAgent _agent;

        private void Awake()
        {
            Init();
        }

        private void Start()
        {
            _currentState = GetState<RoamingState>();
        }

        private void Init()
        {
            _agent = GetComponent<NavMeshAgent>();

            _states = new Dictionary<Type, IState>
            {
                [typeof(RoamingState)] = new RoamingState(this, _agent, waypoints),
                [typeof(StunState)] = new StunState(2f, this, _agent)
            };
        }

        public void SetState(IState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState?.Enter();
        }

        public IState GetState<T>() where T : IState
        {
            var type = typeof(T);
            return _states[type];
        }

        private void Update()
        {
            _currentState?.Update();
        }

        private void OnTriggerEnter(Collider other)
        {
            _currentState?.OnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _currentState?.OnTriggerExit(other);
        }

        private void OnTriggerStay(Collider other)
        {
            _currentState?.OnTriggerStay(other);
        }
    }
}