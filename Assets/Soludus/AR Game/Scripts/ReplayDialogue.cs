using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayDialogue : MonoBehaviour {

    public AudioSource currentDialogueAS;

    public void PlayDialogueAgain()
    {
        if (currentDialogueAS != null)
        {
            currentDialogueAS.Stop();
            currentDialogueAS.Play();
        }
    }
}
