using System.Collections;
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
            var obj = Instantiate(Manager._.Seeds[Random.Range(0, Manager._.Seeds.Length)], this.transform) as GameObject;
            obj.transform.position = other.transform.position;
            //TODO find normal
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Tracker") {
            
        }
    }
}
