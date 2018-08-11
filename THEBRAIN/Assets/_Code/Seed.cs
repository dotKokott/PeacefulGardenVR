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

    public bool IsGrass = false;

    public float TimeToGrow = 4f;
    private float grownTime = 0;

    private float growScaleFactor = 0.05f;
    private Vector3 originalScale;


	void Start () {
        season = Random.Range(0, 4);        
        
        doodleAnimator = GetComponent<DoodleAnimator>();
        doodleAnimator.ChangeAnimation(GrowAnimations[season]);

        doodleAnimator.SetFrame(0);

        var newScale = UnityEngine.Random.Range(minSize, maxSize);

        transform.parent.localScale *= newScale;
        originalScale = transform.parent.localScale;        

        //transform.parent.localScale = originalScale * GrowScaleFactor;

        SetRotation();   
        
        if(!IsGrass) {
            audio = gameObject.AddComponent<AudioSource>();
            Manager._.PlayRandomSeed(audio);
        }
        
	}

    public void FaceCamera() {
        var v = transform.position - Manager._.hmd.position;
        v.y = 0;
        transform.rotation = Quaternion.LookRotation(v);
    }

    public void SetRotation() {
         transform.Rotate(new Vector3(0, Random.Range(0, 180f), 0));

         if(IsGrass) {
             transform.Rotate(new Vector3(0, 0, Random.Range(-20, 20f)));
             transform.Rotate(new Vector3(Random.Range(-20, 20f), 0, 0));

         }         
    }

    private void Update() {
        IsInGaze = false;
    }

    void LateUpdate() {
        if(Planted) return;

        if(Input.GetKey(KeyCode.Space) || IsInGaze) {
            Grow();    
        }

        //if ((Input.GetKeyDown(KeyCode.Space) || IsInGaze) && Paused && !Planted) {
        //    Paused = false;
        //    StartCoroutine(PlayUntilFinishAndReplace());
        //}

        //if ((Input.GetKeyDown(KeyCode.P) || !IsInGaze) && !Paused && !Planted) {
        //    Paused = true;
        //    StopAllCoroutines();
        //    doodleAnimator.Pause();
        //}
    }

    public void Grow() {
        grownTime += Time.deltaTime;

        var currentFrame = Mathf.FloorToInt((doodleAnimator.File.Length / TimeToGrow) * grownTime);
        doodleAnimator.SetFrame(currentFrame);

        //var scale = (GrowScaleFactor / TimeToGrow) * grownTime;

        transform.parent.localScale += new Vector3(growScaleFactor, growScaleFactor, growScaleFactor) * Time.deltaTime;

        if(grownTime >= TimeToGrow) {
            Planted = true;
            GetComponent<Collider>().enabled = false;

            doodleAnimator.ChangeAnimation(IdleAnimations[season]);
            doodleAnimator.Play();

            enabled = false;
        }
    }

    //public IEnumerator PlayUntilFinishAndReplace() {
    //    if(!IsGrass) {
    //        Manager._.PlayRandomGrow(audio);
    //    }        

    //    yield return doodleAnimator.PlayAndPauseAt(doodleAnimator.CurrentFrame, doodleAnimator.File.Length - 1);

    //    Planted = true;        

    //    doodleAnimator.ChangeAnimation(IdleAnimations[season]);
    //    doodleAnimator.Play();
    //}

    public void Plant() {
        Planted = true;
    }
}
