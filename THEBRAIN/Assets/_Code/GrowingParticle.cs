using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingParticle : MonoBehaviour {
	
	void Start () {
		//GetComponent<ParticleSystemRenderer>().material = ParticleMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward, 100f * Time.deltaTime);
	}
}
