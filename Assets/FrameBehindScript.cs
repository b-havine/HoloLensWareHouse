using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameBehindScript : MonoBehaviour {
	// Use this for initialization
	void Start () {
        this.transform.SetAsFirstSibling();
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
