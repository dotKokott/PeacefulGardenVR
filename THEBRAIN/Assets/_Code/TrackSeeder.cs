﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSeeder : MonoBehaviour {
    

    private float timeoutTime = 0.3f;

    private Collider col;

	void Start () {
        col = GetComponent<Collider>();		
	}
	
    public void Timeout(float time = 0.3f) {        
        StartCoroutine(timeout(time));
    }

    private IEnumerator timeout(float time = 0.3f) {
        if(!col.enabled) yield break;

        col.enabled = false;
        yield return new WaitForSeconds(time);
        col.enabled = true;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
