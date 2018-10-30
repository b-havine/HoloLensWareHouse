using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{

    //public NavMeshSurface[] surfaces;
    public List<NavMeshSurface> surfaces = new List<NavMeshSurface>();
    public Transform[] objectsToRotate;

    // Use this for initialization
    void Start()
    {

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            surfaces[0] = GameObject.Find("SpatialMapping").GetComponent<NavMeshSurface>();
            for (int i = 0; i < surfaces.Count; i++)
            {
                surfaces[i].BuildNavMesh();
            }
            Debug.Log("did we bake");
        }
        if (Input.GetKeyDown(KeyCode.C)) {

            Destroy(GameObject.Find("SpatialMapping").GetComponent<NavMeshSurface>());
        }
        
    }

}