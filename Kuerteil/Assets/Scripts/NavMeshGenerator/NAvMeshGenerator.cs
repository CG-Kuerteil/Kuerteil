using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NAvMeshGenerator : MonoBehaviour
{
    private GameObject[] navMeshes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GenerateNAvMeshes()
    {
        navMeshes = GameObject.FindGameObjectsWithTag("lab");
        Debug.Log("To Bake " + navMeshes.Length + " surfaces");
        for (int i = 0; i < navMeshes.Length; i++)
        {
            navMeshes[i].GetComponent<NavMeshSurface>().UpdateNavMesh(navMeshes[i].GetComponent<NavMeshSurface>().navMeshData);
            Debug.Log("Baked " + i + " surfaces");
        }
        Debug.Log("Baked " + navMeshes.Length + " surfaces");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
