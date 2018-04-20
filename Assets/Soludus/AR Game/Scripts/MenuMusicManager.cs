using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("PlayMenuMusic", 5);
	}

    void PlayMenuMusic()
    {
        GetComponent<AudioSource>().Play();
    }
}
