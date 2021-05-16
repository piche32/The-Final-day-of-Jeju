﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LavaSound : MonoBehaviour
{
    [SerializeField] AudioClip boom = null;
    AudioSource audioPlayer = null;
    bool isFinished = false;
    // Start is called before the first frame update

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.spatialBlend = 1.0f;
        audioPlayer.clip = boom;
    }

    private void Update()
    {
        if(isFinished && !audioPlayer.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayBoomSound()
    {
        audioPlayer.Play();
        isFinished = true;
    }
}