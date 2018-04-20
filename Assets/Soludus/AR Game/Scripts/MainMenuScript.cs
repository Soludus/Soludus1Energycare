using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour {

    public Transform scoreBoard, mainBoard, resetPanel;

    public GameEngine GE;

    public AudioSource buttonPressAudio;

    public Text loadingText;

    bool allowSceneLoading;

    private void Start()
    {
        allowSceneLoading = true;
    }

    public void StartTheGameButton()
    {
        
        if (allowSceneLoading)
        {
            buttonPressAudio.Play();
            loadingText.gameObject.SetActive(true);
            SceneManager.LoadSceneAsync("GameScene");
        }
        allowSceneLoading = false;
    }

    public void ScoreBoardButton()
    {
        if (allowSceneLoading)
        {
            buttonPressAudio.Play();
            scoreBoard.gameObject.SetActive(true);
            mainBoard.gameObject.SetActive(false);
            scoreBoard.GetComponent<ScoreBoardLoader>().Initialize();
        }
    }

    public void ReturnFromScoreBoardButton()
    {
        buttonPressAudio.Play();
        scoreBoard.gameObject.SetActive(false);
        mainBoard.gameObject.SetActive(true);
    }

    public void ExitGameButton()
    {
        if (allowSceneLoading)
        {
            Application.Quit();
        }
    }

    public void OpenResetGamePanel()
    {
        if (allowSceneLoading)
        {
            buttonPressAudio.Play();
            resetPanel.gameObject.SetActive(true);
            mainBoard.gameObject.SetActive(false);
        }
    }

    public void CloseResetGamePanel()
    {
        buttonPressAudio.Play();
        mainBoard.gameObject.SetActive(true);
        resetPanel.gameObject.SetActive(false);
    }

    public void ResetScoreButton()
    {
        buttonPressAudio.Play();
        GE.ResetGame();
        scoreBoard.GetComponent<ScoreBoardLoader>().EmptyStars();
        if (File.Exists(Application.persistentDataPath + "/WastedFood.txt"))
        {
            File.Delete(Application.persistentDataPath + "/WastedFood.txt");
        }
        CloseResetGamePanel();
    }
}
