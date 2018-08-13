using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRecorder : MonoBehaviour {

    public float SampleTime = 0.01f;
	private float sampleTimer = 0;
    private float recordingTimer = 0;

    private List<Vector3> samples = new List<Vector3>();    

    private bool recording = false;

    public HandRole HandRole;
    
	void Start () {		        
	}
	
	
	void Update () {       
		if (ViveInput.GetPressDown(HandRole, ControllerButton.TriggerTouch)) {
            recording = true;
            samples.Clear();
            Debug.Log("Start recording");
        }

        if(recording) {
            sampleTimer += Time.deltaTime;
            recordingTimer += Time.deltaTime;

            if(sampleTimer >= SampleTime) {
                sampleTimer = 0;

                samples.Add(transform.position);
            }
        }

        if (ViveInput.GetPressUp(HandRole, ControllerButton.TriggerTouch)) {            
            recording = false;            

            if(recordingTimer < 0.5f) {
                recordingTimer = 0;
                return;
            }

            recordingTimer = 0;


            //var parent_obj = new GameObject("WindParent").transform;
            //parent_obj.gameObject.AddComponent<WindParent>();
            //parent_obj.position = samples[0];

            var obj = Instantiate(this.gameObject);
            //obj.transform.parent = parent_obj;
            obj.transform.position = samples[0];
            
            obj.GetComponent<WindRecorder>().enabled = false;

            var player = obj.AddComponent<WindPlayer>();

            Debug.Log("Stop recording " + samples.Count);
            player.Play(samples, SampleTime);
        }
	}
}
