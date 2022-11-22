using UnityEngine;

namespace BehaviourTree
{
    public class TreeExecutor : MonoBehaviour
    {
        public BTree tree;
        public bool ExecuteOnAwake = true;

        public Status status
        {
            get => tree.status;
        }
        
        
        public void ExecuteTree() => tree.Execute();

        void Start()
        {
            if (ExecuteOnAwake) tree.Execute();
        }

        void Update()
        {
            tree.Update();
        }
    }
}
