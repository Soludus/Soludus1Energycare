using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SocialQuestLineScript : MonoBehaviour {

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

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(15, true);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(9).updateTimestamp) > 0)
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
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(255f, 0f, 0f, 1f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[161];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Onpas luonto ihan tolaltaan! Täällä leijailee kiukkua ja murheita.", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[162];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Etsitään yhdessä punaisia rasteja, joilla parannamme kaikkien mieltä, niin luontokin ilahtuu! Aloitetaan ykkösestä!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(9, true);
        }
        else if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(10).updateTimestamp) > 0)
        {

            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(false);

            transform.GetChild(4).gameObject.SetActive(false);
            transform.GetChild(5).gameObject.SetActive(true);
            transform.GetChild(6).gameObject.SetActive(true);
            transform.GetChild(7).gameObject.SetActive(false);
            transform.GetChild(8).gameObject.SetActive(false);
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(false);
            transform.GetChild(11).gameObject.SetActive(false);
            transform.GetChild(12).gameObject.SetActive(false);
            transform.GetChild(14).gameObject.SetActive(false);

            ParticleSystem.MainModule ps = transform.GetChild(13).GetComponent<ParticleSystem>().main;
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(255f, 0f, 0f, 0.7f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[224];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä, Murheet alkavat haihtua! Jatketaan vaan!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(10, true);
        }
        else if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(11).updateTimestamp) > 0)
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
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(255f, 0f, 0f, 0.3f));

            localFairySpeechAS.clip = acm.fairyDialoqueClips[163];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Mainiota! Täällä puhaltaa jo selvästi parempi tuuli! Vähän vielä!", 1f));
            localFairySpeechAS.Stop();

            replayDialog.SetActive(false);

            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(11, true);
        }
        else
        {

            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(8, true);

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
            ps.startColor = new ParticleSystem.MinMaxGradient(new Color(255f, 0f, 0f, 0f));

            natureViewAS.Play();
            ambience.Play();

            localFairySpeechAS.clip = acm.fairyDialoqueClips[239];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("No johan se auttoi! Nyt meidän on vain muistettava kannustaa toisiamme, arvostaa omia vahvuuksiamme ja sitä, että meitä on monenlaisia.", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[240];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Silloin kaikilla pysyy hyvä mieli, ja aina löytyy auttajia ja leikkikavereita!", 1f));
            localFairySpeechAS.clip = acm.fairyDialoqueClips[225];
            localFairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Kiitos lapset, kun paransitte metsäni mieltä! Eiköhän pistetä jalalla koreasti!", 1f));

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
