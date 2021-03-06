using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGenerator : MonoBehaviour
{

    [SerializeField] private GameObject navMeshRoot = null;

    private List<GameObject> navMeshElements = new List<GameObject>();
    public void SetNavMeshElements(List<GameObject> values)
    {
        navMeshElements.Clear();
        navMeshElements.AddRange(values);
    }

    private void Awake()
    {
        if (navMeshRoot == null) { 
            navMeshRoot = new GameObject("NavMeshRoot");
            navMeshRoot.tag = "lab";
            navMeshRoot.transform.tag = "lab";
        }
    }

    public void BuildNavRoot()
    {
        if (navMeshRoot == null)
        {
            navMeshRoot = new GameObject("NavMeshRoot");
            navMeshRoot.tag = "lab";
            navMeshRoot.transform.tag = "lab";
        }
    }

    public void BuildNavMesh()
    {
        int agentTypeCount = NavMesh.GetSettingsCount();
        if (agentTypeCount < 1) { return; }
        for (int i = 0; i < navMeshElements.Count; ++i) { navMeshElements[i].transform.SetParent(navMeshRoot.transform, true); }
        for (int i = 0; i < agentTypeCount; ++i)
        {
            NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(i);
            NavMeshSurface navMeshSurface = navMeshRoot.AddComponent<NavMeshSurface>();
            navMeshSurface.agentTypeID = settings.agentTypeID;

            NavMeshBuildSettings actualSettings = navMeshSurface.GetBuildSettings();
            navMeshSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders; // or you can use RenderMeshes

            navMeshSurface.BuildNavMesh();
        }

    }

}
