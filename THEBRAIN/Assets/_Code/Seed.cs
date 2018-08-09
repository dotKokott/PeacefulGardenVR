using DoodleStudio95;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour {

    private DoodleAnimator doodleAnimator;

    public bool Planted = false;
    public bool Paused = true;

    public DoodleAnimationFile IdleAnimation;
    
    private AudioSource audio;

    private float minSize = 0.3f;
    private float maxSize = 1f;

    public bool IsInGaze = false;

    public DoodleAnimationFile[] GrowAnimations;
    public DoodleAnimationFile[] IdleAnimations;

    private int season = 0;

	void Start () {
        season = Random.Range(0, 4);        

        audio = gameObject.AddComponent<AudioSource>();
        doodleAnimator = GetComponent<DoodleAnimator>();
        doodleAnimator.ChangeAnimation(GrowAnimations[season]);

        var newScale = UnityEngine.Random.Range(minSize, maxSize);

        transform.parent.localScale *= newScale;

        SetRotation();

        Manager._.PlayRandomSeed(audio);
	}

    public void SetRotation() {
         //var v = transform.position - Manager._.hmd.position;
         //v.y = 0;
         //transform.rotation = Quaternion.LookRotation(v);
         transform.Rotate(new Vector3(0, Random.Range(0, 180f), 0));
         //transform.RotateAround(Vector3.up, UnityEngine.Random.Range(0, 180f));
    }
	
    

	void Update () {    
        IsInGaze = false;
	}

    void LateUpdate() {
        if(Planted) return;

        if((Input.GetKeyDown(KeyCode.Space) || IsInGaze) && Paused && !Planted) {
            Paused = false;
            StartCoroutine(PlayUntilFinishAndReplace());
        }

        if((Input.GetKeyDown(KeyCode.P) || !IsInGaze) && !Paused && !Planted) {
            Paused = true;
            StopAllCoroutines();
            doodleAnimator.Pause();
        }        
    }

    public IEnumerator PlayUntilFinishAndReplace() {
        Manager._.PlayRandomGrow(audio);

        yield return doodleAnimator.PlayAndPauseAt(doodleAnimator.CurrentFrame, doodleAnimator.File.Length - 1);

        Planted = true;

        GetComponent<Collider>().enabled = false;

        doodleAnimator.ChangeAnimation(IdleAnimations[season]);
        doodleAnimator.Play();
    }

    public void Plant() {
        Debug.Log("Planted seed");
        Planted = true;
    }
}
