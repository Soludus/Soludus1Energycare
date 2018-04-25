using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelectionScript : MonoBehaviour
{
    public int questLineSelected;
    public GameObject arTargets;
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

    private void Start()
    {
        questLineSelected = 0;
    }

    private void Awake()
    {
        GE.LoadGame();
    }

    private void OnDisable()
    {
        tss.touchScreenTouched = false;
        tss.allowInput = true;
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "";
    }

    private void OnEnable()
    {
        lfm.StopSpeechCO();
        gameStarter.GetComponents<AudioSource>()[1].mute = true;

        StartCoroutine(ShowFairySpeechWaitForInput("Valitkaa tehtäväkokonaisuus, mitä aloitamme tekemään!"));

        DateTime dtNow = DateTime.Now;
        DateTime dtCooldown = dtNow.AddSeconds(-GE.CoolDownInSeconds);

        if (DateTime.Compare(dtCooldown, GE.GetScore(0).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(1).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(2).updateTimestamp) < 0)
        {
            winStars[0].SetActive(true);
        }
        else winStars[0].SetActive(false);

        if (DateTime.Compare(dtCooldown, GE.GetScore(3).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(4).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(5).updateTimestamp) < 0)
        {
            winStars[1].SetActive(true);
        }
        else winStars[1].SetActive(false);

        if (DateTime.Compare(dtCooldown, GE.GetScore(6).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(7).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(8).updateTimestamp) < 0)
        {
            winStars[2].SetActive(true);
        }
        else winStars[2].SetActive(false);

        if (DateTime.Compare(dtCooldown, GE.GetScore(9).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(10).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(11).updateTimestamp) < 0)
        {
            winStars[3].SetActive(true);
        }
        else winStars[3].SetActive(false);


        if (DateTime.Compare(dtCooldown, GE.GetScore(2).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(5).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(8).updateTimestamp) < 0 &&
            DateTime.Compare(dtCooldown, GE.GetScore(11).updateTimestamp) < 0)
        {
            victoryButton.SetActive(true);
        }
        else victoryButton.SetActive(false);
    }

    public void ChooseQuestLine(int line)
    {
        questLineAudio.Play();
        openMenuButton.SetActive(true);
        questInfoCanvas.GetComponent<QuestInformation>().showingQuestInfo = line;
        questInfoCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ChooseEnergyQuestLine()
    {
        ChooseQuestLine(1);
    }

    public void ChooseNatureQuestLine()
    {
        ChooseQuestLine(2);
    }

    public void ChoosePollutionQuestLine()
    {
        ChooseQuestLine(3);
    }

    public void ChooseSocialQuestLine()
    {
        ChooseQuestLine(4);
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

    public void AcceptQuestLine(int line)
    {
        questLineAudio.Play();
        questLineSelected = line;

        if (questLineSelected == 1)
        {
            natureViews.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (questLineSelected == 2)
        {
            natureViews.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (questLineSelected == 3)
        {
            natureViews.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (questLineSelected == 4)
        {
            natureViews.transform.GetChild(3).gameObject.SetActive(true);
        }
        touchPanel.SetActive(true);
    }

    private IEnumerator ShowFairySpeech(string inputText, float showDuration)
    {
        localFairySpeechBubble.SetActive(true);
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;
        yield return new WaitForSeconds(showDuration);
        localFairySpeechBubble.SetActive(false);
    }

    private IEnumerator ShowFairySpeechWaitForInput(string inputText)
    {
        localFairySpeechBubble.SetActive(true);
        localFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;

        yield return StartCoroutine(WaitForInput());

        localFairySpeechBubble.SetActive(false);
    }

    private IEnumerator WaitForInput()
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
