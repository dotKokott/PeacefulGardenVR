using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazePointer : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {
        var gazeRay = new Ray(transform.position, transform.forward);

        RaycastHit hitInfo;

	    if(Physics.Raycast(gazeRay, out hitInfo, 1000f)) {
            Debug.DrawRay(gazeRay.origin, gazeRay.direction);
            Debug.Log(hitInfo.collider.name);

        }
	}
}
