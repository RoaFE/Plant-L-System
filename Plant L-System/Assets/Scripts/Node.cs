using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Plant plant;
    Node prevNode;
    Node nextNode;
    public Vector3 position = Vector3.zero;
    //Stem stem;

    public void InitNode(Plant inPlant, Node inPrevNode)
    {
        plant = inPlant;
        if(inPrevNode != null)
        {
            prevNode = inPrevNode;
        }
        
    }

    // Start is called before the first frame update
    public bool Extend(int nodeDepth, Vector3 dir, float dist)
    {
        if(nodeDepth < 1)
        {
            return false;
        }
        GameObject newObj = new GameObject(string.Format("Node {0}",nodeDepth));
        GameObject obj = Instantiate(newObj,dir*dist,Quaternion.identity,this.transform);
        obj.transform.position = transform.position + (dir.magnitude * dir);
        DestroyImmediate(newObj);
        nextNode = obj.AddComponent<Node>();
        nextNode.InitNode(plant,this);
        return(nextNode.Extend(nodeDepth - 1,dir,dist));
    } 

    public void Clear()
    {
        if(nextNode != null)
        {
            nextNode.Clear();
            DestroyImmediate(nextNode.gameObject);
        }
        nextNode = null;
    }

    public List<Vector3> DebugDrawNode()
    {
        List<Vector3> nodes = new List<Vector3>();
        if(nextNode != null)
        {
           nodes = nextNode.DebugDrawNode();
        }
        nodes.Add(transform.position);
        return nodes;
    }
}
