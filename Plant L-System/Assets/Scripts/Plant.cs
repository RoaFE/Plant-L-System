using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]
    Node rootNode;
    public int nodeDepth;
    public float nodeDistance;
    public int splitDepth, splitFrequency, splitCount;
    public void Generate()
    {
        if(rootNode == null)
        {
            rootNode = gameObject.AddComponent<Node>();            
            rootNode.InitNode(this,null);
        }
        rootNode.Extend(nodeDepth,Vector3.up,nodeDistance);        
    }
    
    public void Clear()
    {
        if(rootNode != null)
        {
            rootNode.Clear();
        }
    }

    public List<Vector3> nodePosition()
    {
        List<Vector3> nodes = new List<Vector3>();
        if(rootNode != null)
        {
            nodes = rootNode.DebugDrawNode();
        }
        return nodes;
    }

    private void OnDrawGizmos() {
        if(rootNode != null)
        {
            List<Vector3> nodes = rootNode.DebugDrawNode();
            Gizmos.color = Color.red;
            foreach(Vector3 position in nodes)
            {
                Gizmos.DrawSphere(position,1);
            }
        }

        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position,new Vector3(1,0.5f,1));
    }
}
