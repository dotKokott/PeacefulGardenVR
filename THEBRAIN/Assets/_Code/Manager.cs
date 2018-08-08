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

	void Start () {
	    XRSettings.eyeTextureResolutionScale = RenderScale;	
        
        Debug.Log("Setting Floor position");
        Floor.position = new Vector3(Floor.position.x, FloorY, Floor.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
