using UnityEngine;

namespace AI
{
    public interface IState
    {
        void Enter();
        void Exit();
        void Update();
        void OnTriggerEnter(Collider other);
        void OnTriggerStay(Collider other);
        void OnTriggerExit(Collider other);
    }
}