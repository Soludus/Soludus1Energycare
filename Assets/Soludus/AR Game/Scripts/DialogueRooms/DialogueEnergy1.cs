using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEnergy1 : BaseTargetScript
{
    public Animator bearAnim;

    public bool inputOn;

    public GameObject[] resultImages;
    public GameObject sink;

    public GameObject buttonCanvas;

    public Animator hanaAnimator;

    public Animator trackableFairyAnim;

    public AudioSource localFairyAudioSource;

    public GameObject targetSprite;

    public GameObject replayDialog;

    public AudioSource sinkLoop;

    private void OnEnable()
    {
        StartCoroutine(StartTimeLine());
    }

    IEnumerator StartTimeLine()
    {
        if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
        {
            dataActionController.RunAction(dataAction[0]);
        }

        targetSprite.SetActive(false);

        localFairy.SetActive(false);
        HideResults();
        inputOn = false;
        bear.SetActive(true);
        BearLookAtPosition(sink.transform.position);
        hanaAnimator.SetBool("open", true);
        bearAnim.SetTrigger("handwash");
        trackableFairyAnim.SetTrigger("celebrate");

        sinkLoop.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[10];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Täällähän vesi virtaa kuin kosken kuohuissa ikään! Hienoa, että Otso muistaa pitää käpälät puhtaina, mutta käsienpesuun on olemassa myös konsti, jolla säästämme luonnon voimia!", 1));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        bearAnim.SetTrigger("pressButton");
        hanaAnimator.SetBool("open", false);
        yield return new WaitForSeconds(0.7f);
        sinkLoop.Stop();
        yield return new WaitForSeconds(0.8f);
        BearLookAtPosition(trackableFairy.transform.position);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[11];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Meillä täällä Suomessa riittää onneksi vettä peseytymiseen ja juomiseen, kaikkialla eivät ole asiat niin hyvin. Mutta meilläkin vesi pitää usein lämmittää, etteivät käpälät palele.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[12];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Veden lämmittäminen kuluttaa luonnon energiaa. Viemäriin valunut vesi täytyy myös puhdistaa ennen sen palauttamista luontoon. Siihenkin kuluu energiaa.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[13];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Siksi kädet kannattaakin pestä mahdollisimman paljon vettä säästäen. Minäpä näytän!", 1f));

        buttonCanvas.SetActive(true);

        soundEffectAS.clip = acm.energyRoom1Clips[4];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[14];
        fairySpeechAS.Play();
        resultImages[0].SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Laita ensin hana päälle.", 1f));

        soundEffectAS.clip = acm.energyRoom1Clips[9];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[15];
        fairySpeechAS.Play();
        resultImages[1].SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Huuhtele kädet.", 1f));

        soundEffectAS.clip = acm.energyRoom1Clips[6];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[16];
        fairySpeechAS.Play();
        resultImages[2].SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Sulje hana.", 1f));

        soundEffectAS.clip = acm.energyRoom1Clips[11];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[17];
        fairySpeechAS.Play();
        resultImages[3].SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Saippuoi kädet.", 1f));

        soundEffectAS.clip = acm.energyRoom1Clips[5];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[18];
        fairySpeechAS.Play();
        resultImages[4].SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Avaa hana.", 1f));

        soundEffectAS.clip = acm.energyRoom1Clips[9];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[19];
        fairySpeechAS.Play();
        resultImages[5].SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Pese kädet saippuasta.", 1f));

        soundEffectAS.clip = acm.energyRoom1Clips[7];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[20];
        fairySpeechAS.Play();
        resultImages[6].SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Sulje hana.", 1f));

        soundEffectAS.clip = acm.energyRoom1Clips[10];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[21];
        fairySpeechAS.Play();
        resultImages[7].SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kuivaa kädet.", 1f));

        soundEffectAS.clip = acm.energyRoom1Clips[12];
        soundEffectAS.Play();
        fairySpeechAS.clip = acm.fairyDialoqueClips[22];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä! Kokeilkaapa nyt itse. Mutta varokaa ettei minun ja Otson päälle loisku vettä!", 1f));
        fairySpeechAS.Stop();

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("KOKEILKAA ITSE PESTÄ KÄDET NIIN KUIN HIPPA NEUVOI.", 3f));

        replayDialog.SetActive(false);
        bearAnim.SetBool("thinking", true);
        yield return new WaitForSeconds(1f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[23];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso ei tainnut aivan vielä oppia, miten pestään kädet energiaa säästäen. Voisitteko opettaa hänelle oikean järjestyksen?", 1f));
        fairySpeechAS.Stop();

        HideResults();
        replayDialog.SetActive(false);
        bearAnim.SetBool("thinking", false);
        yield return new WaitForSeconds(1f);

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("OPETTAKAA OTSOLLE KÄSIENPESU KOSKETTAMALLA HANAA, SAIPPUAA JA PYYHETTÄ OIKEASSA JÄRJESTYKSESSÄ.", 3f));

        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;

        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(0, false);
    }

    void HideResults()
    {
        foreach (GameObject go in resultImages)
        {
            go.SetActive(false);
        }
    }

    void BearLookAtPosition(Vector3 pos)
    {
        pos.y = bear.transform.position.y;
        bear.transform.LookAt(pos);
    }

    IEnumerator InputCooldown(float waitTime)
    {
        inputOn = false;
        yield return new WaitForSeconds(waitTime);
        inputOn = true;
    }
}
