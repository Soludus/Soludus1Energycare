using UnityEngine;
using UnityEngine.UI;

public class QuestInformation : MonoBehaviour
{
    public int showingQuestInfo = 0;
    public GameObject questSelectionCanvas;
    public Text infoText;

    private QuestSelectionScript qss;

    private void Awake()
    {
        qss = questSelectionCanvas.GetComponent<QuestSelectionScript>();
    }

    private void OnEnable()
    {
        if (showingQuestInfo == 1)
        {
            infoText.text = "Tehtävä aiheet: \n 1. Vedenkulutus \n 2. Valojen sammuttaminen \n 3. Ruuan säästäminen \nSuositeltu suorituspaikka: sisätilat";
        }
        else if (showingQuestInfo == 2)
        {
            infoText.text = "Tehtävä aiheet: \n 1. Kasvien kasvatus \n 2. Luonnon kunnioittaminen \n 3. Luonnon kuuntelu \nSuositeltu suorituspaikka: ulkona";
        }
        else if (showingQuestInfo == 3)
        {
            infoText.text = "Tehtävä aiheet: \n 1. Roskien lajittelu \n 2. Liikkumisvälineet \n 3. Paperin säästäminen \nSuositeltu suorituspaikka: sisätilat";
        }
        else if (showingQuestInfo == 4)
        {
            infoText.text = "Tehtävä aiheet: \n 1. Toisten kannustaminen \n 2. Erilaisuus \n 3. Missä olen hyvä? \nSuositeltu suorituspaikka: sisätilat";
        }
    }

    public void AcceptQuestLine()
    {
        qss.AcceptQuestLine(showingQuestInfo);
        gameObject.SetActive(false);
    }

    public void ReturnToSelection()
    {
        qss.questLineAudio.Play();
        questSelectionCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
