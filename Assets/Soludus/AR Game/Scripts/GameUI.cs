using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameUI : MonoBehaviour {

	public Transform scoreBoard, mainBoard, openMenuButton, electricityBoard;

	public GameEngine GE;

    public GameObject QuestSelecting;

    public AudioSource buttonPressAudio;

	private Coroutine turnFairyCo;

	[SerializeField] Transform localFairy = null;

    public GameObject touchPanel;

	public void Start(){
		turnFairyCo = null;
	}

    public void ChangeTargetsButton()
    {
        buttonPressAudio.Play();
        localFairy.gameObject.GetComponent<LocalFairyManager>().StopSpeechCO();
        GameObject.Find("NatureViews").transform.GetChild(4).gameObject.SetActive(false);

        mainBoard.gameObject.SetActive(false);
        openMenuButton.gameObject.SetActive(true);
        QuestSelecting.SetActive(true);

        if (turnFairyCo != null)
            StopCoroutine(turnFairyCo);
        turnFairyCo = StartCoroutine(TurnFairy(152));
    }

	public void ScoreBoardButton(){
        buttonPressAudio.Play();
        scoreBoard.gameObject.SetActive (true);
		mainBoard.gameObject.SetActive (false);
		scoreBoard.GetComponent<ScoreBoardLoader> ().Initialize ();
	}

	public void ReturnFromScoreBoardButton(){
        buttonPressAudio.Play();
        scoreBoard.gameObject.SetActive (false);
		mainBoard.gameObject.SetActive (true);
	}

	public void CloseMenuButton(){
        touchPanel.SetActive(true);
        buttonPressAudio.Play();
        mainBoard.gameObject.SetActive (false);
		openMenuButton.gameObject.SetActive (true);
		if (turnFairyCo != null)
			StopCoroutine (turnFairyCo);
		turnFairyCo = StartCoroutine (TurnFairy (152));

        if (QuestSelecting.GetComponent<QuestSelectionScript>().questLineSelected == 0)
        {
            QuestSelecting.SetActive(true);
        }
	}

	public void OpenMenuButton(){
        touchPanel.SetActive(false);
        buttonPressAudio.Play();
        QuestSelecting.SetActive(false);
        mainBoard.gameObject.SetActive (true);
		openMenuButton.gameObject.SetActive (false);
		if (turnFairyCo != null)
			StopCoroutine (turnFairyCo);
		turnFairyCo = StartCoroutine (TurnFairy (80));
	}

	public void ExitGameButton(){
        Application.Quit ();
	}

	public void OpenElectricityBoard(){
        buttonPressAudio.Play();
        mainBoard.gameObject.SetActive(false);
        electricityBoard.gameObject.SetActive (true);
	}

	public void CloseElectricityBoard(){
        buttonPressAudio.Play();
        electricityBoard.gameObject.SetActive (false);
        mainBoard.gameObject.SetActive(true);
    }

	public void ResetScoreButton(){
        buttonPressAudio.Play();
        GE.ResetGame ();
		scoreBoard.GetComponent<ScoreBoardLoader> ().EmptyStars ();
	}

    public void BackToMainMenuButton()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

	public IEnumerator TurnFairy(float y){
		Quaternion desRot = Quaternion.Euler (new Vector3 (0, y, 0));
		while (localFairy.rotation.eulerAngles.y != y) {
			localFairy.rotation = Quaternion.Lerp (localFairy.rotation, desRot, Time.deltaTime * 4f);

			yield return null;
		}
	}
}
