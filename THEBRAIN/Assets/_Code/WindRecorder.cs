using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindRecorder : MonoBehaviour {

    public float SampleTime = 0.01f;
	private float sampleTimer = 0;

    private List<Vector3> samples = new List<Vector3>();    

    private bool recording = false;

    public HandRole HandRole;

    //private SteamVR_TrackedObject tracked;

	void Start () {		
        //tracked = GetComponentInParent<SteamVR_TrackedObject>();
	}
	
	// Update is called once per frame
	void Update () {       
        //if(tracked.index == SteamVR_TrackedObject.EIndex.None) return;

        //Debug.Log(SteamVR_Controller.Input((int)tracked.index).GetTouchDown(SteamVR_Controller.ButtonMask.Trigger));

		if (ViveInput.GetPressDown(HandRole, ControllerButton.TriggerTouch)) {
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

        if (ViveInput.GetPressUp(HandRole, ControllerButton.TriggerTouch)) {            
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
