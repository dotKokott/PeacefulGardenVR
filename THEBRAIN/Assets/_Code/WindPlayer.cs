using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindPlayer : MonoBehaviour {

	private List<Vector3> positions = new List<Vector3>();
    private int currentSample = 0;

    private float sampleRate = 0;
    private float sampleTimer = 0;

    public bool PlaySamples = false;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!PlaySamples) return;

        transform.position = positions[currentSample];
        sampleTimer += Time.deltaTime;
        if(sampleTimer >= sampleRate) {
            sampleTimer = 0;

            currentSample++;
            if(currentSample >= positions.Count) {
                currentSample = 0;
            }
        }
	}

    public void Play(List<Vector3> samples, float rate) {
       positions.AddRange(samples);
       sampleRate = rate;
       currentSample = 0;
       PlaySamples = true;
    }
}
