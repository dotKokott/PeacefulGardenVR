﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour {

	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Tracker") {
            Debug.Log("Tracker entered");
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Tracker") {
            Debug.Log("Tracker Exit");
        }
    }
}
