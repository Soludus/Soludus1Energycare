using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelectionScript : MonoBehaviour {

    public GameObject arTargets;
    public int questLineSelected;
    public GameObject openMenuButton;
    public GameObject natureViews;
    public GameEngine GE;
    public GameObject localFairySpeechBubble;
    public LocalFairyManager lfm;
    public TouchScreenScript tss;
    public GameObject gameStarter;
    public AudioSource questLineAudio;
    public GameObject touchPanel;
    public GameObject[] winStars;
    public GameObject victoryButton;
    public GameObject questInfoCanvas;

    // Use this for initialization
    void Start () {

        questLineSelected = 0;
    }

    private void Awake()
    {
        GE.LoadGame();
    }

    private void OnDisable()
    {
        tss.touchScreenTouched = false;
        tss.allowInput = false;
    }

    private void OnEnable()
    {
        lfm.StopSpeechCO();
        gameStarter.GetComponents<AudioSource>()[1].mute = true;

        StartCoroutine(ShowFairySpeechWaitForInput("Valitkaa tehtäväkokonaisuus, mitä aloitamme tekemään!"));

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(0).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(1).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(2).updateTimestamp) < 0)
        {
            winStars[0].SetActive(true);
        }
        else winStars[0].SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(3).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(4).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(5).updateTimestamp) < 0)
        {
            winStars[1].SetActive(true);
        }
        else winStars[1].SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(6).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(7).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(8).updateTimestamp) < 0)
        {
            winStars[2].SetActive(true);
        }
        else winStars[2].SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(9).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(10).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(11).updateTimestamp) < 0)
        {
            winStars[3].SetActive(true);
        }
        else winStars[3].SetActive(false);


        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(2).updateTimestamp) < 0 && System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(5).updateTimestamp) < 0 &&
            System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(8).updateTimestamp) < 0 && System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(11).updateTimestamp) < 0) {

            victoryButton.SetActive(true);
        }
        else victoryButton.SetActive(false);
    }

    public void ChooseEnergyQuestLine()
    {
        questLineAudio.Play();
        questLineSelected = 1;
        openMenuButton.SetActive(true);
        questInfoCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ChooseNatureQuestLine()
    {
        questLineAudio.Play();
        questLineSelected = 2;
        openMenuButton.SetActive(true);
        questInfoCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ChoosePollutionQuestLine()
    {
        questLineAudio.Play();
        questLineSelected = 3;
        openMenuButton.SetActive(true);
        questInfoCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ChooseSocialQuestLine()
    {
        questLineAudio.Play();
        questLineSelected = 4;
        openMenuButton.SetActive(true);
        questInfoCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ChooseVictoryStage()
    {
        touchPanel.SetActive(true);
        questLineAudio.Play();
        questLineSelected = 5;
        openMenuButton.SetActive(true);
        natureViews.transform.GetChild(4).gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator ShowFairySpeech(string inputText, float showDuration)
    {

        localFairySpeechBubble.SetActive(true);
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;
        yield return new WaitForSeconds(showDuration);
        localFairySpeechBubble.SetActive(false);
    }

    IEnumerator ShowFairySpeechWaitForInput(string inputText)
    {
        localFairySpeechBubble.SetActive(true);
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;

        yield return StartCoroutine(WaitForInput());

        localFairySpeechBubble.SetActive(false);
    }

    IEnumerator WaitForInput()
    {
        tss.allowInput = false;
        yield return new WaitForSeconds(1f);
        tss.allowInput = true;

        while (!tss.touchScreenTouched)
        {
            yield return null;
        }
        tss.touchScreenTouched = false;
        tss.allowInput = false;
    }
}
