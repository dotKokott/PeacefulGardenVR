using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRecorder : MonoBehaviour {

    public float SampleTime = 0.01f;
	private float sampleTimer = 0;

    private List<Vector3> samples = new List<Vector3>();    

    private bool recording = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.R)) {
            recording = true;
            samples.Clear();
            Debug.Log("Start recording");
        }

        if(recording) {
            sampleTimer += Time.deltaTime;

            if(sampleTimer >= SampleTime) {
                sampleTimer = 0;

                samples.Add(transform.position);
            }
        }

        if(Input.GetKeyDown(KeyCode.T)) {
            
            recording = false;

            var obj = Instantiate(this.gameObject);
            obj.transform.position = samples[0];
            
            obj.GetComponent<WindRecorder>().enabled = false;

            var player = obj.AddComponent<WindPlayer>();

            Debug.Log("Stop recording " + samples.Count);
            player.Play(samples, SampleTime);
        }
	}
}
