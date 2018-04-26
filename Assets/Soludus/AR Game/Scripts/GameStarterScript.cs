using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStarterScript : MonoBehaviour
{

    public GameObject localFairySpeechBubble;
    public GameObject questLineCanvas;
    public GameObject openMenu;
    public GameEngine GE;
    public GameObject ArTargets;
    public Animator localFairyAnim;
    public TouchScreenScript tss;
    public AudioClipManagerScript acm;
    public GameObject HelpCanvas;
    AudioSource[] sources;
    Coroutine helpCO;
    public GameObject replayDialog;
    public ImageTargetManagerScript itms;

    private void Awake()
    {
        GE.LoadGame();
        sources = localFairyAnim.gameObject.GetComponents<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        openMenu.SetActive(false);
        StartCoroutine(DeactivateImageTargets());

        if (GE.playerData.firstTimePlaying)
        {
            StartCoroutine(StartGameSpeech());
        }
        else
        {
            itms.DisableAllTargets();
            openMenu.SetActive(true);
            questLineCanvas.SetActive(true);
        }
    }

    IEnumerator DeactivateImageTargets()
    {
        yield return null;
        itms.DisableAllTargets();
    }

    IEnumerator StartGameSpeech()
    {
        yield return new WaitForSeconds(0.05f);
        itms.DisableAllTargets();
        yield return new WaitForSeconds(1.95f);

        localFairyAnim.SetTrigger("celebrate");

        yield return new WaitForSeconds(3f);

        helpCO = StartCoroutine(ActivateHelpCanvas(5f));
        sources[1].clip = acm.fairyDialoqueClips[0];
        sources[1].Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = sources[1];
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hei! Oivallista, että tulitte apuun!", 1f));
        StopCoroutine(helpCO);
        helpCO = StartCoroutine(ActivateHelpCanvas(12f));
        sources[1].clip = acm.fairyDialoqueClips[1];
        sources[1].Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Minä se olen metsänhaltija Hippa, ja olen kovasti yrittänyt helliä ja hoivata luontoa ja auttaa sitä voimaan hyvin, mutta siinä on kuulkaapa melkomoinen urakka!", 1f));
        StopCoroutine(helpCO);
        helpCO = StartCoroutine(ActivateHelpCanvas(5f));
        sources[1].clip = acm.fairyDialoqueClips[2];
        sources[1].Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Katsokaapa nyt vaikka tätä minun metsääni!", 1));

        replayDialog.SetActive(false);

        sources[1].Stop();

        transform.GetChild(0).gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        StopCoroutine(helpCO);
        helpCO = StartCoroutine(ActivateHelpCanvas(8f));
        sources[1].clip = acm.fairyDialoqueClips[3];
        sources[1].Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kuitenkin kaikitenkin... minä olen varma, että yhdessä saamme luonnon ja koko maapallon kukoistamaan.", 1f));
        StopCoroutine(helpCO);
        helpCO = StartCoroutine(ActivateHelpCanvas(8f));
        sources[1].clip = acm.fairyDialoqueClips[4];
        sources[1].Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Taidanpa viedä teidät tapaamaan ystävääni Otsoa, hänen luonaan voimme yhdessä pohtia tilannetta.", 1f));
        StopCoroutine(helpCO);
        helpCO = StartCoroutine(ActivateHelpCanvas(8f));
        sources[1].clip = acm.fairyDialoqueClips[5];
        sources[1].Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mutta jos haluatte auttaa, meidän on aivan ensimmäiseksi valittava mihin toimeen tartumme.", 1f));

        replayDialog.SetActive(false);

        StopCoroutine(helpCO);
        HelpCanvas.SetActive(false);
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        sources[1].Stop();

        transform.GetChild(0).gameObject.SetActive(false);

        GE.FirstTimePlayed();
        openMenu.SetActive(true);
        questLineCanvas.SetActive(true);
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

        yield return StartCoroutine(tss.WaitForInput(waitTime));
        HelpCanvas.SetActive(false);

        localFairySpeechBubble.SetActive(false);
    }

    IEnumerator ActivateHelpCanvas(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        HelpCanvas.SetActive(true);
    }
}
