using System;
using UnityEngine;

namespace BehaviourTree
{
    public class TreeExecutor : MonoBehaviour
    {
        public BehaviourTreeObject tree;
        public bool ExecuteOnAwake = true;

        public Status status
        {
            get => tree.tree.status;
        }
        
        
        public void ExecuteTree() => tree.tree.Execute();

        private void Awake()
        {
            if(!tree.tree.isInitialized) tree.tree.Initialize();
        }

        void Start()
        {
            if (ExecuteOnAwake) tree.tree.Execute();
        }

        void Update()
        {
            tree.tree.Update();
        }
    }
}
