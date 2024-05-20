using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshOven : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface2D;
    public void GenerateLevel()
    {
        surface2D.BuildNavMeshAsync();
        //BroadcastMessage("OnLevelReady");

    }

    private void Update()
    {
        surface2D.UpdateNavMesh(surface2D.navMeshData);
    }
}
