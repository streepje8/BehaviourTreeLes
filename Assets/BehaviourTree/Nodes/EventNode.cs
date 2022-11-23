using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class EventNode : Node
{
    public EventNode()
    {
        Name = "Event Node";
    }
    public override Status Start()
    {
        return Status.SUCCESS;
    }
}
