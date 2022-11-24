using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BehaviourTree
{
    public class BTree
    {
        public Dictionary<string,Node> nodeLookup = new Dictionary<string,Node>();
        [JsonIgnore]public Action updateCallback;
        public Node RootNode { get; private set; }

        protected internal Status status { get; private set; }

        [field: NonSerialized]public bool isInitialized = false;

        public BTree()
        {
            RootNode = new EntryNode();
            RootNode.InitNodeCreation(this);
            updateCallback?.Invoke();
        }

        public void Initialize()
        {
            RootNode.Deserialize(this);
            isInitialized = true;
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