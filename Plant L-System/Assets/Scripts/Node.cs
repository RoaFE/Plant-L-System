using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class Node : MonoBehaviour
{
    Plant plant;
    public Node parentNode;
    List<Node> childNodes;
    public Vector3 position = Vector3.zero;

    Plane splitPlane;

    int depth;

    public Vector3 planeNormal;
    //Stem stem;

    public void InitNode(Plant inPlant, Node inPrevNode)
    {
        childNodes = new List<Node>();
        plant = inPlant;
        if(inPrevNode != null)
        {
            parentNode = inPrevNode;
        }
        planeNormal = transform.forward;
        splitPlane = new Plane(planeNormal,transform.position);
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
        depth = nodeDepth;
        Vector3 randomDir = Random.insideUnitSphere.normalized;
        dir = Vector3.Lerp(dir,randomDir,plant.randomisationPercentage);
        
        bool result = true;
        if(plant.nodeDepth - nodeDepth >= plant.splitDepth && ((plant.nodeDepth - nodeDepth) - plant.splitDepth) % plant.splitFrequency == 0)
        {
            
            GameObject newObj = new GameObject(string.Format("Node {0}",plant.nodeDepth - nodeDepth));
            float angleStep = plant.splitAngle / plant.splitCount;
            for(int i = 0; i < plant.splitCount; i++)
            {
                float angle = angleStep * i + (angleStep / 2) - (plant.splitAngle / 2);
                float percentage = angle / plant.splitAngle;
                Vector3 direction = Quaternion.AngleAxis(angle,splitPlane.normal) * dir;

                GameObject obj = Instantiate(newObj,dir*dist,Quaternion.identity,this.transform);
                obj.transform.position = transform.position + (direction * dist);
                obj.transform.localRotation = Quaternion.AngleAxis(Random.Range(plant.nodeRotationMin,plant.nodeRotationMax),dir);
                childNodes.Add(obj.AddComponent<Node>());
                childNodes[i].InitNode(plant,this);
                result &= childNodes[i].Extend(nodeDepth - 1,direction,dist);
            }
            DestroyImmediate(newObj);
        }
        else {
            GameObject newObj = new GameObject(string.Format("Node {0}",plant.nodeDepth - nodeDepth));
            GameObject obj = Instantiate(newObj,dir*dist,Quaternion.identity,this.transform);
            obj.transform.position = transform.position + (dir.normalized * dist);
            obj.transform.localRotation = Quaternion.AngleAxis(Random.Range(plant.nodeRotationMin,plant.nodeRotationMax),dir);
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

    public void constructMesh(int curveDepth)
    {

    }

    public List<Node> GetParentNodes()
    {
        List<Node> parentNodes = new List<Node>();        
        if(parentNode != null)
        {
            parentNodes.Add(parentNode);
            parentNodes.AddRange(parentNode.GetParentNodes());
        }
        return parentNodes;
    }
}
