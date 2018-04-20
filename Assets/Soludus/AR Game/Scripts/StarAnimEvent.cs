using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarAnimEvent : MonoBehaviour {

    AudioSource audioS;

    private void PlayStarSound()
    {
        audioS = GetComponent<AudioSource>();
        audioS.Play();
    }
}
