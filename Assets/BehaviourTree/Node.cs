using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace BehaviourTree {

    [Serializable]
    public enum Status
    {
        SUCCESS = 0,
        FAILED = 1,
        RUNNING = 2
    }
    
    [Serializable]
    public enum State
    {
        Inactive = 0,
        Active = 1
    }

    [Serializable]
    public class Node
    {
        public string Name { get; protected set; } = "Base Node";
        public string guid = Guid.NewGuid().ToString();
        public Status Status { get; protected set; }
        public string parentGuid= "null";
        public string[] childrenGuids = Array.Empty<string>();
        [JsonIgnore]public BTree tree;
        [JsonIgnore]public Node Parent { get; protected set; }
        [JsonIgnore]public Node[] Children { get; protected set; } = Array.Empty<Node>();

        private bool isInitialized = false;

        public void InitNodeCreation(BTree tree)
        {
            if (!(GetType() == typeof(Node))) //No node base class
            {
                this.tree = tree;
                tree.nodeLookup.Add(guid, this);
            }
        }

        public void Deserialize(BTree tree)
        {
            if (!(GetType() == typeof(Node))) //No node base class
            {
                if (!tree.nodeLookup.ContainsKey(guid)) tree.nodeLookup.Add(guid, this);
            }
            if(!parentGuid.Equals("null",StringComparison.OrdinalIgnoreCase))
                Parent = tree.nodeLookup[parentGuid];
            foreach (string guid in childrenGuids)
            {
                Children = Children.Append(tree.nodeLookup[guid]).ToArray();
            }
            foreach (var child in Children)
            {
                child.Deserialize(tree);
            }
            this.tree = tree;
        }

        public virtual void Initialize() { }

        public virtual Status Start()
        {
            return Status.SUCCESS;
        }

        public virtual Status Update() => Status.SUCCESS;

        public virtual Status End() => Status.SUCCESS;

        private State state;
        public Status ExecuteStep()
        {
            if(!isInitialized) Initialize();
            switch(state)
            {
                case State.Inactive:
                    Status = Start();
                    if (Status == Status.RUNNING)
                    {
                        state = State.Active;
                    }
                    else
                    {
                        Status = End();
                    }
                    break;
                case State.Active:
                    Status = Update();
                    if (Status != Status.RUNNING)
                    {
                        state = State.Inactive;
                        Status = End();
                    }
                    break;
            }
            return Status;
        }

        public void Append(Node n)
        {
            n.Parent = this;
            n.parentGuid = guid;
            Children = Children.Append(n).ToArray();
            childrenGuids = childrenGuids.Append(n.guid).ToArray();
            tree.updateCallback?.Invoke();
        }
    }
}