using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AuringonKukkaTarget : BaseTargetScript
{
    private bool completed;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public GameObject openMenu;
    public GameObject screenshotCanvas;
    public GameObject screenshotPreview;
    public GameObject screenshotPocket;

    public bool screenshotTaken = false;

    //string filePath = "GamePicture.png";

    public GameObject bugGameManager;

    private bool moveImageToPocket = false;
    float startTime;
    float timeToMove = 2f;

    public int questState;

    public Camera localCamera;

    public GameObject webCam;

    public GameObject star;

    public Animator trackableFairyAnim;

    Vector3 bearOriginalPos;
    Vector3 screenShotOriginalPos;

    private bool showNatureOnce;

    public Texture2D tex;

    float bugGameDuration;
    bool reduceDuration;

    public GameObject replayDialog;

    void Awake()
    {
        completed = false;

        // testing quest state
        //questState = 0;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();
        bearOriginalPos = bear.transform.position;
        screenShotOriginalPos = screenshotPocket.transform.position;
        startTime = 0f;
        showNatureOnce = false;
    }

    void Update()
    {
        if (questState == 4 && completed == false)
        {
            completed = true;

            GameObject[] bugs = GameObject.FindGameObjectsWithTag("bug");
            foreach (GameObject bug in bugs)
            {
                Destroy(bug);
            }

            engine.IncrementScore(3);

            star.SetActive(true);

            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");
            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
            StartCoroutine(VictorySpeech());
        }

        if (moveImageToPocket)
        {
            ImageToPocket();
        }
    }

    private void FixedUpdate()
    {
        if (reduceDuration == true)
        {
            bugGameDuration -= Time.deltaTime;
            if (bugGameDuration < 0)
                bugGameDuration = 0;
            Debug.Log(bugGameDuration);
        }
    }

    IEnumerator VictorySpeech()
    {
        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(27, false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[72];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Huh! Nyt Otson maa-artisokat ovat taas turvassa tuholaisilta! Kiitos lapset!", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    //IEnumerator StartTimeLine()
    //{
    //    trackableFairyAnim.SetTrigger("celebrate");
    //    bear.SetActive(true);
    //    yield return new WaitForSeconds(1f);
    //    bearAnim.SetTrigger("nod");
    //    yield return new WaitForSeconds(2f);

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[64];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Oi, Otsoseni! Onpa sinulla hienot, korkeat maa-artisokat! Olet tainnut hoitaa niitä hyvin!", 1f));
    //    fairySpeechAS.clip = acm.fairyDialoqueClips[65];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Puutarhanhoidosta onkin paljon iloa ja hyötyä! Se on mainiota liikuntaa, ja syötäviä kasveja kasvattamalla voi myös säästää luontoa!", 1f));
    //    fairySpeechAS.clip = acm.fairyDialoqueClips[66];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Itse kasvatettuja vihanneksia, hedelmiä ja marjoja ei tarvitse rahdata saastuttavilla kulkupeleillä kaukaa eikä käsitellä hyönteismyrkyin.", 1f));
    //    fairySpeechAS.clip = acm.fairyDialoqueClips[67];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Mitä kasveja te, lapset, olette itse kasvattaneet?", 1f));
    //    fairySpeechAS.Stop();

    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: MITÄ KASVEJA OLETTE ITSE KASVATTANEET?", 5f));

    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Olettepa te aikamoisia viherpeukaloita! Voisinko saada kasvimaastanne kuvan muistoksi? Kääntäkää kamera kasveja kohti ja napatkaa kuva, niin minä pistän sen taskuun, kun palaatte!", 1f));

    //    if (File.Exists(Application.persistentDataPath + "/" + filePath))
    //    {
    //        File.Delete(Application.persistentDataPath + "/" + filePath);
    //    }

    //    screenshotCanvas.SetActive(true);
    //}

    IEnumerator ScreenshotTakenQuestLine()
    {
        bear.SetActive(true);

        if (questState == 1)
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }

            trackableFairyAnim.SetTrigger("celebrate");


            startTime = 0f;
            screenshotPocket.SetActive(true);
            screenshotPocket.transform.position = screenShotOriginalPos;
            screenshotPocket.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            StartCoroutine(DrawImageOnScreen());
            moveImageToPocket = true;
            fairySpeechAS.clip = acm.fairyDialoqueClips[68];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Kiitos! Olettepa pitäneet kasveistanne hyvää huolta!", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[69];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("On hienoa nähdä työnsä tulokset, kun kasvit kasvavat. Samalla oppii niistä kaikenlaista. Tiesittekös esimerkiksi, että tämä Otson maa-artisokka on maatiaiskasvi, eli suorastaan historiallinen lajike...", 1f));

            bugGameManager.SetActive(true);
            bugGameManager.GetComponent<BugChaseScript>().SpawnFirstBug();

            bearAnim.SetTrigger("scare");

            fairySpeechAS.clip = acm.fairyDialoqueClips[70];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Mikäs nyt on, Otso-kulta?", 1f));

            bugGameManager.GetComponent<BugChaseScript>().SpawnSingleBug();

            fairySpeechAS.clip = acm.fairyDialoqueClips[71];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Voi ei! Tuhohyönteiset vaanivat sinun maa-artisokkiasi! Äkkiä, lapset, auttakaa Otsoa suojelemaan kasveja ja huitomaan hyönteiset pois!", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            StartCoroutine(ShowFairySpeech("KOSKETTAKAA HYÖNTEISIÄ JA PYYHKÄISKÄÄ NE POIS.", 5f));

            bugGameDuration = 30;
            questState = 2;

            yield return StartCoroutine(BugChaseGame(bugGameDuration));
        }
        else if (questState == 2)
        {
            StartCoroutine(ShowFairySpeech("KOSKETTAKAA HYÖNTEISIÄ JA PYYHKÄISKÄÄ NE POIS.", 5f));

            yield return StartCoroutine(BugChaseGame(bugGameDuration));
        }
        else if (questState == 3)
        {
            AfterBugGameQuestLine();
        }
    }

    void AfterBugGameQuestLine()
    {
        StartCoroutine(IncQuestStateAfterDelay());
    }

    IEnumerator DrawImageOnScreen()
    {
        yield return new WaitForEndOfFrame();

        screenshotPocket.gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }

    void ImageToPocket()
    {
        Vector3 destination = trackableFairy.transform.position;
        destination.y = 0f;
        startTime += Time.deltaTime;
        screenshotPocket.transform.position = Vector3.Lerp(screenshotPocket.transform.position, destination, Time.deltaTime * 2f);
        if (screenshotPocket.transform.localScale.z > 0.01f)
        {
            screenshotPocket.transform.localScale -= Vector3.one * Time.deltaTime * 0.05f;
        }
        if (startTime >= timeToMove)
        {
            moveImageToPocket = false;
            screenshotPocket.SetActive(false);
        }
    }

    IEnumerator BugChaseGame(float gameTime)
    {
        if (gameTime > 0)
        {
            reduceDuration = true;

            Debug.Log("Game is on!");
            bugGameManager.SetActive(true);
            bugGameManager.GetComponent<BugChaseScript>().allowSpawn = true;
            bugGameManager.GetComponent<BugChaseScript>().gameIsActive = true;

            yield return new WaitForSeconds(gameTime);
            Debug.Log("Spawning ends...");
            bugGameManager.GetComponent<BugChaseScript>().gameIsActive = false;

            reduceDuration = false;


            yield return new WaitForSeconds(5f);
            Debug.Log("Game is over.");

            GameObject[] bugs = GameObject.FindGameObjectsWithTag("bug");
            foreach (GameObject bug in bugs)
            {
                Destroy(bug);
            }
        }

        questState = 3;

        bugGameManager.SetActive(false);
        AfterBugGameQuestLine();
    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState++;
    }

    private void OnEnable()
    {
        menu.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.GetComponent<LocalFairyManager>().StopAllCoroutines();
        localFairy.GetComponent<LocalFairyManager>().localFairySpeechBubble.SetActive(false);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.SetActive(true);
        bear.transform.position = bearOriginalPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bear.SetActive(false);

        engine.LoadGame();

        star.SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(3).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(28, true);

            Debug.Log("targetissa: " + questState + " " + screenshotTaken);

            if (screenshotTaken == false || completed == true)
            {
                screenshotTaken = false;
                completed = false;

                //StartCoroutine(StartTimeLine());

                questState = 0;
            }
            else if (screenshotTaken = true && questState > 0)
            {
                StartCoroutine(ScreenshotTakenQuestLine());
            }
        }
        else
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[73];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Otson maa-artisokat ovat turvassa! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));
        }
    }

    private void OnDisable()
    {
        reduceDuration = false;
        if (menu != null)
        {
            menu.SetActive(true);
        }
        if (localFairy != null)
        {
            localFairy.SetActive(true);
        }
        trackableFairySpeechBubble.SetActive(false);
        StopAllCoroutines();
        screenshotPocket.SetActive(false);
        bugGameManager.GetComponent<BugChaseScript>().gameIsActive = false;
        bugGameManager.SetActive(false);
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
        GameObject[] bugs = GameObject.FindGameObjectsWithTag("bug");
        foreach (GameObject bug in bugs)
        {
            Destroy(bug);
        }
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
