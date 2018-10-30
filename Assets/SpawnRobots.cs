using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnRobots : MonoBehaviour {
    public GameObject robot;
    public static Transform startPosition;
    private List<LocationNumber> locations;
    public static int counter = 0;
	// Use this for initialization
	void Start () {
        locations = FinalBarcodeScanner.locationNumbers;
        StartCoroutine(InstantiateRobotCooldown());
        GameObject.Find("Errors").GetComponent<Text>().text = "";//"we should be spawning robots now. First is: " + locations[counter].LocNumOnWall.name;
	}
    private IEnumerator InstantiateRobotCooldown() {
        while (true) {
            Instantiate(robot);
            startPosition = locations[counter].LocNumOnWall.transform;
            robot.transform.position = startPosition.position;
            yield return new WaitForSeconds(1.5F);
        }
    }
    private void Update()
    {
        
    }
}

