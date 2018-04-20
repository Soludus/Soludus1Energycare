using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNature2 : BaseTargetScript
{
    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public GameObject screenshotCanvas;

    //string filePath = "GamePicture.png";

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

        trackableFairySpeechBubble.SetActive(false);

        trackableFairyAnim.SetTrigger("celebrate");
        bear.SetActive(true);

        yield return new WaitForSeconds(1f);

        bearAnim.SetTrigger("jump");
        yield return new WaitForSeconds(3f);

        fairySpeechAS.clip = acm.fairyDialoqueClips[74];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Katsokaapas kuinka hyvällä tuulella Otso on! Onkos sinusta, Otso, luonnon keskellä ihanaa?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[75];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mutta muistathan sinä, että täällä on muitakin?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        bearAnim.SetTrigger("wondering");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[76];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Metsässä asustaa monenmoisia eläimiä, lintuja, kasveja, sieniä ja hyönteisiä. Tämä on niille kaikille koti. Ja kun tullaan toisten kotiin vierailulle, pitää käyttäytyä kauniisti, vai mitä?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[77];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mikä teidän mielestänne, lapset, on metsässä parasta, ja miten siellä kuuluu käyttäytyä?", 1f));
        fairySpeechAS.Stop();

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: MIKÄ ON METSÄSSÄ PARASTA? MITEN METSÄSSÄ TULEE KÄYTTÄYTYÄ?", 0.1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[78];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tehän osaattekin olla metsässä hyvin ja kunnioittaa ympäristöä! Luonto arvostaa, jos varomme vahingoittamasta tai sotkemasta sitä.", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[79];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Metsä kuuluu meille kaikille. Siellä on oikeus liikkua ja yöpyäkin, sekä tietenkin poimia metsän antimia: marjoja, sieniä ja kukkia.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[80];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Emme kuitenkaan saa häiritä muita ihmisiä tai eläimiä emmekä kasveja ja puita. Sammal ja jäkälä on jätettävä paikalleen, eikä metsää saa roskata.", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[81];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tiedättekös, kun minä olin nuori, metsissä asui myös haltijakansaa, johon itsekin kuulun. Siihen aikaan metsä oli pyhä paikka, jota kuului tervehtiä, kun sinne astui.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[82];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Haltijat ja maahiset olivat mielissään lahjoista ja muista tervehdyksistä. Nykyisin haltijaväki taitaa pysyä visusti piilossa, mutta jotain taianomaista metsässä silti on, vai mitä?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[83];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Jos haluatte tervehtiä metsää, voitte vaikkapa rakentaa pienen taideteoksen luonnon omista tarvikkeista: kivistä, kävyistä ja maahan pudonneista oksista ja lehdistä.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[84];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tehkääpä se nyt, ja ottakaa sitten minulle kuva veistoksestanne!", 1f));
        fairySpeechAS.Stop();

        //StartCoroutine(ShowFairySpeech("RAKENTAKAA OMA TAIDETEOS YMPÄRISTÖSTÄNNE LÖYTYVISTÄ MATERIAALEISTA JA OTTAKAA SIITÄ KUVA.", 5f));

        //if (File.Exists(Application.persistentDataPath + "/" + filePath))
        //{
        //    File.Delete(Application.persistentDataPath + "/" + filePath);
        //}

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Ottakaa kuva veistoksesta ja etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);

        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, true, false, false, false, false, false, false, false);

        screenshotCanvas.GetComponent<ScreenshotManager>().SetLevelNumber(2);
        screenshotCanvas.SetActive(true);
        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        gameObject.SetActive(false);

        enabled = false;
    }

    public void ResetTouchScreen()
    {
        tss.touchScreenTouched = false;
        tss.allowInput = false;
    }
}
