using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearEventScript : MonoBehaviour {

    public AudioClipManagerScript acm;
    AudioSource aso;

    private void Awake()
    {
        aso = GetComponent<AudioSource>();
    }

    public void BearIdleEvent()
    {
        if (aso != null)
        {
            if (!aso.isPlaying || aso.clip.name == "OtsoIdleNewTest_Plcaeholder")
            {
                aso.clip = acm.bearAudioClips[0];
                aso.Play();
            }
        }
    }

    public void BearNodEvent()
    {
        aso.clip = acm.bearAudioClips[1];
        aso.Play();
    }

    public void BearHeadDownEvent()
    {
        aso.clip = acm.bearAudioClips[2];
        aso.Play();
    }

    public void BearWonderingEvent()
    {
        aso.clip = acm.bearAudioClips[3];
        aso.Play();
    }

    public void BearDevouringEvent()
    {
        aso.clip = acm.bearAudioClips[4];
        aso.Play();
    }

    public void BearSittingEvent()
    {
        aso.clip = acm.bearAudioClips[5];
        aso.Play();
    }

    public void BearJumpEvent()
    {
        aso.clip = acm.bearAudioClips[6];
        aso.Play();
    }

    public void BearDisapprovalEvent()
    {
        aso.clip = acm.bearAudioClips[7];
        aso.Play();
    }

    public void BearClappingEvent()
    {
        aso.clip = acm.bearAudioClips[8];
        aso.Play();
    }

    public void BearChoosingEvent()
    {
        aso.clip = acm.bearAudioClips[9];
        aso.Play();
    }

    public void BearWalkRightFootEvent()
    {
        aso.clip = acm.bearAudioClips[10];
        aso.Play();
    }

    public void BearWalkLeftFootEvent()
    {
        aso.clip = acm.bearAudioClips[11];
        aso.Play();
    }

    public void BearScareEvent()
    {
        aso.clip = acm.bearAudioClips[12];
        aso.Play();
    }
}
