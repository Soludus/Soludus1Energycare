using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioSource musicSource;
    public AudioClip[] musics;
    private bool changeMusic;
    private AudioClip nextMusic;
	
	// Update is called once per frame
	void Update () {

        if (changeMusic == true && !musicSource.isPlaying)
        {
            changeMusic = false;
            musicSource.clip = nextMusic;
            musicSource.loop = true;
            musicSource.Play();
        }
	}

    public void PlayMusicClip(int index, bool looping)
    {
        musicSource.clip = musics[index];
        musicSource.Play();

        if (looping)
            musicSource.loop = true;
        else musicSource.loop = false;
    }

    public void ChangeMusicAfterLoop(int index)
    {
        nextMusic = musics[index];
        changeMusic = true;
        musicSource.loop = false;
    }
}
