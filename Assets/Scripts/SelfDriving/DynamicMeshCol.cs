using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMeshCol : MonoBehaviour
{
    void Start()
    {
        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
    }

}
