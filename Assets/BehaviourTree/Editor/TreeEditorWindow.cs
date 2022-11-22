using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TreeEditorWindow : EditorWindow
{
    [MenuItem("Window/Node Based Editor")]
    private static void OpenWindow()
    {
        TreeEditorWindow window = GetWindow<TreeEditorWindow>();
        window.titleContent = new GUIContent("Behaviour Tree Editor");
    }
 
    private void OnGUI()
    {
        DrawNodes();
 
        ProcessEvents(Event.current);
 
        if (GUI.changed) Repaint();
    }
 
    private void DrawNodes()
    {
    }
 
    private void ProcessEvents(Event e)
    {
    }
}
