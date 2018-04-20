using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryQuestLineScript : MonoBehaviour {

    public GameObject localFairy;
    public GameObject localFairySpeechBubble;
    public Animator localFairyAnim;
    public Animator bearAnim;
    public Animator pandaAnim;
    public LocalFairyManager lfm;
    public TouchScreenScript tss;
    public AudioClipManagerScript acm;
    public AudioSource localFairySpeechAS;
    public AudioSource natureViewAS;
    public GameObject replayDialog;

    private void OnEnable()
    {
        StartCoroutine(VictoryQuestLine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
    }

    IEnumerator VictoryQuestLine()
    {
        FindObjectOfType<ImageTargetManagerScript>().DisableAllTargets();
        localFairy.SetActive(true);

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(29, true);

        natureViewAS.Play();

        localFairyAnim.SetTrigger("celebrate");
        StartCoroutine(BearCelebrate(bearAnim));
        StartCoroutine(BearCelebrate(pandaAnim));

        localFairySpeechAS.clip = acm.fairyDialoqueClips[219];
        localFairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = localFairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kiitos avusta lapset! Yhteiset yrityksemme auttoivat. Luonto on nyt siistimpi ja energisempi, eikä se koe enää oloaan yksinäiseksi tai unohdetuksi.", 1f));
        localFairySpeechAS.clip = acm.fairyDialoqueClips[220];
        localFairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otsokin on samalla oppinut paljon siitä, miten voimme jatkossakin pitää luonnosta huolta! Eiköhän juhlita ja tuuleteta yhdessä, olemme kaikki tehneet hienoa työtä!", 1f));
        localFairySpeechAS.Stop();

        replayDialog.SetActive(false);

        localFairyAnim.SetTrigger("celebrate");
    }

    IEnumerator BearCelebrate(Animator bear)
    {

        while (true)
        {
            bear.SetTrigger("celebrate");
            yield return new WaitForSeconds(Random.Range(3, 7));
        }
    }

    IEnumerator ShowFairySpeech(string inputText, float showDuration)
    {

        localFairySpeechBubble.SetActive(true);
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;
        yield return new WaitForSeconds(showDuration);
        localFairySpeechBubble.SetActive(false);
    }


    IEnumerator ShowFairySpeechWaitForInput(string inputText, float waitTime)
    {
        localFairySpeechBubble.SetActive(true);
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;

        yield return StartCoroutine(WaitForInput(waitTime));

        localFairySpeechBubble.SetActive(false);
    }

    IEnumerator WaitForInput(float waitTime)
    {
        tss.allowInput = false;
        yield return new WaitForSeconds(waitTime);
        tss.allowInput = true;

        while (!tss.touchScreenTouched)
        {
            yield return null;
        }
        tss.touchScreenTouched = false;
        tss.allowInput = false;
    }
}
