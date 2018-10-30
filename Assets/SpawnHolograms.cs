using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.SpatialMapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHolograms : MonoBehaviour/*, ISpeechHandler*/
{
/*

    public GameObject robot;
    public Camera camera;
    public float distance = 2.0f;
    private int countBob = 1;
    public static List<LocationNumber> locationNumbers = new List<LocationNumber>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject holoGram = GameObject.CreatePrimitive(PrimitiveType.Cube);
            holoGram.AddComponent<TapToPlace>();
            holoGram.AddComponent<WorldAnchorManager>();
            
            holoGram.transform.position = camera.transform.position + camera.transform.forward * distance;
            holoGram.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            holoGram.name = "bob " + countBob;
            countBob++;
            locationNumbers.Add(new LocationNumber);
            Debug.Log(locationNumbers.Count + " size" + holoGram.name);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            robot.transform.position = camera.transform.position + camera.transform.forward * 2.0f;
            Instantiate(robot);
        }
    }
    public List<GameObject> GetHolograms()
    {
        return locationNumbers;
    }

    public void OnSpeechKeywordRecognized(SpeechEventData eventData)
    {
        if (eventData.RecognizedText.Equals("go")) {
            robot.transform.position = camera.transform.position + camera.transform.forward * 2.0f;
            Instantiate(robot);
        }
        if (eventData.RecognizedText.Equals("new")) {
            GameObject holoGram = GameObject.CreatePrimitive(PrimitiveType.Cube);
            holoGram.AddComponent<TapToPlace>();
            holoGram.AddComponent<WorldAnchorManager>();

            holoGram.transform.position = camera.transform.position + camera.transform.forward * distance;
            holoGram.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            holoGram.name = "bob " + countBob;
            countBob++;
            locationNumbers.Add(holoGram);
            Debug.Log(locationNumbers.Count + " size" + holoGram.name);
        }
    }*/
}
