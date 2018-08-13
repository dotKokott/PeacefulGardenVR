using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class WindRecording {
    public float SampleTime;
    public List<Vector3> Samples;    
}

public class WindRecorder : MonoBehaviour {

    public static string SAVE_PATH = @"D:\WindRecordings";

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

            var obj = Instantiate(this.gameObject);            
            obj.transform.position = samples[0];
            
            obj.GetComponent<WindRecorder>().enabled = false;

            var player = obj.AddComponent<WindPlayer>();

            var rec = getCurrentRecording();
            
            SaveCurrentRecording(rec);

            player.Play(rec);
        }
	}

    private WindRecording getCurrentRecording() {
        var recording = new WindRecording();
        recording.SampleTime = SampleTime;
        recording.Samples = new List<Vector3>(samples);

        return recording;
    }

    public void SaveCurrentRecording(WindRecording rec) {
        Debug.Log("Attempting to save to disk!");
        var t = new Thread(new ParameterizedThreadStart(saveCurrentRecording));
        t.Start(rec);
    }

    private void saveCurrentRecording(object rec) {       
        var json = JsonUtility.ToJson(rec);

        var path = Path.Combine(SAVE_PATH, System.DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".json");

        File.WriteAllText(path, json);

        Debug.Log("Saved to disk!");
    }
}
