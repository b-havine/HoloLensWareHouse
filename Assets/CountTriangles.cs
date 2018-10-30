using HoloToolkit.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountTriangles : MonoBehaviour {

    public Text wall;
    public Text surface;
    public Text allTriangles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        switch (SpatialUnderstanding.ScanStates.Scanning)
        {
            case SpatialUnderstanding.ScanStates.Scanning:
                this.LogSurfaceState();
                break;
        }
	}
    private void LogSurfaceState()
    {
        IntPtr statsPtr = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStatsPtr();
        if (SpatialUnderstandingDll.Imports.QueryPlayspaceStats(statsPtr) != 0)
        {
            var stats = SpatialUnderstanding.Instance.UnderstandingDLL.GetStaticPlayspaceStats();
            //Debug.Log(stats.TotalSurfaceArea);
            wall.GetComponent<Text>().text = "Wall count: " + stats.WallSurfaceArea.ToString();
            surface.GetComponent<Text>().text = "Surface: " + stats.HorizSurfaceArea.ToString();
            allTriangles.GetComponent<Text>().text = "Total: " + stats.TotalSurfaceArea.ToString();

                }
    }
}
