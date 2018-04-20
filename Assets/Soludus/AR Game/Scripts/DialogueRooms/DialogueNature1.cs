using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNature1 : BaseTargetScript
{
    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public GameObject screenshotCanvas;
    public GameObject screenshotPocket;

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

        trackableFairyAnim.SetTrigger("celebrate");
        bear.SetActive(true);
        yield return new WaitForSeconds(1f);
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);

        fairySpeechAS.clip = acm.fairyDialoqueClips[64];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Oi, Otsoseni! Onpa sinulla hienot, korkeat maa-artisokat! Olet tainnut hoitaa niitä hyvin!", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[65];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Puutarhanhoidosta onkin paljon iloa ja hyötyä! Se on mainiota liikuntaa, ja syötäviä kasveja kasvattamalla voi myös säästää luontoa!", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[66];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Itse kasvatettuja vihanneksia, hedelmiä ja marjoja ei tarvitse rahdata saastuttavilla kulkupeleillä kaukaa eikä käsitellä hyönteismyrkyin.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[67];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mitä kasveja te, lapset, olette itse kasvattaneet?", 1f));
        fairySpeechAS.Stop();

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: MITÄ KASVEJA OLETTE ITSE KASVATTANEET?", 5f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[238];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Olettepa te aikamoisia viherpeukaloita! Voisinko saada kasvimaastanne kuvan muistoksi? Kääntäkää kamera kasveja kohti ja napatkaa kuva, niin minä pistän sen taskuun, kun palaatte!", 1f));

        fairySpeechAS.Stop();

        //if (File.Exists(Application.persistentDataPath + "/" + filePath))
        //{
        //    File.Delete(Application.persistentDataPath + "/" + filePath);
        //}

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Ottakaa kuva kasvimaasta ja etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);

        // show image of the target

        screenshotCanvas.GetComponent<ScreenshotManager>().SetLevelNumber(1);
        screenshotCanvas.SetActive(true);
        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        gameObject.SetActive(false);
        enabled = false;
    }
}
