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

    private ParticleSystem growingEffect;

	void Start () {
        growingEffect = (Instantiate(Manager._.GrowingParticles) as GameObject).GetComponent<ParticleSystem>();
        growingEffect.transform.position = transform.parent.position;
        growingEffect.gameObject.SetActive(true);
        growingEffect.Stop();



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
            audio.playOnAwake = false;
            Manager._.PlayRandomSeed(audio);

            audio.clip = Manager._.GrowSounds[Random.Range(0, Manager._.GrowSounds.Length)];
            audio.loop = true;
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

            if(growingEffect && !growingEffect.isPlaying) {
                growingEffect.Play();
            }

            if(!IsGrass) {
                if(!audio.isPlaying) {
                    audio.Play();
                }
                audio.volume = 1f;
            }

        } else {
            if(growingEffect && growingEffect.isPlaying) growingEffect.Stop();

            if(!IsGrass && grownTime > 0) {
                audio.volume = 0f;            
            }
            
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
            
            if(growingEffect) growingEffect.Stop();
            
            Destroy(growingEffect.gameObject, 5f);

            GetComponent<Collider>().enabled = false;

            doodleAnimator.ChangeAnimation(IdleAnimations[season]);            
            doodleAnimator.GoToAndPlay(Random.Range(0, doodleAnimator.File.Length));

            if(audio) {
                audio.enabled = false;
            }            

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
