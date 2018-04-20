using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyQuestLineScript : MonoBehaviour {

    public GameObject localFairy;
    public GameObject arTargets;
    public GameObject localFairySpeechBubble;
    public LocalFairyManager lfm;
    public GameEngine engine;
    public GameObject openMenu;
    public TouchScreenScript tss;
    public GameObject questSelectionCanvas;
    public AudioClipManagerScript acm;
    public AudioSource localFairySpeechAS;
    public AudioSource natureViewAS;
    public GameObject replayDialog;
    public AudioSource ambience;

    private void OnEnable()
    {
        engine.LoadGame();
        StartCoroutine(QuestTimeLine());
    }

    IEnumerator QuestTimeLine()
    {
        FindObjectOfType<ImageTargetManagerScript>().DisableAllTargets();
        localFairy.SetActive(true);

        openMenu.SetActive(false);

        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = localFairySpeechAS;

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(0).updateTimestamp) > 0) {

            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(2, true);

            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);

            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(false);
            transform.GetChild(6).gameObject.SetActive(false);
            transform.GetChild(7).gameObject.SetActive(false);
            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(11).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(10).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(0f,0f,255f,1f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[6];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Voi voipunutta luontoparkaa! Minun metsänikin nuokkuu uneliaana. Sillä ovat voimat ihan lopussa. Onneksi te voitte auttaa!", 1f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[7];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Sinisillä tehtävärasteilla voimme säästää energiaa, eli luonnon voimaa, että puut piristyisivät. Eiköhän ryhdytä hommiin? Aloitetaan ykkösestä!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(0, true);
        }
        else if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(1).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(20, true);

            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);

            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(true);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(false);

            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(11).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(10).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 0f, 255f, 0.7f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[8];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Uneliaisuus alkaa hälvetä! Kohti seuraavaa rastia!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(1, true);
        }
        else if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(2).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(20, true);

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);

            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);

            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(11).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(10).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 0f, 255f, 0.3f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[9];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Mahtavaa! Vielä yksi rasti, niin eiköhän luonto taas uhku tarmoa!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(2, true);
        }
        else
        {

            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(9, true);

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);

            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);

            transform.GetChild(8).gameObject.SetActive(true);
            transform.GetChild(9).gameObject.SetActive(true);
            transform.GetChild(11).gameObject.SetActive(true);

            ParticleSystem.MainModule ps = transform.GetChild(10).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 0f, 255f, 0f));

            natureViewAS.Play();

            ambience.Play();

            localFairySpeechAS.clip = acm.fairyDialoqueClips[55];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Te teitte sen! Kylläpä minun mieltäni ilahduttaa, että metsäni piristyi niin kovasti!", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[56];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Nyt meidän ja Otson täytyy vain säästää lämmintä vettä, sammuttaa turhat valot ja käyttää ruokamme tarkkaan hyödyksi.", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[57];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Jos nämä muistamme, niin autamme luontoa joka päivä!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            questSelectionCanvas.SetActive(true);

            ambience.Stop();
        }
        openMenu.SetActive(true);
        gameObject.SetActive(false);
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
