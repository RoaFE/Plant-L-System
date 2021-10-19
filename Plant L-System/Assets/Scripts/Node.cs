using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    Plant plant;
    Node parentNode;
    List<Node> childNodes;
    public Vector3 position = Vector3.zero;
    //Stem stem;

    public void InitNode(Plant inPlant, Node inPrevNode)
    {
        childNodes = new List<Node>();
        plant = inPlant;
        if(inPrevNode != null)
        {
            parentNode = inPrevNode;
        }
        
    }

    // Start is called before the first frame update
    public bool Extend(int nodeDepth, Vector3 dir, float dist)
    {
        if(childNodes.Count > 0)
        {
            Debug.Log("Node system already exists");
            return false;
        }
        if(nodeDepth < 1)
        {
            return true;
        }
        bool result = true;
        if(plant.nodeDepth - nodeDepth >= plant.splitDepth && ((plant.nodeDepth - nodeDepth) - plant.splitDepth) % plant.splitFrequency == 0)
        {
            GameObject newObj = new GameObject(string.Format("Node {0}",plant.nodeDepth - nodeDepth));
            for(int i = 0; i < plant.splitCount; i++)
            {
                GameObject obj = Instantiate(newObj,dir*dist,Quaternion.identity,this.transform);
                obj.transform.position = transform.position + (dir.magnitude * dir);
                childNodes.Add(obj.AddComponent<Node>());
                childNodes[i].InitNode(plant,this);
                result &= childNodes[i].Extend(nodeDepth - 1,dir,dist);
            }
            DestroyImmediate(newObj);
        }
        else{
            GameObject newObj = new GameObject(string.Format("Node {0}",plant.nodeDepth - nodeDepth));
            GameObject obj = Instantiate(newObj,dir*dist,Quaternion.identity,this.transform);
            obj.transform.position = transform.position + (dir.magnitude * dir);
            DestroyImmediate(newObj);
            childNodes.Add(obj.AddComponent<Node>());
            childNodes[0].InitNode(plant,this);
            result = childNodes[0].Extend(nodeDepth - 1,dir,dist);
        }
        return result;
    } 

    public void Clear()
    {
        if(childNodes.Count > 0)
        {
            foreach(Node node in childNodes)
            {
                node.Clear();
                DestroyImmediate(node.gameObject);
            }
        }
        childNodes.Clear();
    }

    public List<Vector3> DebugDrawNode()
    {
        List<Vector3> nodes = new List<Vector3>();
        if(childNodes.Count > 0)
        {
            foreach(Node node in childNodes)
            {
                nodes.AddRange(node.DebugDrawNode());
            }
        }
        nodes.Add(transform.position);
        return nodes;
    }
}
