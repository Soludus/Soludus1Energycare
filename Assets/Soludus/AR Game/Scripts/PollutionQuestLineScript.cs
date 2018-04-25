using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollutionQuestLineScript : MonoBehaviour {

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

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(7, true);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(6).updateTimestamp) > 0)
        {

            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);

            transform.GetChild(6).gameObject.SetActive(false);
            transform.GetChild(7).gameObject.SetActive(false);
            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(false);
            transform.GetChild(12).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(11).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(255f, 255f, 0f, 1f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[99];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Voi, luonto tarvitsisi suursiivouksen! Sen päälle on kertynyt niin paljon roskaa ja saasteita, että se on aivan puserruksissa.", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[100];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Mutta jos me löydämme yhdessä keltaisia rasteja, voimme siivota ja puhdistaa luontoa! Aloitetaan ykkösestä!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(6, true);
        }
        else if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(7).updateTimestamp) > 0)
        {

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(false);

            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(false);
            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(false);
            transform.GetChild(12).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(11).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(255f, 255f, 0f, 0.7f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[101];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä! Saasteet alkavat haihtua! Eespäin!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(7, true);
        }
        else if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(8).updateTimestamp) > 0)
        {

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(false);

            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);
            transform.GetChild(8).gameObject.SetActive(true);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(false);
            transform.GetChild(12).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(11).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(255f, 255f, 0f, 0.3f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[223];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa, pian metsä on putipuhdas!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(8, true);
        }
        else
        {

            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(4, true);

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(false);

            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);
            transform.GetChild(8).gameObject.SetActive(true);
            transform.GetChild(9).gameObject.SetActive(true);
            transform.GetChild(10).gameObject.SetActive(true);
            transform.GetChild(12).gameObject.SetActive(true);

            ParticleSystem.MainModule ps = transform.GetChild(11).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(255f, 255f, 0f, 0.0f));

            natureViewAS.Play();
            ambience.Play();

            localFairySpeechAS.clip = acm.fairyDialoqueClips[102];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Näyttääpä luonto tyytyväiseltä ja puhtaalta nyt, kun olemme siivonneet niin paljon roskia ja saasteita sen yltä!", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[103];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Nyt meidän ja Otson täytyy vain muistaa lajitella jätteet, valita sopivin kulkupeli eri matkoille ja säästää paperia. Silloin autamme luontoa joka päivä!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            questSelectionCanvas.GetComponent<QuestSelectionScript>().questLineSelected = 0;
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
