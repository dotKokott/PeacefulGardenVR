using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GazeTracker))]
public class Seed : MonoBehaviour {

    private GazeTracker gazeTracker;

    public bool Planted = false;

    public SteamVR_TrackedObject ParentTracker;

	void Start () {
	    gazeTracker = GetComponent<GazeTracker>();
	}

    public void LookAtCamera() {
         var v = transform.position - Camera.main.transform.position;
         v.y = 0;
         transform.rotation = Quaternion.LookRotation(v);
    }
		
	void Update () {    
        //if(!Planted && gazeTracker.IsInGaze) {
        //    Plant();
        //}
        
        //if(ParentTracker != null && !Planted) {
        //    transform.position = ParentTracker.transform.position;
        //}
	}

    public void Plant() {
        Debug.Log("Planted seed");
        Planted = true;
    }
}
