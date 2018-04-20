using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class RuokaTargetScript : BaseTargetScript
{
    private bool completed;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;
    public Transform bearHand;

    public bool textInput = false;
    string filePath = "/WastedFood.txt";
    string wastedFood;

    public GameObject textCanvas;
    public GameObject textArea;

    private int questState;

    public GameObject servingPlate;
    public PlateTaskManager plateTaskManager;
    public GameObject ruokaCanvas;
    public GameObject fullPlate;

    public GameObject table;
    public GameObject trashBin;
    public GameObject choppingBoard;

    public Camera localCamera;

    public GameObject star;

    public Animator trackableFairyAnim;

    public int leftoverDecision;

    Vector3 bearOriginalPos;

    private bool showNatureOnce;

    Coroutine speechCO;

    public GameObject replayDialog;

    void Awake()
    {
        completed = false; ;

        // testing quest state
        questState = 0;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();
        bearOriginalPos = bear.transform.position;

        showNatureOnce = false;
    }

    void Update()
    {
        if (questState == 4 && completed == false)
        {
            completed = true;

            engine.IncrementScore(2);

            star.SetActive(true);
            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");
            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
            StartCoroutine(VictorySpeech());
        }
    }

    IEnumerator VictorySpeech()
    {
        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(10, false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[54];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa! Nyt Otsokin varmasti osaa käyttää ruokaa säästäväisesti, ja energiaa säästyy!", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    void StartTimeLine()
    {
        choppingBoard.SetActive(false);
        bear.SetActive(true);

        if (questState == 0)
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }

            servingPlate.SetActive(true);
            plateTaskManager.RestartPlate();
            // OHJE: kootkaa otsolle monipuolinen ateria vetämällä ruuat lautaselle
            StartCoroutine(ShowFairySpeech("KOOTKAA OTSOLLE MONIPUOLINEN ATERIA VETÄMÄLLÄ RUUAT LAUTASELLE.", 3f));

            servingPlate.SetActive(true);
        }
        else
        {
            StartCoroutine(PlateTaskFinished());
        }
    }

    void WriteToFile()
    {
        // esim: 1.0kg, päiväys: 1.1.2017 12:30:00
        string foodInput = ruokaCanvas.GetComponentInChildren<InputField>().text + "kg, päivä: " + System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

        StreamWriter writer = new StreamWriter(Application.persistentDataPath + filePath, true);
        writer.WriteLine(foodInput);
        writer.Close();
    }

    void ReadFromFile()
    {
        string[] lines = File.ReadAllLines(Application.persistentDataPath + filePath);
        wastedFood = lines[lines.Length - 1];
        Debug.Log(wastedFood);
        textArea.GetComponent<Text>().text = wastedFood;
    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState++;
    }

    IEnumerator BearPickUp()
    {
        table.transform.GetChild(0).gameObject.SetActive(false);
        fullPlate.SetActive(true);

        bearAnim.SetTrigger("pickUp");

        yield return new WaitForSeconds(1f);

        fullPlate.transform.SetParent(bearHand);
        fullPlate.transform.localPosition = new Vector3(23f, 15.2f, -8.8f);
    }

    IEnumerator BearEats()
    {
        bearAnim.SetTrigger("eat");
        yield return new WaitForSeconds(4.5f);
    }

    IEnumerator BearPutDownPlate()
    {
        bearAnim.SetTrigger("drop");

        yield return new WaitForSeconds(1f);

        fullPlate.transform.SetParent(table.transform);
        fullPlate.transform.position = table.transform.GetChild(0).position;
        fullPlate.transform.rotation = table.transform.GetChild(0).rotation;
        fullPlate.SetActive(false);
        table.transform.GetChild(0).gameObject.SetActive(true);
    }

    public IEnumerator WalkBear(Vector3 pos, int action)
    {
        pos.y = bear.transform.position.y;

        // positioning for trashbin
        if (action == 1)
        {
            pos.x -= 0.25f;
            pos.z += 0.125f;
        }

        bearAnim.SetBool("walking", true);
        while (Vector3.Distance(bear.transform.position, pos) > 0.01f * transform.localScale.x)
        {
            bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            bear.transform.LookAt(pos);
            yield return null;
        }

        bearAnim.SetBool("walking", false);

        if (action == 1)
        {
            pos.x += 0.25f;
            pos.z -= 0.125f;
            bear.transform.LookAt(trackableFairy.transform.position);
        }
        else if (action == 2)
        {
            bear.transform.LookAt(trackableFairy.transform.position);
        }

        yield return new WaitForSeconds(2f);
        if (action == 1)
        {
            bear.transform.LookAt(trackableFairy.transform.position);
        }
    }

    public void StartPlateTaskFinished()
    {
        questState++;
        StartCoroutine(PlateTaskFinished());
    }

    public IEnumerator PlateTaskFinished()
    {
        servingPlate.SetActive(false);

        if (questState == 1)
        {
            trackableFairySpeechBubble.SetActive(false);

            yield return StartCoroutine(BearPickUp());
            yield return StartCoroutine(BearEats());
            yield return StartCoroutine(BearPutDownPlate());

            fairySpeechAS.clip = acm.fairyDialoqueClips[47];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;

            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä, Otso tuli täyteen! Auttakaa häntä vielä miettimään, mitä aterialta jääneistä tähteistä voi heittää pois ja mitä voi käyttää uudelleen!", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            servingPlate.SetActive(true);

            speechCO = StartCoroutine(ShowFairySpeech("VALITKAA KUVISTA: MENEVÄTKÖ TÄHTEET ROSKIIN VAI TALTEEN?", 3f));
        }
        else if (questState == 2)
        {
            servingPlate.SetActive(false);
            StopCoroutine(speechCO);

            if (leftoverDecision == 1)
            {
                fairySpeechAS.clip = acm.fairyDialoqueClips[48];
                fairySpeechAS.Play();
                replayDialog.SetActive(true);
                yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä! Otson omalle lautaselle jääneitä ruuantähteitä ei voi käyttää uudeksi ruuaksi, etenkään jos Otsolle sattuu vieraita seuraavallekin aterialle. Nämä voi laittaa biojätteeseen.", 1f));
                fairySpeechAS.clip = acm.fairyDialoqueClips[49];
                fairySpeechAS.Play();
                yield return StartCoroutine(ShowFairySpeechWaitForInput("Entäpä nämä kattilaan jääneet perunat, mitä tekisimme niille?", 1f));
                fairySpeechAS.Stop();

                replayDialog.SetActive(false);

                speechCO = StartCoroutine(ShowFairySpeech("VALITKAA KUVISTA: MENEVÄTKÖ TÄHTEET ROSKIIN VAI TALTEEN?", 3f));

                servingPlate.SetActive(true);
            }
            else if (leftoverDecision == 2)
            {
                fairySpeechAS.clip = acm.fairyDialoqueClips[50];
                fairySpeechAS.Play();
                replayDialog.SetActive(true);
                yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso voi laittaa lautasen jääkaappiin syödäkseen loput myöhemmin.", 1f));
                fairySpeechAS.clip = acm.fairyDialoqueClips[51];
                fairySpeechAS.Play();
                yield return StartCoroutine(ShowFairySpeechWaitForInput("Näitä tähteitä ei kannata käyttää enää uuteen ruokaan, etenkään, jos seuraavallekin aterialle tulee vieraita! Nämä voi siis heittää poiskin.", 1f));
                fairySpeechAS.clip = acm.fairyDialoqueClips[49];
                fairySpeechAS.Play();
                yield return StartCoroutine(ShowFairySpeechWaitForInput("Entäpä nämä kattilaan jääneet perunat, mitä tekisimme niille?", 1f));
                fairySpeechAS.Stop();

                replayDialog.SetActive(false);

                speechCO = StartCoroutine(ShowFairySpeech("VALITKAA KUVISTA: MENEVÄTKÖ TÄHTEET ROSKIIN VAI TALTEEN?", 3f));

                servingPlate.SetActive(true);
            }
        }
        else if (questState == 3)
        {
            StopCoroutine(speechCO);

            if (leftoverDecision == 3)
            {
                fairySpeechAS.clip = acm.fairyDialoqueClips[52];
                fairySpeechAS.Play();
                replayDialog.SetActive(true);
                yield return StartCoroutine(ShowFairySpeechWaitForInput("Ei hyviä perunoita kannata hukkaan heittää, ne voi laittaa jääkaappiin ja tehdä niistä huomenna vaikka maittavaa perunasalaattia! Pistetään talteen!", 1f));
                fairySpeechAS.Stop();
                replayDialog.SetActive(false);
            }
            else if (leftoverDecision == 4)
            {
                fairySpeechAS.clip = acm.fairyDialoqueClips[53];
                fairySpeechAS.Play();
                replayDialog.SetActive(true);
                yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä! Perunat kannattaa laittaa jääkaappiin ja käyttää ne seuraavana päivänä uuteen ruokaan, vaikkapa herkulliseen perunasalaattiin!", 1f));
                fairySpeechAS.Stop();
                replayDialog.SetActive(false);
                bearAnim.SetTrigger("celebrate");
                yield return new WaitForSeconds(1.5f);
            }

            StartCoroutine(IncQuestStateAfterDelay());
        }
    }

    private void OnEnable()
    {
        fullPlate.transform.SetParent(table.transform);
        fullPlate.transform.position = table.transform.GetChild(0).position;
        fullPlate.transform.rotation = table.transform.GetChild(0).rotation;
        fullPlate.gameObject.SetActive(false);

        trackableFairySpeechBubble.SetActive(false);

        textInput = false;
        bearAnim.SetBool("walking", false);
        bearAnim.Play("idleanim");
        bearAnim.Rebind();

        menu.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.SetActive(true);
        bear.transform.position = bearOriginalPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bear.SetActive(false);
        bearAnim.Play("idleanim");

        engine.LoadGame();

        star.SetActive(false);
        textCanvas.SetActive(true);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(2).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(12, true);

            completed = false;
            textArea.GetComponent<Text>().text = null;

            table.transform.GetChild(0).gameObject.SetActive(false);
            table.transform.GetChild(1).gameObject.SetActive(false);
            table.transform.GetChild(2).gameObject.SetActive(false);
            table.transform.GetChild(3).gameObject.SetActive(false);

            StartTimeLine();
        }
        else
        {
            StartCoroutine(ShowFairySpeech("Otso on jo kylläinen ja tähteetkin tallessa! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));
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

        textCanvas.SetActive(false);
        servingPlate.SetActive(false);
        ruokaCanvas.SetActive(false);
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        StopAllCoroutines();

        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    IEnumerator WaitForTextInput()
    {
        yield return new WaitUntil(() => textInput == true);
    }
}
