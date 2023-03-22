using System;
using UnityEngine;

namespace AI
{
    public abstract class State : MonoBehaviour
    {
        public abstract void MyUpdate();
        public abstract void Enter();
        public abstract void Exit();
        public abstract void MyOnTriggerEnter(Collider other);
        public abstract void MyOnTriggerExit(Collider other);
    }
}