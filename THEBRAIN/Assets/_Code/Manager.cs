using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;

public class Manager : MonoBehaviour {    

	public float RenderScale = 1f;

    public float FloorY = 0.4f;

    public Transform Floor;

    public GameObject[] Controllers;
    public GameObject[] Trackers;

    public GameObject[] Seeds;
    
    public AudioClip[] SeedSounds;
    public AudioClip[] GrowSounds;    

    public AudioClip[] Chimes;

    public Transform hmd;



    private static Manager instance;
    public static Manager _ {
        get {           
            return instance;
        }
    }



	void Start () {
        if(instance == null) {
            instance = this;

	        XRSettings.eyeTextureResolutionScale = RenderScale;	
        
            Debug.Log("Setting Floor position");
            Floor.position = new Vector3(Floor.position.x, FloorY, Floor.position.z);
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
	
	void Update () {
		assignHMD();

        //var ray = new Ray(hmd.transform.position, hmd.transform.forward);        

        //var all = Physics.RaycastAll(ray, float.MaxValue);

        //foreach(var hit in all) {
        //    if(hit.collider.tag == "Flower") {
        //        hit.collider.GetComponent<Seed>().
        //    }
        //}

        //if(Physics.Raycast(ray, out hit, float.MaxValue)) {
        //    if(hit.collider.gameObject == this.gameObject) {
        //        IsInGaze = true;
        //    }
        //}
	}
}
