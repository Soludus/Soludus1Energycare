using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEnergy2 : BaseTargetScript
{
    public GameObject bearSpeechBubble;
    public Animator bearAnim;
    public Transform bearHand;

    public Animator trackableFairyAnim;

    public GameObject trash;

    Vector3 bearStartPos;
    Vector3 trashOriginalPos;
    Quaternion trashOriginalRot;

    public GameObject targetSprite;

    bool setInitRefernces = true;

    public GameObject replayDialog;

    private void OnEnable()
    {
        if (setInitRefernces)
        {
            bear.SetActive(true);
            bearStartPos = bear.transform.position;
            trashOriginalPos = trash.transform.position;
            trashOriginalRot = trash.transform.rotation;
            bear.SetActive(false);
            setInitRefernces = false;
        }

        StartCoroutine(StartTimeLine());
    }

    IEnumerator StartTimeLine()
    {
        if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
        {
            dataActionController.RunAction(dataAction[0]);
        }

        bear.transform.position = bearStartPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bearAnim.SetBool("sitting", false);
        trash.SetActive(true);
        trash.transform.SetParent(gameObject.transform);
        trash.transform.position = trashOriginalPos;
        trash.transform.rotation = trashOriginalRot;
        targetSprite.SetActive(false);
        //acm.currentFairyAudioSource = fairyAudioSource;
        //acm.currentBearAudioSource = bearAudioSource;

        localFairy.SetActive(false);
        bear.gameObject.SetActive(false);

        bearStartPos = bear.transform.position;
        trashOriginalPos = trash.transform.position;
        trashOriginalRot = trash.transform.rotation;
        trackableFairyAnim.SetTrigger("celebrate");

        fairySpeechAS.clip = acm.fairyDialoqueClips[26];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Onpas täällä hurjan valoisaa! Miksiköhän kaikki lamput palavat, vaikka Otso ei ole paikalla?", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[27];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Valojen pitäminen päällä kuluttaa luonnon energiaa, joten sitä ei kannata tehdä turhaan. Lamput kannattaa sammuttaa, jos lähtee pois huoneesta tai jos päivänvaloa on tarpeeksi.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[28];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Käykääpä tarkistamassa, ettei siellä teidän lähellänne ole turhaan jätetty lamppuja palamaan!", 1f));
        fairySpeechAS.Stop();

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("TUTKIKAA YMPÄRISTÖÄNNE: ONKO VALOJA JÄÄNYT TURHAAN PÄÄLLE? SAMMUTTAKAA TURHAT VALOT!", 3f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[29];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa, kiitos!", 1f));
        trackableFairyAnim.SetTrigger("celebrate");

        replayDialog.SetActive(false);
        yield return new WaitForSeconds(1f);
        bear.gameObject.SetActive(true);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[30];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Sieltäpä se Otso saapuukin!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        yield return StartCoroutine(MoveAndPick(trash));
        yield return StartCoroutine(WalkBearToPos(bearStartPos));
        bear.transform.LookAt(trackableFairy.transform.position);

        fairySpeechAS.clip = acm.fairyDialoqueClips[31];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kuuleppa nyt, hajamielinen mesikämmeneni!", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[32];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Ystävä kallis, sinä olet taas unohtanut sammuttaa valot lähtiessäsi!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        replayDialog.SetActive(false);
        trash.SetActive(false);
        bearAnim.Play("idleanim");
        bearAnim.SetTrigger("headDown");
        yield return new WaitForSeconds(2.5f);
        bearAnim.Play("pickupidleanim");
        trash.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(WalkBearToPos(bearStartPos + new Vector3(0f, 0f, 0.25f)));
        trash.SetActive(false);
        bearAnim.Play("idleanim");
        bearAnim.SetBool("sitting", true);
        yield return new WaitForSeconds(1.5f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[33];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tuo näyttää vaikealta. Auttaisitteko, lapset, Otsoa sammuttamaan valot katkaisijoista, ennen kuin hän lähtee viemään roskia?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
        bearAnim.SetBool("sitting", false);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(WalkBearToPos(bearStartPos));
        bear.transform.LookAt(trackableFairy.transform.position);

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);

        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, true, false, false, false, false, false, false, false, false, false, false);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(1, false);
    }

    public IEnumerator WalkBearToPos(Vector3 pos, float threshold = 0.01f)
    {
        bearAnim.SetBool("walking", true);
        while (Vector3.Distance(bear.transform.position, pos) > threshold * transform.localScale.x)
        {
            bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            bear.transform.LookAt(pos);
            yield return null;
        }

        bearAnim.SetBool("walking", false);
    }

    private IEnumerator MoveAndPick(GameObject go)
    {
        Vector3 desPos = go.transform.position;
        desPos.y = bear.transform.position.y;

        yield return StartCoroutine(WalkBearToPos(desPos, 0.1f));

        bearAnim.SetTrigger("pickUp");
        yield return new WaitForSeconds(1f);
        go.transform.SetParent(bearHand);
        go.transform.localPosition = new Vector3(0, 0.01f, -0.026f);

        yield return new WaitForSeconds(1f);
    }
}
