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
        var closestPoint = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);       

        if(other.tag == "Tracker") {           
            var obj = Instantiate(Manager._.Seeds[Random.Range(0, Manager._.Seeds.Length)]) as GameObject;            
            obj.transform.position = closestPoint;
            obj.SetActive(true);            
            
            //other.GetComponent<TrackSeeder>().Timeout();
        } else if(other.tag == "Controller") {
            Manager._.PlayGrassSound();

            var spread = 0.03f;
            for(var i = 0; i < 3; i++) {
                var obj = Instantiate(Manager._.Grasses[Random.Range(0, Manager._.Grasses.Length)]) as GameObject;

                var uCircle = Random.insideUnitCircle;

                closestPoint += new Vector3(uCircle.x, 0, uCircle.y) * spread;

                obj.transform.position = closestPoint;    
                obj.SetActive(true);                
            }

            other.GetComponent<TrackSeeder>().Timeout(0.3f);            
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Tracker") {
            other.GetComponent<TrackSeeder>().Timeout();
        }
    }
}
