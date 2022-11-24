namespace BehaviourTree
{
    public class EntryNode : Node
    {
        public EntryNode()
        {
            Name = "Entry Node";
        }
        
        public override Status Start()
        {
            if (Children.Length > 0)
            {
                return Children[0].ExecuteStep();
            }
            else
            {
                return Status.SUCCESS;
            }
        }

        public override Status Update()
        {
            return Children[0].ExecuteStep();
        }
    }
}