using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenshotManager : MonoBehaviour
{

    string filePath = "GamePicture.png";
    Texture2D _tex;
    public GameObject openMenu;
    public GameObject screenshotPreview;
    public AuringonKukkaTarget auringonKukkaTarget;
    public MetsanTargetScript metsanTarget;
    private int levelNumber = 0;

    public void TakeScreenShot()
    {

        StartCoroutine(SaveScreenShot());
    }

    public void SetLevelNumber(int number)
    {
        levelNumber = number;
    }

    IEnumerator SaveScreenShot()
    {

        _tex = new Texture2D(Screen.width, Screen.height);

        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        openMenu.SetActive(false);

        //yield return new WaitUntil(() => File.Exists(Application.persistentDataPath + "/" + filePath) != true);

        string path = filePath;
        if (!Application.isMobilePlatform)
        {
            path = Application.persistentDataPath + filePath;
        }

        ScreenCapture.CaptureScreenshot(path);

        yield return StartCoroutine(DrawImageOnScreen());

        openMenu.SetActive(true);
        gameObject.transform.GetChild(2).gameObject.SetActive(true);
        gameObject.transform.GetChild(3).gameObject.SetActive(true);

    }

    public void CancelScreenShot()
    {

        gameObject.SetActive(false);
    }

    public void AcceptScreenShot()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
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

        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
        gameObject.transform.GetChild(3).gameObject.SetActive(false);
        screenshotPreview.SetActive(false);
    }

    IEnumerator DrawImageOnScreen()
    {

        yield return new WaitForEndOfFrame();

        _tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _tex.Apply();

        screenshotPreview.SetActive(true);
        screenshotPreview.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), new Vector2(0.5f, 0.5f));
    }
}
