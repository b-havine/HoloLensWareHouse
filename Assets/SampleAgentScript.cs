using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleAgentScript : MonoBehaviour {

    public static Transform target;
    public NavMeshAgent agent;
    public Transform camPosition;
    private bool readyForNext = false;

        // Use this for initialization
        void Start() {
            agent = GetComponent<NavMeshAgent>();
        // target = GameObject.Find("bob").transform;
        //target = TakeScreenshot.locationNumbers[0].LocNumOnWall.transform;
        //target = SpawnHolograms.holograms[countElementInArray].gameObject.transform;
    }

    // Update is called once per frame
    void Update() {
        if (camPosition == null) {
            camPosition = GameObject.Find("MixedRealityCamera").transform;
        }
            agent.SetDestination(camPosition.position);

            //Debug.Log("Robot position: " + gameObject.transform.position + ", Hologram position: " + SpawnHolograms.holograms[0].transform.position);
        if (agent.remainingDistance < 0.8 && agent.remainingDistance != 0 && readyForNext == false) {
                Debug.Log("destination reached!" + agent.remainingDistance);
            Destroy(gameObject);
            //hvis det du scanner nu er det rigtige lokations nummmer og robotten er tæt på den, så kan du scanne varen bagefter,
            //hvis varen sammensvarer med locationsnummeret, så er opgaven fuldført, og du kan nu sige 'location' for at sende robotten videre.
            //når du når til baydoor, så skal der stå at der ingen opgaver er tilbage.
            }
        }
    }

