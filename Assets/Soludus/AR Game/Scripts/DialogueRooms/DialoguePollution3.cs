using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePollution3 : BaseTargetScript
{
    public Animator bearAnim;

    public Animator trackableFairyAnim;

    public GameObject targetSprite;

    public GameObject replayDialog;

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
        //acm.currentFairyAudioSource = fairyAudioSource;
        //acm.currentBearAudioSource = bearAudioSource;

        localFairy.SetActive(false);

        trackableFairyAnim.SetTrigger("celebrate");
        bear.gameObject.SetActive(true);
        bearAnim.SetBool("sitting", true);
        bearAnim.Play("sittinganim");

        fairySpeechAS.clip = acm.fairyDialoqueClips[143];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso on kovasti innostunut kuvataidekerhosta ja alkanut haaveilla taiteilijan ammatista. Hän harjoittelee uutterasti piirtämistä ja maalaamista kotonakin.", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        bearAnim.SetBool("sitting", false);
        yield return new WaitForSeconds(1f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[144];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mutta hänhän piirsi paperista vain toisen puolen täyteen! Eihän se käy laatuun, että puolet jää käyttämättä! Otso!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        bearAnim.SetTrigger("wondering");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[145];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tiedätkös, Otso, että paperi tehdään puista? Sen valmistamiseksi täytyy kaataa metsää! Ja kaatamisen jälkeenkin pitää tehdä jos jonkinmoisia saastuttavia ja energiaa syöviä operaatioita, jotta saadaan paperia.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[146];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kannattaa siis käyttää paperi huolella ja piirtää molemmille puolille.", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[147];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Vanhat piirustukset, joita ei enää katsella, kuuluvat paperinkeräykseen. Niistä voidaan tehdä uutta paperia kaatamatta lisää puita.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[222];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kierrätyspaperin valmistaminen saastuttaa vähemmän ja vie vähemmän luonnon energiaa.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[148];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Paperia kannattaa säästää ja kierrättää siksikin, että kaatopaikalle joutuessaan se maatuu, jolloin siitä vapautuu haitallista kaasua.", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        bearAnim.SetTrigger("wondering");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[149];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Muistattekos te, lapset, piirtää paperin molemmille puolille? Mitä te olette viimeksi piirtäneet?", 1f));
        fairySpeechAS.Stop();

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: PIIRRÄTTEKÖ TE MOLEMMILLE PUOLILLE? MITÄ PIIRSITTE VIIMEKSI?", 5f));

        replayDialog.SetActive(false);
        bearAnim.SetBool("sitting", true);
        yield return new WaitForSeconds(1f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[150];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Osaatte varmasti piirtää hienoja kuvia, tekin taidatte olla aikamoisia taiteilijoita!", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[151];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso taitaa haluta piirtää lisää! Voisitteko auttaa häntä värittämään kuvan?", 1f));

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);

        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, false, false, false, true, false, false, false);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        //gameObject.SetActive(false);
        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(8, false);
    }
}
