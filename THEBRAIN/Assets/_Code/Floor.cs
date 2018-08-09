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
        var closestPoint = other.ClosestPointOnBounds(transform.position);        

        if(other.tag == "Tracker") {           
            var obj = Instantiate(Manager._.Seeds[Random.Range(0, Manager._.Seeds.Length)]) as GameObject;            
            obj.transform.position = closestPoint;
            obj.SetActive(true);

            Manager._.PlayRandomSeed(obj.GetComponent<AudioSource>());
            //TODO find normal
        } else if(other.tag == "Controller") {
            var obj = Instantiate(Manager._.Grasses[Random.Range(0, Manager._.Grasses.Length)]) as GameObject;
            obj.transform.position = closestPoint;    
            obj.SetActive(true);

            other.GetComponent<TrackSeeder>().Timeout();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Tracker") {
            other.GetComponent<TrackSeeder>().Timeout();
        }
    }
}
