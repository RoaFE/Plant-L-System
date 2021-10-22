using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor {

    private void OnSceneGUI() {
        Node  curNode = (Node)target;
        List<Node> nodes = curNode.DebugGetNodes();
        foreach(Node node in nodes)
        {
            List<Node> childNodes = node.GetChildNodes();
            foreach(Node childnode in childNodes)
            {
                Handles.color = Color.green;
                Handles.DrawLine(node.transform.position,childnode.transform.position);
            }
        }
        Handles.color = Color.red;
        Handles.DrawWireArc(curNode.transform.position,Vector3.up,Vector3.forward,360,0.5f);
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
    }
}