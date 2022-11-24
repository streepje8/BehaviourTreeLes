using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEditorInternal.VR;
using UnityEngine;

namespace BehaviourTree
{
    public class VisualTree
    {
        public Dictionary<string, NodeVisual> visualLookup = new Dictionary<string, NodeVisual>();
        public NodeVisual rootNode;
    }
    
    [CreateAssetMenu(menuName = "Behaviour Tree", fileName = "New Behaviour Tree", order = 0)]
    public class BehaviourTreeObject : ScriptableObject
    {
        public string treeJSON = "";
        [NonSerialized]public BTree tree = new BTree();
        
        #if UNITY_EDITOR
        public string visualJSON = "";
        [NonSerialized]public VisualTree vtree = new VisualTree();
        public bool isInitialized = false;
        #endif

        private void OnValidate()
        {
            tree.updateCallback = Serialize;
        }

        public void Serialize()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };
            treeJSON = JsonConvert.SerializeObject(tree,settings);
            #if UNITY_EDITOR
            visualJSON = JsonConvert.SerializeObject(vtree, settings);
            #endif
        }

        public void DeSerialize()
        {
            if (treeJSON.Length < 1)
            {
                Serialize();
            }
            tree = JsonConvert.DeserializeObject<BTree>(treeJSON);
            tree.updateCallback = Serialize;
            #if UNITY_EDITOR
            if (visualJSON.Length < 1)
            {
                Serialize();
            }
            //vtree = JsonConvert.DeserializeObject<VisualTree>(visualJSON);
            #endif
        }
    }
}
