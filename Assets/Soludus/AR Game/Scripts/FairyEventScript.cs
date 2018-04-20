using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyEventScript : MonoBehaviour {

    public AudioClipManagerScript acm;
    AudioSource fas;

    private void Awake()
    {
        fas = GetComponent<AudioSource>();
    }

    public void FairyIdleEvent()
    {
        if (fas != null)
        {
            if (!fas.isPlaying || fas.clip.name == "KeijuIdleNew_Test2")
            {
                fas.clip = acm.fairyAudioClips[0];
                fas.Play();
            }
        }
    }

    public void FairyMoveEvent()
    {
        fas.clip = acm.fairyAudioClips[1];
        fas.Play();
    }

    public void FairyClappingEvent()
    {
        fas.clip = acm.fairyAudioClips[2];
        fas.Play();
    }

    public void FairyTurnEvent()
    {
        fas.clip = acm.fairyAudioClips[3];
        fas.Play();
    }

    public void FairySpeakEvent()
    {
        fas.clip = acm.fairyAudioClips[4];
        fas.Play();
    }

    public void FairyTouchEvent()
    {
        fas.clip = acm.fairyAudioClips[5];
        fas.Play();
    }

    public void FairyWavingEvent()
    {
        fas.clip = acm.fairyAudioClips[6];
        fas.Play();
    }

    public void FairyWinEvent()
    {
        fas.clip = acm.fairyAudioClips[8];
        fas.Play();
    }

    public void FairyHugEvent()
    {
        fas.clip = acm.fairyAudioClips[9];
        fas.Play();
    }
}
