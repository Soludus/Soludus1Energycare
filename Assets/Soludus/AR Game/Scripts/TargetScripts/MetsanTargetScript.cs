using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MetsanTargetScript : BaseTargetScript
{
    private bool completed;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public int questState;

    public GameObject statueReadyButton;

    public GameObject localStatueConstruct;

    public GameObject statue;
    public GameObject statuePosition;
    GameObject spawnedStatue;

    public GameObject[] statueParts;

    public GameObject ant1;
    public GameObject ant2;
    public GameObject ant3;

    public GameObject openMenu;
    public GameObject screenshotCanvas;
    public GameObject screenshotPreview;
    public GameObject screenshotPocket;

    public bool screenshotTaken = false;

    //string filePath = "/GamePicture.png";

    private bool moveImageToPocket = false;
    float startTime;
    float timeToMove = 2f;
    Vector3 screenShotOriginalPos;

    public Camera localCamera;
    public GameObject star;

    public Animator trackableFairyAnim;

    Vector3 bearOriginalPos;

    private bool showNatureOnce;

    public Texture2D tex;

    public GameObject replayDialog;

    void Awake()
    {
        completed = false;

        //// testing quest state
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
        // level completed
        if (questState == 4 && completed == false)
        {
            completed = true;

            engine.IncrementScore(4);

            star.SetActive(true);

            ant1.SetActive(true);
            ant2.SetActive(true);
            ant3.SetActive(true);

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

    IEnumerator VictorySpeech()
    {
        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(27, false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[89];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Minä ainakin ilahduin, ja varmasti muukin metsänväki. Nyt voimme hyvillä mielin nauttia metsän rauhasta!", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    //IEnumerator StartTimeLine()
    //{
    //    trackableFairyAnim.SetTrigger("celebrate");
    //    bear.SetActive(true);

    //    yield return new WaitForSeconds(1f);

    //    bearAnim.SetTrigger("jump");
    //    yield return new WaitForSeconds(3f);

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[74];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Katsokaapas kuinka hyvällä tuulella Otso on! Onkos sinusta, Otso, luonnon keskellä ihanaa?", 1f));
    //    fairySpeechAS.Stop();

    //    bearAnim.SetTrigger("nod");
    //    yield return new WaitForSeconds(3f);

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[75];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Mutta muistathan sinä, että täällä on muitakin?", 1f));
    //    fairySpeechAS.Stop();

    //    bearAnim.SetTrigger("wondering");
    //    yield return new WaitForSeconds(3f);

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[76];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Metsässä asustaa monenmoisia eläimiä, lintuja, kasveja, sieniä ja hyönteisiä. Tämä on niille kaikille koti. Ja kun tullaan toisten kotiin vierailulle, pitää käyttäytyä kauniisti, vai mitä?", 1f));
    //    fairySpeechAS.Stop();

    //    bearAnim.SetTrigger("nod");
    //    yield return new WaitForSeconds(3f);

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[77];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Mikä teidän mielestänne, lapset, on metsässä parasta, ja miten siellä kuuluu käyttäytyä?", 1f));
    //    fairySpeechAS.Stop();

    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: MIKÄ ON METSÄSSÄ PARASTA? MITEN METSÄSSÄ TULEE KÄYTTÄYTYÄ?", 0.1f));

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[78];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Tehän osaattekin olla metsässä hyvin ja kunnioittaa ympäristöä! Luonto arvostaa, jos varomme vahingoittamasta tai sotkemasta sitä.", 1f));

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[79];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Metsä kuuluu meille kaikille. Siellä on oikeus liikkua ja yöpyäkin, sekä tietenkin poimia metsän antimia: marjoja, sieniä ja kukkia.", 1f));
    //    fairySpeechAS.clip = acm.fairyDialoqueClips[80];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Emme kuitenkaan saa häiritä muita ihmisiä tai eläimiä emmekä kasveja ja puita. Sammal ja jäkälä on jätettävä paikalleen, eikä metsää saa roskata.", 1f));

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[81];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Tiedättekös, kun minä olin nuori, metsissä asui myös haltijakansaa, johon itsekin kuulun. Siihen aikaan metsä oli pyhä paikka, jota kuului tervehtiä, kun sinne astui.", 1f));
    //    fairySpeechAS.clip = acm.fairyDialoqueClips[82];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Haltijat ja maahiset olivat mielissään lahjoista ja muista tervehdyksistä. Nykyisin haltijaväki taitaa pysyä visusti piilossa, mutta jotain taianomaista metsässä silti on, vai mitä?", 1f));
    //    fairySpeechAS.Stop();

    //    bearAnim.SetTrigger("nod");
    //    yield return new WaitForSeconds(3f);

    //    fairySpeechAS.clip = acm.fairyDialoqueClips[83];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Jos haluatte tervehtiä metsää, voitte vaikkapa rakentaa pienen taideteoksen luonnon omista tarvikkeista: kivistä, kävyistä ja maahan pudonneista oksista ja lehdistä.", 1f));
    //    fairySpeechAS.clip = acm.fairyDialoqueClips[84];
    //    fairySpeechAS.Play();
    //    yield return StartCoroutine(ShowFairySpeechWaitForInput("Tehkääpä se nyt, ja ottakaa sitten minulle kuva veistoksestanne!", 1f));
    //    fairySpeechAS.Stop();

    //    StartCoroutine(ShowFairySpeech("RAKENTAKAA OMA TAIDETEOS YMPÄRISTÖSTÄNNE LÖYTYVISTÄ MATERIAALEISTA JA OTTAKAA SIITÄ KUVA.", 5f));

    //    if (File.Exists(Application.persistentDataPath + "/" + filePath))
    //    {
    //        File.Delete(Application.persistentDataPath + "/" + filePath);
    //    }

    //    screenshotCanvas.SetActive(true);
    //}

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

    IEnumerator ScreenShotTakenQuestLine()
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

            fairySpeechAS.clip = acm.fairyDialoqueClips[85];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Tulipa kerrassaan hieno! Kiitos!", 1f));

            fairySpeechAS.clip = acm.fairyDialoqueClips[86];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Otsokin taitaa haluta rakentaa tervehdyksen metsänväelle. Tarvitsetko apua siinä, Otso?", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            bearAnim.SetTrigger("nod");
            yield return new WaitForSeconds(3f);

            fairySpeechAS.clip = acm.fairyDialoqueClips[87];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Auttaisitteko te lapset Otsoa?", 1f));

            fairySpeechAS.clip = acm.fairyDialoqueClips[221];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Voitte suunnitella teoksen itse, siirtäkää vain tarvikkeet keskelle ruutua haluamaanne asetelmaan.", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            //yield return StartCoroutine(ShowFairySpeechWaitForInput("AUTTAKAA OTSOA RAKENTAMAAN VEISTOS VETÄMÄLLÄ MATERIAALIT HALUAMAANNE ASETELMAAN.", 3f));

            localStatueConstruct.SetActive(true);

            questState = 2;

            yield return StartCoroutine(ShowFairySpeechWaitForInput("Aloitetaan viemällä iso kivi keskelle ruutua!", 1f));
        }
        else if (questState == 2)
        {
            yield return StartCoroutine(ShowFairySpeechWaitForInput("AUTTAKAA OTSOA RAKENTAMAAN VEISTOS VETÄMÄLLÄ MATERIAALIT HALUAMAANNE ASETELMAAN.", 1f));

            localStatueConstruct.SetActive(true);
        }
        else if (questState == 3)
        {
            StartCoroutine(EndTimeLine());
        }
    }

    public void StartEndTimeLine()
    {
        StartCoroutine(EndTimeLine());
    }

    IEnumerator EndTimeLine()
    {
        questState = 3;

        foreach (GameObject go in statueParts)
        {
            go.GetComponent<DragForestObjectScript>().enabled = false;
        }

        localStatueConstruct.SetActive(false);
        spawnedStatue = Instantiate(statue, statuePosition.transform.position, Quaternion.identity, statuePosition.transform);
        foreach (Transform t in spawnedStatue.transform)
        {
            t.gameObject.layer = 0;

            foreach (Transform tt in t)
            {
                tt.gameObject.layer = 0;
            }
        }
        spawnedStatue.layer = 0;

        yield return new WaitForSeconds(1f);

        fairySpeechAS.clip = acm.fairyDialoqueClips[88];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Oivallinen veistos!", 1f));
        fairySpeechAS.Stop();

        questState = 4;
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

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(4).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(28, true);

            if (screenshotTaken == false || completed == true)
            {
                screenshotTaken = false;
                completed = false;

                ant1.SetActive(false);
                ant2.SetActive(false);
                ant3.SetActive(false);

                Destroy(spawnedStatue);
                localStatueConstruct.SetActive(true);

                localStatueConstruct.SetActive(false);

                //StartCoroutine(StartTimeLine());

                questState = 0;
            }
            else if (screenshotTaken = true && questState > 0)
            {
                localStatueConstruct.SetActive(true);

                localStatueConstruct.SetActive(false);

                StartCoroutine(ScreenShotTakenQuestLine());
            }
        }
    }

    private void OnDisable()
    {
        if (menu != null)
        {
            menu.SetActive(true);
        }
        if (localFairy != null)
        {
            localFairy.SetActive(true);
        }
        localStatueConstruct.SetActive(false);
        trackableFairySpeechBubble.SetActive(false);
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
        StopAllCoroutines();
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    public void ResetTouchScreen()
    {
        tss.touchScreenTouched = false;
        tss.allowInput = false;
    }

    public void DragSpeech(string text, float time)
    {
        StartCoroutine(ShowFairySpeech(text, time));
    }
}
