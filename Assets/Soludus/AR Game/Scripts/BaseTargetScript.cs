using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseTargetScript : MonoBehaviour {

    public GameObject trackableFairy;
    public GameObject localFairy;
    public GameObject trackableFairySpeechBubble;

    public GameEngine engine;
    public TouchScreenScript tss;

    public GameObject bear;

    public GameObject menu;

    public AudioClipManagerScript acm;
    //public AudioSource bearAudioSource;
    //public AudioSource fairyAudioSource;
    public AudioSource fairySpeechAS;
    public AudioSource soundEffectAS;

    public DataAction[] dataAction;
    public DataActionController dataActionController;

    protected IEnumerator ShowFairySpeech(string inputText, float showDuration)
    {
        trackableFairySpeechBubble.SetActive(true);
        trackableFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;
        yield return new WaitForSeconds(showDuration);
        trackableFairySpeechBubble.SetActive(false);
    }

    protected IEnumerator ShowFairySpeechWaitForInput(string inputText, float waitTime)
    {
        trackableFairySpeechBubble.SetActive(true);
        trackableFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;

        yield return StartCoroutine(tss.WaitForInput(waitTime));

        trackableFairySpeechBubble.SetActive(false);
    }
}
