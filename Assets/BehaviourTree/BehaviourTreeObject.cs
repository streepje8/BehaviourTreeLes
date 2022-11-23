using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

namespace BehaviourTree
{
    [CreateAssetMenu(menuName = "Behaviour Tree", fileName = "New Behaviour Tree", order = 0)]
    public class BehaviourTreeObject : ScriptableObject
    {
        public BTree tree = new BTree();
        #if UNITY_EDITOR
        public Dictionary<Node, NodeVisual> lookupTable = new Dictionary<Node, NodeVisual>();
        public NodeVisual rootNode = null;
        [HideInInspector]public bool isInitialized = false;
#endif
    }
}
