using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Plant))]
public class PlantEditor : Editor 
{
    private void OnSceneGUI() {
        Plant  plant = (Plant)target;
        List<Vector3> nodes = plant.nodePosition();
        for(int i = 1; i < nodes.Count; i++)
        {
            Handles.color = Color.green;
            Handles.DrawLine(nodes[i-1],nodes[i]);
        }
        Handles.DrawWireArc(plant.transform.position,Vector3.up,Vector3.forward,360,2);
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
