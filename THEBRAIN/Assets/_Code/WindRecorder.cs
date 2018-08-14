using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class WindRecording {
    public float TrailTime = 0;
    public float SampleTime = 0.01f;
    public List<Vector3> Samples;    
    public Vector3 HemispherePosition;
    public Vector3 Origin;
}

public class WindRecorder : MonoBehaviour {

    public static string SAVE_PATH = @"D:\WindRecordings";

    public float SampleTime = 0.01f;
	private float sampleTimer = 0;
    private float recordingTimer = 0;

    private List<Vector3> samples = new List<Vector3>();    

    private bool recording = false;

    public HandRole HandRole;
    
    private TrailRenderer trail;

	void Start () {		        
        trail = GetComponent<TrailRenderer>();
        //originalTime = trail.time;
	}
	
	
	void Update () {       
		if (ViveInput.GetPressDown(HandRole, ControllerButton.TriggerTouch)) {
            recording = true;
            samples.Clear();
        }

        if(recording) {
            trail.time += Time.deltaTime;

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
            var rec = getCurrentRecording();
            
            SaveCurrentRecording(rec);
            PlayRecording(rec);

            Invoke("ResetTrail", 1);
        }
	}

    public void ResetTrail() {
        trail.time = 1;
    }

    public WindPlayer PlayRecording(WindRecording windRecording) {
        var obj = Instantiate(this.gameObject);            
        obj.transform.position = windRecording.Samples[0];
            
        obj.GetComponent<WindRecorder>().enabled = false;
        var player = obj.AddComponent<WindPlayer>();              
        
        player.Play(windRecording);        

        return player;
    }

    private WindRecording getCurrentRecording() {
        var recording = new WindRecording();
        recording.SampleTime = SampleTime;
        recording.Samples = new List<Vector3>(samples);
        recording.TrailTime = trail.time;

        recording.Origin = transform.position;


        var pos = Random.onUnitSphere;
        pos.y = Mathf.Abs(pos.y);

        const float HEMISPHERE_RADIUS = 6f;

        pos *= HEMISPHERE_RADIUS;

        recording.HemispherePosition = pos;

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
