using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor {

    private void OnSceneGUI() {
        Node  curNode = (Node)target;
        List<Node> nodes = curNode.DebugGetNodes();
        List<Node> parentnodes = curNode.GetParentNodes();
        foreach(Node node in nodes)
        {
            List<Node> childNodes = node.GetChildNodes();
            foreach(Node childnode in childNodes)
            {
                Handles.color = Color.green;
                Handles.DrawLine(node.transform.position,childnode.transform.position);
            }
            Handles.color = Color.red;
            Handles.DrawWireCube(node.transform.position,Vector3.one * 0.05f);
        }
        Node prevNode = curNode;
        foreach(Node node in parentnodes)
        {
            Handles.color = Color.blue;
            Handles.DrawLine(node.transform.position,prevNode.transform.position);
            prevNode = node;
        }
        Handles.color = Color.red;
        Handles.DrawWireArc(curNode.transform.position,Vector3.up,Vector3.forward,360,0.5f);
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
    }
}
