using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayer : MonoBehaviour {

	private WindRecording recording;
    private int currentSample = 0;
    
    private float sampleTimer = 0;

    public bool PlaySamples = false;

    private TrailRenderer trail;
    private float trailTime = 1f;
    private float playTimer = 0f;

    private Vector3 offset = new Vector3();
    private Vector3 direction = Vector3.zero;

    const float DIE_TIME = 20f;     

    public void SetToFinal() {
        offset = (recording.HemispherePosition);
        playTimer = DIE_TIME;
    }

	void Start () {
        trail = GetComponent<TrailRenderer>();
		trailTime = trail.time;
	}
	
    
    bool stopTravelling = false;
	// Update is called once per frame
	void Update () {
		if(!PlaySamples || !trail.enabled || trail.time == 0) return;
        
        playTimer += Time.deltaTime;
        sampleTimer += Time.deltaTime;
        if(sampleTimer >= recording.SampleTime) {
            sampleTimer = 0;
                        
            transform.position = recording.Samples[currentSample] + offset;            

            currentSample++;
            if(currentSample >= recording.Samples.Count) {
                PlaySamples = false;                
                Invoke("RestartTrail", trail.time);                   
            }
        }   
                
        if(playTimer >= DIE_TIME && !stopTravelling ) {            
            offset += direction * Time.deltaTime * 0.3f;            
            var dist = (recording.HemispherePosition - transform.position).sqrMagnitude;
            if(dist < 0.5f) {
                stopTravelling = true;
            }
        }
	}

    public void RestartTrail() {
        trail.enabled = false;

        currentSample = 0;
        transform.position = recording.Samples[currentSample] + offset;            

        trail.Clear();
        trail.enabled = true;
                    
        PlaySamples = true;
    }

    public void Play(WindRecording windRecording) {
       recording = windRecording;           
       sampleTimer = windRecording.SampleTime;
       
       direction = (windRecording.HemispherePosition - windRecording.Origin).normalized;

       GetComponent<TrailRenderer>().time = windRecording.TrailTime;
        
       currentSample = 0;
       PlaySamples = true;
    }
}
