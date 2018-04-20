using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSocial3 : BaseTargetScript
{
    public Animator bearAnim;

    public Animator trackableFairyAnim;

    Vector3 fairyOriginalPos;

    Quaternion fairyOriginalRot;

    public GameObject targetSprite;

    public GameObject replayDialog;

    void Start()
    {
        fairyOriginalPos = trackableFairy.transform.position;
        fairyOriginalRot = trackableFairy.transform.rotation;
    }

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
        trackableFairy.SetActive(true);

        trackableFairyAnim.SetTrigger("celebrate");
        bear.SetActive(true);
        bearAnim.SetBool("thinking", true);
        bearAnim.Play("thinkinganim");

        fairySpeechAS.clip = acm.fairyDialoqueClips[235];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Heipä hei, Otsoseni!", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[236];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso?", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[237];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mikäköhän sen Otson on noin syviin mietteisiin saanut. Kysytäänkö?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        trackableFairyAnim.SetBool("walking", true);
        yield return StartCoroutine(WalkFairy(bear.transform.position - new Vector3(0.25f, 0, 0)));
        trackableFairyAnim.SetBool("walking", false);

        trackableFairyAnim.SetBool("speaking", true);
        yield return new WaitForSeconds(1f);
        trackableFairyAnim.SetBool("speaking", false);

        bearAnim.SetBool("thinking", false);
        bearAnim.SetTrigger("nod");
        yield return null;
        bearAnim.SetBool("thinking", true);

        yield return new WaitForSeconds(3f);

        trackableFairyAnim.SetBool("walking", true);
        yield return StartCoroutine(WalkFairy(fairyOriginalPos));
        trackableFairyAnim.SetBool("walking", false);

        trackableFairy.transform.rotation = fairyOriginalRot;

        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[202];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso yrittää pohtia, missä hän on hyvä! Kaikilla meillä on taitoja ja vahvuuksia, mutta niitä ei aina tunne ennen kuin kokeilee ja opettelee.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[203];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä ei myöskään aina tarkoita muita parempaa; moni voi olla hyvä samassa asiassa! Missä asioissa te olette hyviä tai haluaisitte oppia taitavaksi?", 1f));
        fairySpeechAS.Stop();

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: MISSÄ OLETTE HYVIÄ? MISSÄ HALUAISITTE OPPIA TAITAVIKSI!", 5f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[204];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Onpa teillä paljon taitoja! Kaikessa ei tietenkään tarvitsekaan olla hyvä, ja vaikka mitä voi opetella!", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[205];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso taitaa vielä pohtia omia vahvuuksiaan. Mutta kokeilemallahan se selviää, Otso! Autetaan Otsoa kokeilemaan erilaisia taitoja, niin saadaan hänelle selvyyttä tähän asiaan", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[206];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mitäpä kokeilisimme ensin? Hyppäämistä, piirtämistä vai muistamista?", 1f));

        fairySpeechAS.Stop();

        bearAnim.SetBool("thinking", false);

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);
        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, false, false, false, false, false, false, true);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        //gameObject.SetActive(false);
        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(11, false);
    }

    IEnumerator WalkFairy(Vector3 targetPosition)
    {
        Vector3 desPos = trackableFairy.transform.position;
        desPos.x = targetPosition.x;

        //Debug.Log(desPos);

        trackableFairy.transform.LookAt(desPos);

        Vector3 direction = (desPos - trackableFairy.transform.position).normalized;

        while (Vector3.Distance(trackableFairy.transform.position, desPos) > 0.1f * transform.localScale.x)
        {
            trackableFairy.transform.position += direction * Time.deltaTime * 0.3f * transform.localScale.x;
            yield return null;
        }
    }
}
