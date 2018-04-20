using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestInformation : MonoBehaviour
{

    public GameObject questSelectionCanvas;
    private QuestSelectionScript qss;
    public Text infoText;
    public GameObject natureViews;
    public AudioSource questLineAudio;
    public GameObject touchPanel;

    private void Awake()
    {
        qss = questSelectionCanvas.GetComponent<QuestSelectionScript>();
    }

    private void OnEnable()
    {
        if (qss.questLineSelected == 1)
        {
            infoText.text = "Tehtävä aiheet: \n 1. Vedenkulutus \n 2. Valojen sammuttaminen \n 3. Ruuan säästäminen \nSuositeltu suorituspaikka: sisätilat";
        }
        else if (qss.questLineSelected == 2)
        {
            infoText.text = "Tehtävä aiheet: \n 1. Kasvien kasvatus \n 2. Luonnon kunnioittaminen \n 3. Luonnon kuuntelu \nSuositeltu suorituspaikka: ulkona";
        }
        else if (qss.questLineSelected == 3)
        {
            infoText.text = "Tehtävä aiheet: \n 1. Roskien lajittelu \n 2. Liikkumisvälineet \n 3. Paperin säästäminen \nSuositeltu suorituspaikka: sisätilat";
        }
        else if (qss.questLineSelected == 4)
        {
            infoText.text = "Tehtävä aiheet: \n 1. Toisten kannustaminen \n 2. Erilaisuus \n 3. Missä olen hyvä? \nSuositeltu suorituspaikka: sisätilat";
        }
    }

    public void AcceptQuestLine()
    {
        questLineAudio.Play();

        if (qss.questLineSelected == 1)
        {
            natureViews.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (qss.questLineSelected == 2)
        {
            natureViews.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (qss.questLineSelected == 3)
        {
            natureViews.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (qss.questLineSelected == 4)
        {
            natureViews.transform.GetChild(3).gameObject.SetActive(true);
        }
        touchPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ReturnToSelection()
    {
        questLineAudio.Play();
        questSelectionCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

}
