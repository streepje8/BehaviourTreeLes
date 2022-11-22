using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "Behaviour Tree", fileName = "New Behaviour Tree", order = 0)]
    public class BTree : ScriptableObject
    {
        public Node RootNode { get; private set; }

        protected internal Status status { get; private set; }

        public BTree()
        {
            RootNode = new EntryNode();
        }

        public void Execute()
        {
            status = Status.RUNNING;
        }
        
        public void Update()
        {
            if (status == Status.RUNNING)
            {
                status = RootNode.ExecuteStep();
            }
        }
    }
}