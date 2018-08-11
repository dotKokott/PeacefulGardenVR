﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.XR;
using Valve.VR;

public class Manager : MonoBehaviour {    

	public float RenderScale = 1f;

    public float FloorY = 0.4f;

    public Transform Floor;

    public GameObject[] Controllers;
    public GameObject[] Trackers;

    public GameObject[] Seeds;
    public GameObject[] Grasses;
    
    private AudioSource audioSource;
    public AudioClip PlantingGrass;

    public AudioClip[] SeedSounds;
    public AudioClip[] GrowSounds;    

    public AudioClip[] Chimes;

    public Transform hmd;



    private static Manager instance;
    public static Manager _ {
        get {           
            return instance;
        }
    }



	void Start () {
        if(instance == null) {
            instance = this;

	        XRSettings.eyeTextureResolutionScale = RenderScale;	                   
            SetTronMode(false);
            
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = PlantingGrass;
            audioSource.volume = 0;

            audioSource.Play();
        }               
	}
	

    void assignHMD() {
        if(hmd != null) return;

        SteamVR_TrackedObject[] trackedObjects = FindObjectsOfType<SteamVR_TrackedObject>();
        foreach (SteamVR_TrackedObject tracked in trackedObjects)
        {
            if (tracked.index == SteamVR_TrackedObject.EIndex.Hmd)
            {
                hmd = tracked.transform;
                return;
            }
        }

        //Fallback
        hmd = Camera.main.transform;
    }

    bool tronMode = false;

    float grassSoundTime = 0f;

    private void Update() {
        //if(Input.GetKeyDown(KeyCode.T)) {
        //    tronMode = !tronMode;
        //    SetTronMode(tronMode);
        //}
        
        if(grassSoundTime > 0) {
            grassSoundTime -= Time.deltaTime;

            audioSource.volume = 1f;
        } else {
            audioSource.volume = 0;
        }
        

    }

    void LateUpdate () {
		assignHMD();        

        var ray = new Ray(hmd.transform.position, hmd.transform.forward);        

        var all = Physics.RaycastAll(ray, float.MaxValue);

        foreach(var hit in all) {
            if(hit.collider.tag == "Flower") {
                hit.collider.GetComponent<Seed>().IsInGaze = true;
            }
        }

        //if(Physics.Raycast(ray, out hit, float.MaxValue)) {
        //    if(hit.collider.gameObject == this.gameObject) {
        //        IsInGaze = true;
        //    }
        //}
	}    

    EVRSettingsError SetTronMode(bool enable)
    {
        EVRSettingsError e = EVRSettingsError.None;
        OpenVR.Settings.SetBool(OpenVR.k_pch_Camera_Section,
                                OpenVR.k_pch_Camera_EnableCameraForCollisionBounds_Bool,
                                enable,
                                ref e);
        OpenVR.Settings.Sync(true, ref e);
        return e;
    }

    public void PlayRandomSeed(AudioSource source) {
        source.PlayOneShot(SeedSounds[Random.Range(0, SeedSounds.Length)], 0.5f);
    }

    public void PlayRandomGrow(AudioSource source) {
        source.PlayOneShot(GrowSounds[Random.Range(0, GrowSounds.Length)]);
    }

    public void PlayGrassSound() {
        grassSoundTime = 0.5f;
    }

    public void StopGrassSound() {
        grassSoundTime = 0f;
    }
}
