using System.Collections.Generic;

namespace BehaviourTree {

    [System.Serializable]
    public enum Status
    {
        SUCCESS = 0,
        FAILED = 1,
        RUNNING = 2
    }
    
    [System.Serializable]
    public enum State
    {
        Inactive = 0,
        Active = 1
    }

    [System.Serializable]
    public abstract class Node
    {
        public string Name { get; protected set; } = "Base Node";
        public Status Status { get; protected set; }
        public Node Parent { get; protected set; }
        public List<Node> Children { get; protected set; } = new List<Node>();

        private bool isInitialized = false;

        public virtual void Initialize() { }
        public abstract Status Start();
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
            Children.Add(n);
        }
    }
}