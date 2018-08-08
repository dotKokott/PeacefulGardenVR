using DoodleStudio95;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GazeTracker))]
public class Seed : MonoBehaviour {

    private GazeTracker gazeTracker;
    private DoodleAnimator doodleAnimator;

    public bool Planted = false;
    public bool Paused = true;

    public DoodleAnimationFile IdleAnimation;
    

	void Start () {
	    gazeTracker = GetComponent<GazeTracker>();
        doodleAnimator = GetComponent<DoodleAnimator>();

        LookAtCamera();
	}

    public void LookAtCamera() {
         var v = transform.position - Manager._.hmd.position;
         v.y = 0;
         transform.rotation = Quaternion.LookRotation(v);
    }
	
    

	void Update () {    
        if(Planted) return;

        if((Input.GetKeyDown(KeyCode.Space) || gazeTracker.IsInGaze) && Paused && !Planted) {
            Debug.Log("Grow");
            Paused = false;
            StartCoroutine(PlayUntilFinishAndReplace());
        }

        if((Input.GetKeyDown(KeyCode.P) || !gazeTracker.IsInGaze) && !Paused && !Planted) {
            Debug.Log("Pause");
            Paused = true;
            StopAllCoroutines();
            doodleAnimator.Pause();
        }
	}

    public IEnumerator PlayUntilFinishAndReplace() {
        yield return doodleAnimator.PlayAndPauseAt(doodleAnimator.CurrentFrame, doodleAnimator.File.Length - 1);

        Planted = true;

        GetComponent<Collider>().enabled = false;

        doodleAnimator.ChangeAnimation(IdleAnimation);
        doodleAnimator.Play();
    }

    public void Plant() {
        Debug.Log("Planted seed");
        Planted = true;
    }
}
