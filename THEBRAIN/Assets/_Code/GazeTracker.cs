using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeTracker : MonoBehaviour {

	private Transform hmd;

    public bool IsInGaze = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        assignHMD();
		
        IsInGaze = false;

        var ray = new Ray(hmd.transform.position, hmd.transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, float.MaxValue)) {
            if(hit.collider.gameObject == this.gameObject) {
                IsInGaze = true;
            }
        }
	}

    void assignHMD() {
        if(hmd != null) return;

        SteamVR_TrackedObject[] trackedObjects = FindObjectsOfType<SteamVR_TrackedObject>();
        foreach (SteamVR_TrackedObject tracked in trackedObjects)
        {
            if (tracked.index == SteamVR_TrackedObject.EIndex.Hmd)
            {
                hmd = tracked.transform;
                return;
            }
        }

        //Fallback
        hmd = Camera.main.transform;
    }
}
