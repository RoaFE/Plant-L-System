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
        Vector3 randomDir = Random.insideUnitSphere.normalized;
        dir = Vector3.Lerp(dir,randomDir,plant.randomisationPercentage);
        
        bool result = true;
        if(plant.nodeDepth - nodeDepth >= plant.splitDepth && ((plant.nodeDepth - nodeDepth) - plant.splitDepth) % plant.splitFrequency == 0)
        {
            Plane plane = new Plane(transform.position,dir,transform.right);
            GameObject newObj = new GameObject(string.Format("Node {0}",plant.nodeDepth - nodeDepth));
            float angleStep = plant.splitAngle / plant.splitCount;
            for(int i = 0; i < plant.splitCount; i++)
            {
                float angle = angleStep * i + (angleStep / 2);
                float percentage = angle / plant.splitAngle;
                Vector3 direction = Vector3.ProjectOnPlane(plant.DirFromAngle(angle,false).normalized,plane.normal).normalized;
                GameObject obj = Instantiate(newObj,dir*dist,Quaternion.identity,this.transform);
                obj.transform.position = transform.position + (direction * dist);
                childNodes.Add(obj.AddComponent<Node>());
                childNodes[i].InitNode(plant,this);
                result &= childNodes[i].Extend(nodeDepth - 1,dir,dist);
            }
            DestroyImmediate(newObj);
        }
        {
            GameObject newObj = new GameObject(string.Format("Node {0}",plant.nodeDepth - nodeDepth));
            GameObject obj = Instantiate(newObj,dir*dist,Quaternion.identity,this.transform);
            obj.transform.position = transform.position + (dir.normalized * dist);
            DestroyImmediate(newObj);
            childNodes.Add(obj.AddComponent<Node>());
            childNodes[childNodes.Count - 1].InitNode(plant,this);
            result &= childNodes[childNodes.Count - 1].Extend(nodeDepth - 1,dir,dist);
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
                nodes.Add(transform.position);
                nodes.AddRange(node.DebugDrawNode());
            }
        }
        
        return nodes;
    }

    public List<Node> DebugGetNodes()
    {
        List<Node> nodes = new List<Node>();
        if(childNodes.Count > 0)
        {
            foreach(Node node in childNodes)
            {
                nodes.AddRange(node.DebugGetNodes());
            }
        }
        nodes.Add(this);
        return nodes;
    }

    public List<Node> GetChildNodes()
    {        
        return childNodes;
    }
}