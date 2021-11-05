using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField]



    bool generated = false;
    Node rootNode;

    public int seed;
    public int nodeDepth;
    public float nodeDistance;
    public int splitDepth, splitFrequency, splitCount;
    [Range(0,180)]
    public float splitAngle;

    [Range(0,1)]
    public float randomisationPercentage;
    [Range(0,100)]
    public int upwardBias;

    [Range(-180,180)]
    public int nodeRotationMin,nodeRotationMax;

    public int leafDepth, leafFrequency;

    public bool autoUpdate = false;
    public void Generate()
    {
        System.Random prng = new System.Random(seed);
        if(rootNode == null)
        {
            rootNode = gameObject.AddComponent<Node>();            
            rootNode.InitNode(this,null);
        }
        rootNode.Extend(nodeDepth,Vector3.up,nodeDistance,prng); 
        generated = true;       
    }

    public void UpdateGeneration()
    {
        System.Random prng = new System.Random(seed);
        if(generated)
            rootNode.UpdateGeneration(nodeDepth,Vector3.up,nodeDistance,prng);    
    }
    
    public void Clear()
    {
        if(rootNode != null)
        {
            rootNode.Clear();
        }
        generated = false;
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
                //Gizmos.DrawSphere(position,0.1f);
            }
        }
    }

    public Node GetRootNode()
    {
        return rootNode;
    }

    public Vector3 DirFromAngle(float angleInDegrees,bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
