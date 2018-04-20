using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePollution1 : BaseTargetScript
{
    public Animator bearAnim;

    private Vector3 bearOriginalPos;
    private Quaternion bearOriginalRot;

    public Animator trackableFairyAnim;

    public GameObject saladBall;

    public TrashBioManagerScript trashManager;

    public bool trashDragged;

    Coroutine speechCO;

    public AudioSource trashAS;

    public GameObject targetSprite;

    bool setInitReferences = true;

    public GameObject replayDialog;

    private void OnEnable()
    {
        if (setInitReferences)
        {
            bear.SetActive(true);
            bearOriginalPos = bear.transform.position;
            bearOriginalRot = bear.transform.rotation;
            setInitReferences = false;
        }
        StartCoroutine(StartTimeLine());
    }

    IEnumerator StartTimeLine()
    {
        if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
        {
            dataActionController.RunAction(dataAction[0]);
        }
        trackableFairySpeechBubble.SetActive(false);
        targetSprite.SetActive(false);
        //acm.currentFairyAudioSource = fairyAudioSource;
        //acm.currentBearAudioSource = bearAudioSource;
        bear.transform.position = bearOriginalPos;
        bear.transform.rotation = bearOriginalRot;

        localFairy.SetActive(false);

        saladBall.SetActive(true);
        bearAnim.Play("pickupidleanim");
        bearAnim.SetTrigger("eat");
        yield return new WaitForSeconds(1.5f);
        trackableFairyAnim.SetTrigger("idleTurn");
        yield return new WaitForSeconds(3.5f);

        fairySpeechAS.clip = acm.fairyDialoqueClips[104];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Oi voi Otsoa!", 1f));

        saladBall.SetActive(false);

        bearAnim.Play("idleanim");
        bear.transform.LookAt(new Vector3(trackableFairy.transform.position.x, bear.transform.position.y, trackableFairy.transform.position.z));

        fairySpeechAS.clip = acm.fairyDialoqueClips[105];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso on heitellyt kaikki roskat samaan koppaan! Hänellä on lukuisia erilaisia roskakoreja, mutta hän ei vieläkään osaa lajitella eri sortin jätteitä niihin.", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        bearAnim.SetTrigger("headDown");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[106];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Ei se mitään, Otso, me autamme sinua!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        bearAnim.SetTrigger("wondering");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[107];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Roskat kannattaa aina lajitella, sillä niistä saadaan talteen hyödyllistä materiaalia ja energiaa uusien tarvikkeiden tekemiseen.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[108];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Jotkut jätteet voivat olla myös vaarallisia, jos ne joutuvat väärään paikkaan. Autamme luontoa lajittelemalla huolellisesti.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[109];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otetaanpa muutama esimerkki, ja harjoitellaan samalla roskien viemistä oikeaan astiaan!", 1f));
        fairySpeechAS.Stop();

        trashManager.gameObject.SetActive(true);

        trashDragged = false;
        trashManager.ShowTrashAndBin(0);
        fairySpeechAS.clip = acm.fairyDialoqueClips[110];
        fairySpeechAS.Play();
        speechCO = StartCoroutine(ShowFairySpeech("Tällä tavalla merkittyyn muovinkeräykseen laitetaan esimerkiksi tyhjät jugurttipurkit, karkkipussit ja saippuapullot. Niistä voidaan kerätä talteen energiaa.", 30f));

        yield return StartCoroutine(WaitForTrashDrag());
        replayDialog.SetActive(false);
        trashAS.clip = acm.pollutionRoom1Clips[7];
        trashAS.Play();
        fairySpeechAS.Stop();
        trashManager.HideTrashAndBin(0);
        StopCoroutine(speechCO);
        trackableFairySpeechBubble.SetActive(false);
        yield return new WaitForSeconds(1f);

        trashDragged = false;
        trashManager.ShowTrashAndBin(1);
        fairySpeechAS.clip = acm.fairyDialoqueClips[111];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        speechCO = StartCoroutine(ShowFairySpeech("Tämä taas on biojätteen merkki. Sinne laitetaan esimerkiksi kaikki vihanneksista jäänyt jäte ja erilaiset kiinteät ruuantähteet kuten banaaninkuoret ja tyhjäksikalutut maissintähkät.", 30f));

        yield return StartCoroutine(WaitForTrashDrag());
        replayDialog.SetActive(false);
        trashAS.clip = acm.pollutionRoom1Clips[8];
        trashAS.Play();
        fairySpeechAS.Stop();
        trashManager.HideTrashAndBin(1);
        StopCoroutine(speechCO);
        trackableFairySpeechBubble.SetActive(false);
        yield return new WaitForSeconds(1f);

        trashDragged = false;
        trashManager.ShowTrashAndBin(2);
        fairySpeechAS.clip = acm.fairyDialoqueClips[112];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        speechCO = StartCoroutine(ShowFairySpeech("Otsolle tulee postissa Kontiosanomat ja välillä muutakin postia. Kun Otso on lukenut postit, hänen pitäisi laittaa lehdet ja kirjekuoret paperinkeräykseen. Ystävien kirjeet on toki mukava säästääkin!", 30f));

        yield return StartCoroutine(WaitForTrashDrag());
        replayDialog.SetActive(false);
        trashAS.clip = acm.pollutionRoom1Clips[3];
        trashAS.Play();
        fairySpeechAS.Stop();
        trashManager.HideTrashAndBin(2);
        StopCoroutine(speechCO);
        trackableFairySpeechBubble.SetActive(false);
        yield return new WaitForSeconds(1f);

        trashDragged = false;
        trashManager.ShowTrashAndBin(3);
        fairySpeechAS.clip = acm.fairyDialoqueClips[113];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        speechCO = StartCoroutine(ShowFairySpeech("Otso pitää kovasti sardiineista. Niiden metalliset tölkit menevät huuhdeltuina metallinkeräykseen. Tänne kuuluvat myös lasisten säilykepurkkien metallikannet.", 30f));

        yield return StartCoroutine(WaitForTrashDrag());
        replayDialog.SetActive(false);
        trashAS.clip = acm.pollutionRoom1Clips[4];
        trashAS.Play();
        fairySpeechAS.Stop();
        trashManager.HideTrashAndBin(3);
        StopCoroutine(speechCO);
        trackableFairySpeechBubble.SetActive(false);
        yield return new WaitForSeconds(1f);

        trashDragged = false;
        trashManager.ShowTrashAndBin(4);
        fairySpeechAS.clip = acm.fairyDialoqueClips[114];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        speechCO = StartCoroutine(ShowFairySpeech("Jotkut roskat ovat vaarallisia, jos ne joutuvat luontoon tai kaatopaikalle. Niille on osoitettu aivan erityinen vaarallisen jätteen keräys. Tänne kuuluvat esimerkiksi paristot ja energiansäästölamput.", 30f));

        yield return StartCoroutine(WaitForTrashDrag());
        replayDialog.SetActive(false);
        trashAS.clip = acm.pollutionRoom1Clips[5];
        trashAS.Play();
        fairySpeechAS.Stop();
        trashManager.HideTrashAndBin(4);
        StopCoroutine(speechCO);
        trackableFairySpeechBubble.SetActive(false);
        replayDialog.SetActive(false);
        yield return new WaitForSeconds(1f);
        replayDialog.SetActive(true);

        trashManager.gameObject.SetActive(false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[115];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Jos roska ei kuulu mihinkään noista mainitsemistani ryhmistä, sen voi laittaa sekajätteeseen.", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[227];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tutkikaapa, millaisia keräysastioita teillä siinä lähistöllä on?", 1f));
        fairySpeechAS.Stop();

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("TUTKIKAA YMPÄRISTÖÄNNE: MILLAISIA KIERRÄTYSASTIOITA TEILLÄ ON?", 5f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[228];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso taisi mennä vähän pyörälle päästään! Auttakaa te Otsoa lajittelemaan roskat!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);

        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, false, true, false, false, false, false, false);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        //gameObject.SetActive(false);
        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(6, false);
    }

    IEnumerator WaitForTrashDrag()
    {
        yield return new WaitUntil(() => trashDragged);
    }
}
