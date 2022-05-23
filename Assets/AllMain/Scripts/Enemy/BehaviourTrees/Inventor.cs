using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventor : Node
{
    protected Node node;

    public Inventor (Node node)
    {
        this.node = node;
    }
    public override NodeState Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;
                break;
            case NodeState.SOCCESS:
                _nodeState = NodeState.FAILURE;
                break;
            case NodeState.FAILURE:
                _nodeState = NodeState.SOCCESS;
                break;
            default:
                break;
        }
        return _nodeState;
    }
}
