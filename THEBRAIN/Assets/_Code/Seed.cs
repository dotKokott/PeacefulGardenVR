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

    public DoodleAnimationFile IdleAnimation;

	void Start () {
	    gazeTracker = GetComponent<GazeTracker>();
        doodleAnimator = GetComponent<DoodleAnimator>();

        LookAtCamera();
	}

    public void LookAtCamera() {
         var v = transform.position - Camera.main.transform.position;
         v.y = 0;
         transform.rotation = Quaternion.LookRotation(v);
    }
		
	void Update () {    
        if(Planted) return;

        if(Input.GetKeyDown(KeyCode.Space) && !doodleAnimator.Playing && !Planted) {
            Debug.Log("Grow");
            StartCoroutine(PlayUntilFinishAndReplace());
        }

        if(Input.GetKeyDown(KeyCode.P) && !Planted) {
            Debug.Log("Pause");
            StopAllCoroutines();
            doodleAnimator.Pause();
        }

        //if(!gazeTracker.IsInGaze) {
        //    Debug.Log("Pause");
        //    doodleAnimator.Pause();
        //}

        //if(!Planted && gazeTracker.IsInGaze) {
        //    Plant();
        //}
        
        //if(ParentTracker != null && !Planted) {
        //    transform.position = ParentTracker.transform.position;
        //}
	}

    public IEnumerator PlayUntilFinishAndReplace() {
        yield return doodleAnimator.PlayAndPauseAt(doodleAnimator.CurrentFrame, doodleAnimator.File.Length - 1);

        Planted = true;

        doodleAnimator.ChangeAnimation(IdleAnimation);
        doodleAnimator.Play();
    }

    public void Plant() {
        Debug.Log("Planted seed");
        Planted = true;
    }
}
