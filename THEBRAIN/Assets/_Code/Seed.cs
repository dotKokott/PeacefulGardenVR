using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_GazeTracker))]
public class Seed : MonoBehaviour {

    private SteamVR_GazeTracker tracker;

	void Start () {
	    tracker = GetComponent<SteamVR_GazeTracker>();
	}
		
	void Update () {    
        if(tracker.isInGaze) {            
            //transform.localScale += Vector3.up * Time.deltaTime * 5f;
        }
	}
}
