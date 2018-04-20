using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSocial2 : BaseTargetScript
{
    private bool inputOn;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public GameObject panda;
    public Animator pandaAnim;

    private bool bearClicked = false;

    private int questState;

    public Camera localCamera;

    public Animator trackableFairyAnim;


    Vector3 bearOriginalPos;
    Quaternion bearOriginalRot;
    Vector3 pandaOriginalPos;
    Quaternion pandaOriginalRot;

    Coroutine speechCO;

    public GameObject drapesHiding;
    public GameObject drapesPeeking;
    public GameObject drapes;

    public GameObject targetSprite;

    bool setInitReferences = true;

    public GameObject replayDialog;

    void Update()
    {
        Vector3 inputVector;

        if (TouchScreenScript.GetTouch(out inputVector) && inputOn)
        {
            //Ray ray = Camera.main.ScreenPointToRay(inputVector);
            Ray ray = localCamera.ScreenPointToRay(inputVector);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, 1 << 8) && inputOn)
            {

                if (hit.collider.gameObject.name == "drapesHiding" && questState == 0)
                {
                    Debug.Log("bear is now clicked");
                    inputOn = false;
                    questState = 1;
                    bearClicked = true;

                }
            }
        }
    }

    private void OnEnable()
    {
        if (setInitReferences)
        {
            bear.SetActive(true);
            bearOriginalPos = bear.transform.position;
            bearOriginalRot = bear.transform.rotation;
            bear.SetActive(false);
            pandaOriginalPos = panda.transform.position;
            pandaOriginalRot = panda.transform.rotation;
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

        targetSprite.SetActive(false);
        //acm.currentFairyAudioSource = fairyAudioSource;
        //acm.currentBearAudioSource = bearAudioSource;

        drapes.SetActive(false);
        drapesHiding.SetActive(true);
        drapesPeeking.SetActive(false);
        bear.transform.position = bearOriginalPos;
        bear.transform.rotation = bearOriginalRot;
        panda.transform.position = pandaOriginalPos;
        panda.transform.rotation = pandaOriginalRot;

        bear.SetActive(false);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        panda.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[175];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tässä on tuttavani Panda, joka on tullut Suomeen vierailulle aina Kiinasta asti. Toin Pandan Otsoa tapaamaan, sillä he ovat kaukaisia sukulaisia. Mutta missäköhän Otso on?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        speechCO = StartCoroutine(ShowFairySpeech("ETSIKÄÄ OTSO!", 5f));

        inputOn = true;
        yield return new WaitUntil(() => bearClicked == true);
        inputOn = false;

        StopCoroutine(speechCO);

        drapesHiding.SetActive(false);
        drapesPeeking.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[176];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Siellähän sinä olet Otso! Tulepa nyt tervehtimään vierastasi!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        trackableFairyAnim.SetTrigger("idleTurn");
        yield return new WaitForSeconds(2.5f);

        pandaAnim.SetTrigger("headDown");
        yield return new WaitForSeconds(2f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[177];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otsoa taitaa jännittää Pandan tapaaminen. Jännittääkö sinuakin, Panda, tutustua Otsoon?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        pandaAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[178];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Teitä taitaa jännittää siksi, että ette aikaisemmin ole tavanneet toistenne kaltaisia karhuja.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[179];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mutta minusta on hurjan rohkeaa, että sinä, Panda, lähdit matkailemaan ja tutustumaan maailmaan! Otsoseni, sinun kannattaisi ottaa oppia Pandalta!", 1f));

        drapes.SetActive(true);
        drapesHiding.SetActive(false);
        drapesPeeking.SetActive(false);
        bear.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[180];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Meitä on moneen lähtöön. Otso on suomalainen ruskeakarhu ja Panda kiinalainen Pandakarhu, mutta te molemmat olette karhuja.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[181];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Minä taas olen metsänhaltija, mutta ette te minuakaan pelkää tai ujostele, vai mitä", 1f));
        fairySpeechAS.Stop();

        bear.transform.LookAt(panda.transform);
        panda.transform.LookAt(bear.transform);

        replayDialog.SetActive(false);
        yield return new WaitForSeconds(2f);

        bear.transform.LookAt(new Vector3(localCamera.transform.position.x, bear.transform.position.y, localCamera.transform.position.z));
        panda.transform.LookAt(new Vector3(localCamera.transform.position.x, panda.transform.position.y, localCamera.transform.position.z));

        yield return new WaitForSeconds(1f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[182];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Meillä on kaikilla erilaisia ominaisuuksia, taitoja, ulkonäköjä, uskontoja ja tapoja. Se voi ihmetyttää ja jännittääkin, mutta tekee myös tutustumisesta kiinnostavaa!", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[183];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Ja toisaalta meillä on myös monia yhtäläisyyksiä, joiden avulla ymmärrämme toisiamme. Millä lailla te lapset olette erilaisia ja samanlaisia keskenänne?", 1f));
        fairySpeechAS.Stop();
        //yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: MILLÄ LAILLA OLETTE ERILAISIA? ENTÄ SAMANLAISIA?", 5f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[184];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa! Minullekin tuli tässä vielä mieleen sellainen Otson ja Pandan yhtäläisyys, että molemmat tykkäävät leikkiä piilosta, vai mitä?", 1f));
        fairySpeechAS.Stop();

        bear.transform.LookAt(panda.transform);
        panda.transform.LookAt(bear.transform);

        replayDialog.SetActive(false);
        yield return new WaitForSeconds(1f);

        bearAnim.SetTrigger("nod");
        pandaAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);

        yield return new WaitForSeconds(1f);

        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[185];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Taidattekin jo haluta tutustua toisiinne, karhuystäväni! Eiköhän leikitä piilosta! Annetaan Pandan mennä tällä kertaa piiloon, ja sinä, Otso, saat etsiä!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);
        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, false, false, false, false, false, true, false);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        //gameObject.SetActive(false);
        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(10, false);
    }
}
