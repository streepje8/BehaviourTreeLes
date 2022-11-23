using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace BehaviourTree
{
    public class TreeEditorWindow : EditorWindow
    {
        private BehaviourTreeObject selectedTree = null;
        public GUIStyle nodeStyle;

        [MenuItem("Window/Node Based Editor")]
        private static void OpenWindow()
        {
            TreeEditorWindow window = GetWindow<TreeEditorWindow>();
            window.titleContent = new GUIContent("Behaviour Tree Editor");
        }

        private void OnEnable()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnGUI()
        {
            

            if (selectedTree != null)
            {
                DrawNodes();

                ProcessEvents(Event.current);

                if (GUI.changed) Repaint();
            }
        }

        private void OnSelectionChange()
        {
            foreach (Object obj in Selection.objects)
            {
                if (obj.GetType() == typeof(BehaviourTreeObject))
                {
                    selectedTree = (BehaviourTreeObject)obj;
                    if (!selectedTree.isInitialized)
                    {
                        selectedTree.rootNode = new NodeVisual(selectedTree, selectedTree.tree.RootNode,
                            new Rect(0, 0, 200, 50),
                            selectedTree.tree.RootNode.Name, nodeStyle);
                        selectedTree.isInitialized = true;
                        EditorUtility.SetDirty(selectedTree);
                    }
                    Repaint();
                }
            }
        }

        private void DrawNodes()
        {
            if (!selectedTree.isInitialized)
            {
                selectedTree.rootNode = new NodeVisual(selectedTree, selectedTree.tree.RootNode,
                    new Rect(0, 0, 200, 50),
                    selectedTree.tree.RootNode.Name, nodeStyle);
                selectedTree.isInitialized = true;
                EditorUtility.SetDirty(selectedTree);
            }

            DrawNode(selectedTree.rootNode);
        }

        public void DrawNode(NodeVisual n)
        {
            n.Draw();
            foreach(Node j in n.childVisuals) DrawNode(selectedTree.lookupTable[j]);
        }

        private void ProcessEvents(Event e)
        {
            selectedTree.rootNode.ProcessEvents(e);
        } 

    }
}