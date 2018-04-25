using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NatureQuestLineScript : MonoBehaviour {

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
        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, false, false, false, false, false, false, false);

        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = localFairySpeechAS;

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(4, true);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(3).updateTimestamp) > 0)
        {

            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);

            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(false);
            transform.GetChild(6).gameObject.SetActive(false);
            transform.GetChild(7).gameObject.SetActive(false);
            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(false);
            transform.GetChild(11).gameObject.SetActive(false);
            transform.GetChild(12).gameObject.SetActive(false);
            transform.GetChild(14).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(13).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 255f, 0f, 1f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[58];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Onpa minun metsäni apea ja harmaa! Liekö se painunut ihmisiltä unholaan? Minusta luonto on paras paikka maailmassa.", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[59];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Sen parissa tekee hyvää puuhata ja levähtääkin. Minäpä luulen, että saamme metsäni pian taas kukoistamaan, kun löydämme vihreitä rasteja! Aloitetaan ykkösestä!", 1f));

            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            //GameObject.Find("DialogueRooms").transform.GetChild(3).gameObject.SetActive(true);
            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(3, true);
        }
        else if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(4).updateTimestamp) > 0)
        {

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);

            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(false);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);

            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(false);
            transform.GetChild(11).gameObject.SetActive(false);
            transform.GetChild(12).gameObject.SetActive(false);
            transform.GetChild(14).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(13).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 255f, 0f, 0.7f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[60];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä hyvä! Luonto alkaa jo vehreytyä!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            //GameObject.Find("DialogueRooms").transform.GetChild(4).gameObject.SetActive(true);
            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(4, true);
        }
        else if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(5).updateTimestamp) > 0)
        {

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);

            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);
            transform.GetChild(8).gameObject.SetActive(true);

            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(false);
            transform.GetChild(11).gameObject.SetActive(false);
            transform.GetChild(12).gameObject.SetActive(false);
            transform.GetChild(14).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(13).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 255f, 0f, 0.3f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[61];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Aivan pian luonto taas viheriöi! Vielä vähän matkaa!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            //GameObject.Find("DialogueRooms").transform.GetChild(5).gameObject.SetActive(true);
            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(5, true);
        }
        else
        {
            //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, false, false, false, false, false, false, false);

            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(21, true);

            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            transform.GetChild(3).gameObject.SetActive(false);

            transform.GetChild(4).gameObject.SetActive(true);
            transform.GetChild(5).gameObject.SetActive(true);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(true);
            transform.GetChild(8).gameObject.SetActive(true);

            transform.GetChild(9).gameObject.SetActive(true);
            transform.GetChild(10).gameObject.SetActive(true);
            transform.GetChild(11).gameObject.SetActive(true);
            transform.GetChild(12).gameObject.SetActive(true);
            transform.GetChild(14).gameObject.SetActive(true);

            ParticleSystem.MainModule ps = transform.GetChild(13).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(0f, 255f, 0f, 0.0f));

            natureViewAS.Play();

            ambience.Play();

            localFairySpeechAS.clip = acm.fairyDialoqueClips[62];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Voi kuinka kauniilta ja kukoistavalta luonto nyt näyttää! Se taitaa olla iloinen siitä, että me arvostamme sitä ja viihdymme sen parissa.", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[63];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Pidetään siis huoli puutarhan kasveista, muistetaan kulkea metsässä kunnioittavasti ja hiljennytään välillä kuuntelemaan luontoa, jotta sekin viihtyy meidän kanssamme!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            questSelectionCanvas.GetComponent<QuestSelectionScript>().questLineSelected = 0;
            questSelectionCanvas.SetActive(true);

            //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, true, true, true, false, false, false, false, false, false);

            ambience.Stop();
        }

        //replayDialog.SetActive(false);
        //replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
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
