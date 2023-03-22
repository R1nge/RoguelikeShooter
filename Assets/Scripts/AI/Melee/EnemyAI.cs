using UnityEngine;

namespace AI
{
    public class EnemyAI : MonoBehaviour
    {
        private State[] _states;
        private State _currentState;

        private void Awake() => _states = GetComponentsInChildren<State>();

        private void Start() => SetState(_states[0]);

        public void SetState(State newState)
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            _currentState = newState;
            _currentState.Enter();
        }

        private void Update()
        {
            _currentState.MyUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            _currentState.MyOnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            _currentState.MyOnTriggerExit(other);
        }
    }
}