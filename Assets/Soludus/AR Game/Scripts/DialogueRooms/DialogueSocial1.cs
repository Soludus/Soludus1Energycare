using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSocial1 : BaseTargetScript
{
    private bool inputOn;

    public Animator bearAnim;

    public GameObject drapes;
    public GameObject bearHiding;
    public GameObject bearPeeking;

    public GameObject kannustamisCanvas;

    private int questState;

    public Camera localCamera;

    public Animator trackableFairyAnim;

    Coroutine speechCO;

    public GameObject targetSprite;

    public GameObject replayDialog;

    void Update()
    {
        Vector3 inputVector;

        if (TouchScreenScript.GetTouch(out inputVector) && inputOn)
        {
            Ray ray = localCamera.ScreenPointToRay(inputVector);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, 1 << 8))
            {
                if (hit.collider.gameObject.name == "drapesHiding" && inputOn)
                {
                    questState = 1;
                    StartCoroutine(QuestStateTimeLine(questState, 0));

                    inputOn = false;
                }
            }
        }
    }

    private void OnEnable()
    {
        bearHiding.SetActive(true);
        bearPeeking.SetActive(false);
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

        inputOn = false;

        bear.SetActive(false);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);
        fairySpeechAS.clip = acm.fairyDialoqueClips[165];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa! Satuimmekin sopivasti näkemään Otson tanssiesityksen! Mutta missäköhän hän on, näettekö häntä?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        // OHJE: ETSIKÄÄ OTSO!
        speechCO = StartCoroutine(ShowFairySpeech("ETSIKÄÄ OTSO!", 5f));

        inputOn = true;
    }

    IEnumerator QuestStateTimeLine(int state, int cheerDecision)
    {
        if (state == 1)
        {
            StopCoroutine(speechCO);

            bearHiding.SetActive(false);
            bearPeeking.SetActive(true);

            fairySpeechAS.clip = acm.fairyDialoqueClips[166];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Voi, Otsoa taitaa jännittää!", 1f));

            fairySpeechAS.clip = acm.fairyDialoqueClips[167];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Minä taidan tietää, miten voimme auttaa Otsoa! Kaikkia jännittää joskus, mutta silloin voimme kannustaa ja rohkaista toisiamme.", 1f));
            fairySpeechAS.Stop();

            fairySpeechAS.clip = acm.fairyDialoqueClips[234];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Millä tavoin voisimme kannusta Otsoa? Miten te kannustatte toisianne?", 1f));
            fairySpeechAS.Stop();

            //yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: MITEN TOISIA VOI KANNUSTAA?", 5f));

            bearPeeking.SetActive(false);
            drapes.SetActive(true);
            bear.SetActive(true);

            fairySpeechAS.clip = acm.fairyDialoqueClips[168];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyviä ideoita! Minäkin mietin asiaa samalla, ja tuumin, että voisimme esimerkiksi tuulettaa, taputtaa ja vaikka halatakin, jos se vain toiselle sopii. Kannustetaanpa siis Otsoa yhdessä.", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);
            replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

            menu.SetActive(false);
            targetSprite.SetActive(true);
            yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
            menu.SetActive(true);
            targetSprite.SetActive(false);

            // show image of the target

            //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, false, false, false, false, true, false, false);

            localFairy.SetActive(true);
            //acm.currentBearAudioSource = null;
            //acm.currentFairyAudioSource = null;
            //gameObject.SetActive(false);
            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(9, false);
        }
    }
}
