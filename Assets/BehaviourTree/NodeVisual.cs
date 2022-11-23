#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BehaviourTree;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class NodeVisual
{
    public Rect visualRect = new Rect(0,0,200,50);
    public string visualTitle = "Base Node";
    public GUIStyle visualStyle;
    public Node wrappedNode; 
    public Node[] childVisuals;
    private bool isDragged = false;
    private BehaviourTreeObject currentHome;

    public NodeVisual(BehaviourTreeObject b,Node toWrap,Rect visualRect, string visualTitle, GUIStyle visualStyle)
    {
        wrappedNode = toWrap;
        currentHome = b;
        childVisuals = Array.Empty<Node>();
        toWrap.Children.ForEach(x =>
        {
            childVisuals = childVisuals.Append(x).ToArray();
            b.lookupTable.Add(x, new NodeVisual(b,x,visualRect,x.Name,visualStyle));
        });
        this.visualRect = visualRect;
        this.visualTitle = visualTitle;
        this.visualStyle = visualStyle;
    }

    public virtual void Draw()
    {
        GUI.Box(visualRect, visualTitle, visualStyle);
        foreach(Node n in childVisuals)
            Handles.DrawBezier(
                visualRect.center,
                currentHome.lookupTable[n].visualRect.center,
                visualRect.center + Vector2.left * 50f,
                currentHome.lookupTable[n].visualRect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
    }

    
    
    public virtual bool ProcessEvents(Event e)
    {
switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (visualRect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                    }
                    else
                    {
                        GUI.changed = true;
                    }
                }
                if (e.button == 1)
                {
                    if (visualRect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu(e.mousePosition);
                        GUI.changed = true;
                    }
                }
                break;
            case EventType.MouseUp:
                isDragged = false;
                break;
 
            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }
        foreach(Node n in childVisuals) currentHome.lookupTable[n].ProcessEvents(e);
        return false;
    }

    public void Drag(Vector2 delta)
    {
        visualRect.position += delta;
    }
    
    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        TypeCache.TypeCollection avaibleTypes = TypeCache.GetTypesDerivedFrom<Node>(); //Assembly.GetAssembly(typeof(Node)).GetTypes().Where(myType =>myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Node))).ToList()
        foreach (Type t in avaibleTypes)
        {
            if (t != typeof(EntryNode))
            {
                genericMenu.AddItem(new GUIContent("Add " + t.Name), false, () => OnClickAddNode(mousePosition, t));
            }
        }
        genericMenu.ShowAsContext();
    }
        
    private void OnClickAddNode(Vector2 mousePosition, Type t)
    {
        Node newNode = (Node)Activator.CreateInstance(t);
        wrappedNode.Append(newNode);
        childVisuals = childVisuals.Append(newNode).ToArray();
        Rect newRect = visualRect;
        newRect.position += new Vector2(0, 100);
        currentHome.lookupTable.Add(newNode, new NodeVisual(currentHome,newNode,newRect,newNode.Name,visualStyle));
    }
}
#endif