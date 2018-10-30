using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CombineMeshes : MonoBehaviour{


    public Material material;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.O)) {
            SpatialMappingManager.Instance.StartObserver();
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            SpatialMappingManager.Instance.StopObserver();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            //SpatialMappingManager.Instance.StopObserver();
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 0;
            while (i < meshFilters.Length)
            {
               
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                meshFilters[i].gameObject.SetActive(false);
                i++;
            }
            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            transform.gameObject.SetActive(true);
            gameObject.GetComponent<Renderer>().material = material;

        }
	}
}
