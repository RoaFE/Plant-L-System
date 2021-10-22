using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Plant))]
public class PlantEditor : Editor 
{
    private void OnSceneGUI() {
        Plant  plant = (Plant)target;
        List<Node> nodes = plant.GetRootNode().DebugGetNodes();
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
        Handles.color = Color.red;
        Handles.DrawWireArc(plant.transform.position,Vector3.up,Vector3.forward,360,0.5f);
    }
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Plant  plant = (Plant)target;
        EditorGUILayout.Separator();   
        if (GUILayout.Button("Generate"))
                plant.Generate();

        if (GUILayout.Button("Clear"))
                plant.Clear();
    }
}