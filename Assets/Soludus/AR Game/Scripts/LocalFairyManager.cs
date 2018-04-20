using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalFairyManager : MonoBehaviour {

    public GameObject localFairySpeechBubble;

    public GameObject screenShotC;
    public GameObject screenShotCMetsa;
    private bool COrunning;
    public AudioClipManagerScript acm;

    Coroutine speechCO;

    public void StartFairySuggest(int questline)
    {
        FairySuggestSearch(questline);
    }

    public void FairySuggestSearch(int questline)
    {
        if (questline == 1)
        {
            speechCO = StartCoroutine(ShowFairySpeech("Etsikää sinisiä rasteja!", 5f));
        }
        else if (questline == 2 && screenShotC.activeSelf != true && screenShotCMetsa.activeSelf != true)
        {
            speechCO = StartCoroutine(ShowFairySpeech("Etsikää vihreitä rasteja!", 5f));
        }
        else if (questline == 3)
        {
            speechCO = StartCoroutine(ShowFairySpeech("Etsikää keltaisia rasteja!", 5f));
        }
        else if (questline == 4)
        {
            speechCO = StartCoroutine(ShowFairySpeech("Etsikää punaisia rasteja!", 5f));
        }
    }

    public void FairySuggestQuestlineChange()
    {
        speechCO = StartCoroutine(ShowFairySpeech("Voitte vaihtaa tehtäväkokonaisuutta valikossa!", 30f));
    }

    public void StopSpeechCO()
    {
        if (COrunning)
        {
            StopCoroutine(speechCO);
            COrunning = false;
        }
    }

    //private void OnEnable()
    //{
    //    acm.currentFairyAudioSource = GetComponent<AudioSource>();
    //}

    private void OnDisable()
    {
        StopSpeechCO();
        localFairySpeechBubble.SetActive(false);
    }

    IEnumerator ShowFairySpeech(string inputText, float showDuration)
    {
        COrunning = true;
        localFairySpeechBubble.SetActive(true);
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;
        yield return new WaitForSeconds(showDuration);
        localFairySpeechBubble.SetActive(false);
        COrunning = false;
    }


    IEnumerator ShowFairySpeechWaitForInput(string inputText)
    {
        localFairySpeechBubble.SetActive(true);
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;

        yield return StartCoroutine(WaitForInput());

        localFairySpeechBubble.SetActive(false);
    }

    IEnumerator WaitForInput()
    {
        yield return new WaitForSeconds(1f);
        while (Input.touchCount == 0 && !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
    }
}
