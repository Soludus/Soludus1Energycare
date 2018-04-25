using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{
    public string filePath = "GamePicture.png";
    public GameObject openMenuButton;
    public GameObject screenshotPreview;
    public AuringonKukkaTarget auringonKukkaTarget;
    public MetsanTargetScript metsanTarget;
    private int levelNumber = 0;
    private Texture2D _tex;

    [Header("Buttons")]
    public GameObject takePicButton = null;
    public GameObject cancelButton = null;
    public GameObject acceptButton = null;
    public GameObject takeNewPicButton = null;

    public void TakeScreenShot()
    {
        StartCoroutine(SaveScreenShot());
    }

    public void SetLevelNumber(int number)
    {
        levelNumber = number;
    }

    private IEnumerator SaveScreenShot()
    {
        _tex = new Texture2D(Screen.width, Screen.height);

        takePicButton.SetActive(false);
        cancelButton.SetActive(false);
        openMenuButton.SetActive(false);

        //yield return new WaitUntil(() => File.Exists(Application.persistentDataPath + "/" + filePath) != true);

        string path = filePath;
        if (!Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + "/" + filePath;
        }

        ScreenCapture.CaptureScreenshot(path);

        yield return StartCoroutine(DrawImageOnScreen());

        openMenuButton.SetActive(true);
        acceptButton.SetActive(true);
        takeNewPicButton.SetActive(true);
    }

    public void CancelScreenShot()
    {

        gameObject.SetActive(false);
    }

    public void AcceptScreenShot()
    {
        takePicButton.SetActive(true);
        //cancelButton.SetActive(true);
        acceptButton.SetActive(false);
        takeNewPicButton.SetActive(false);

        screenshotPreview.SetActive(false);
        gameObject.SetActive(false);

        if (levelNumber == 1)
        {

            auringonKukkaTarget.screenshotTaken = true;
            auringonKukkaTarget.questState = 1;
            auringonKukkaTarget.tex = _tex;
            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(3, false);
        }
        else if (levelNumber == 2)
        {
            metsanTarget.screenshotTaken = true;
            metsanTarget.questState = 1;
            metsanTarget.tex = _tex;
            FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(4, false);
        }
    }

    public void TakeNewScreenShot()
    {
        if (File.Exists(Application.persistentDataPath + "/" + filePath))
        {
            File.Delete(Application.persistentDataPath + "/" + filePath);
        }

        takePicButton.SetActive(true);
        //cancelButton.SetActive(true);
        acceptButton.SetActive(false);
        takeNewPicButton.SetActive(false);
        screenshotPreview.SetActive(false);
    }

    private IEnumerator DrawImageOnScreen()
    {
        yield return new WaitForEndOfFrame();

        _tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _tex.Apply();

        screenshotPreview.SetActive(true);
        screenshotPreview.GetComponent<SpriteRenderer>().sprite = Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), new Vector2(0.5f, 0.5f));
    }
}
