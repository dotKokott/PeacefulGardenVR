using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;

public class Manager : MonoBehaviour {

	public float RenderScale = 1f;

	void Start () {
	    XRSettings.eyeTextureResolutionScale = RenderScale;	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
