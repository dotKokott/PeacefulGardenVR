using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayer : MonoBehaviour {

	private WindRecording recording;
    private int currentSample = 0;

    private float sampleRate = 0;
    private float sampleTimer = 0;
    private int direction = 1;

    public bool PlaySamples = false;

    private TrailRenderer trail;
    private float trailTime = 1f;
    private float playTimer = 0f;

    private Vector3 offset = new Vector3();

	void Start () {
        trail = GetComponent<TrailRenderer>();
		trailTime = trail.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(!PlaySamples || !trail.enabled || trail.time == 0) return;
        
        playTimer += Time.deltaTime;
        sampleTimer += Time.deltaTime;
        if(sampleTimer >= sampleRate) {
            sampleTimer = 0;
                        
            transform.position = recording.Samples[currentSample] + offset;            

            currentSample++;
            if(currentSample >= recording.Samples.Count) {
                PlaySamples = false;                
                Invoke("RestartTrail", trail.time);                   
            }
        }

        const float DIE_TIME = 20f;
        const float MAX_Y = 17f;

        if(playTimer >= DIE_TIME && offset.y < MAX_Y ) {
            offset += Vector3.up * Time.deltaTime * 0.3f;            
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
       sampleRate = windRecording.SampleTime;
       sampleTimer = windRecording.SampleTime;
       currentSample = 0;
       PlaySamples = true;
    }
}
