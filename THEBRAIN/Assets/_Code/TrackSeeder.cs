using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSeeder : MonoBehaviour {
    

    private float timeoutTime = 0.3f;

    private Collider col;

	void Start () {
        col = GetComponent<Collider>();		
	}
	
    public void Timeout() {        
        StartCoroutine(timeout());
    }

    private IEnumerator timeout() {
        if(!col.enabled) yield break;

        Debug.Log("tracker off: " + gameObject.name);

        col.enabled = false;
        yield return new WaitForSeconds(timeoutTime);
        col.enabled = true;

        Debug.Log("tracker on: " + gameObject.name);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
