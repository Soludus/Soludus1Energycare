using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ErilaisuusTargetScript : BaseTargetScript
{
    private bool completed;
    private bool inputOn;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public GameObject panda;
    public Animator pandaAnim;

    private int randomSpot = 0;
    private int oldSpot = 0;
    private int olderSpot = 0;

    private int questState;

    public Camera localCamera;

    public GameObject star;

    public Animator trackableFairyAnim;

    public GameObject curtains;
    public GameObject curtainLeft;
    public GameObject curtainRight;

    Vector3 bearOriginalPos;
    Quaternion bearOriginalRot;
    Vector3 pandaOriginalPos;
    Quaternion pandaOriginalRot;
    Vector3 curtainLOriginalPos;
    Vector3 curtainROriginalPos;
    public float curtainSpeed;
    private Text curtainText;

    private bool showNatureOnce;

    Coroutine speechCO;

    public AudioSource localFairyAudioSource;

    void Awake()
    {
        completed = false;

        // testing quest state
        questState = 0;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();
        bearOriginalPos = bear.transform.position;
        pandaOriginalPos = panda.transform.position;
        bearOriginalRot = bear.transform.rotation;
        pandaOriginalRot = panda.transform.rotation;
        curtainLOriginalPos = curtainLeft.transform.position;
        curtainROriginalPos = curtainRight.transform.position;
        curtainText = curtains.transform.GetChild(0).GetChild(0).GetComponent<Text>();

        showNatureOnce = false;
    }

    void Update()
    {
        Vector3 inputVector;

        if (TouchScreenScript.GetTouch(out inputVector) && inputOn)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputVector);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && inputOn)
            {

                if (hit.collider.gameObject.name == "closetHiding" || hit.collider.gameObject.name == "drapesHiding" ||
                    hit.collider.gameObject.name == "carpetHiding" || hit.collider.gameObject.name == "bedHiding")
                {
                    Debug.Log(hit.collider.gameObject.name + " Panda was found here");
                    questState++;
                    StartCoroutine(QuestStateTimeLine(questState));
                }
            }
        }

        if (questState == 5 && completed == false)
        {
            completed = true;

            engine.IncrementScore(10);

            star.SetActive(true);

            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");
            pandaAnim.SetTrigger("celebrate");

            StartCoroutine(VictorySpeech());

            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
            panda.transform.LookAt(new Vector3(Camera.main.transform.position.x, panda.transform.position.y, Camera.main.transform.position.z));
        }
    }

    IEnumerator VictorySpeech()
    {
        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(14, false);
        fairySpeechAS.clip = acm.fairyDialoqueClips[190];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mainiota! Eikös ollutkin mukavaa leikkiä yhdessä! Pidetäänpä sen kunniaksi pienet juhlat!", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    IEnumerator StartTimeLine()
    {
        if (questState == 0)
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }
        }

        panda.SetActive(true);

        transform.GetChild(9).gameObject.SetActive(true);
        transform.GetChild(10).gameObject.SetActive(false);
        transform.GetChild(11).gameObject.SetActive(false);
        bear.SetActive(true);

        bear.transform.LookAt(panda.transform);
        panda.transform.LookAt(bear.transform);

        fairySpeechAS.clip = acm.fairyDialoqueClips[186];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Autetaan Otsoa laskemaan kymmeneen!", 1f));
        fairySpeechAS.Stop();

        yield return StartCoroutine(MoveCurtainsFront());

        yield return StartCoroutine(HidingCountDown());

        panda.SetActive(false);
        bear.transform.position = pandaOriginalPos;
        bear.transform.rotation = bearOriginalRot;
        bearAnim.SetBool("thinking", true);

        inputOn = true;
        Debug.Log("input on is now: " + inputOn);
        Debug.Log(questState);

        ChooseRandomHideoutForPanda();

        fairySpeechAS.clip = acm.fairyDialoqueClips[187];
        fairySpeechAS.Play();
        speechCO = StartCoroutine(ShowFairySpeech("ETSIKÄÄ PANDA!", 5f));

    }

    // chooses random location for panda to hide. If the randomed spot is the same as the previous one or one before it, location is changed. After that it checks again whether the spot has already been used and changes location if needed.
    void ChooseRandomHideoutForPanda()
    {
        ResetHidingSpots();

        olderSpot = oldSpot;
        oldSpot = randomSpot;

        randomSpot = Random.Range(1, 5);

        if (randomSpot == oldSpot && randomSpot < 4 || randomSpot == olderSpot && randomSpot < 4)
        {

            randomSpot++;

            if (randomSpot == oldSpot && randomSpot < 4 || randomSpot == olderSpot && randomSpot < 4)
            {

                randomSpot++;

            }
            else if (randomSpot == oldSpot && randomSpot == 4 || randomSpot == olderSpot && randomSpot == 4)
            {

                randomSpot = randomSpot - 2;

            }
        }
        else if (randomSpot == oldSpot && randomSpot == 4 || randomSpot == olderSpot && randomSpot == 4)
        {

            randomSpot--;

            if (randomSpot == oldSpot || randomSpot == olderSpot)
            {

                randomSpot--;

            }
        }

        if (randomSpot == 1)
        {

            // closet
            transform.GetChild(7).gameObject.SetActive(false);
            transform.GetChild(8).gameObject.SetActive(true);

        }
        else if (randomSpot == 2)
        {

            // drapes
            transform.GetChild(9).gameObject.SetActive(false);
            transform.GetChild(10).gameObject.SetActive(true);
            transform.GetChild(10).GetChild(1).gameObject.SetActive(false);
            transform.GetChild(10).GetChild(2).gameObject.SetActive(true);
            transform.GetChild(10).gameObject.SetActive(true);

        }
        else if (randomSpot == 3)
        {

            // carpet
            transform.GetChild(12).gameObject.SetActive(false);
            transform.GetChild(13).gameObject.SetActive(true);

        }
        else if (randomSpot == 4)
        {

            // bed
            transform.GetChild(14).gameObject.SetActive(false);
            transform.GetChild(15).gameObject.SetActive(true);
        }

        curtains.SetActive(false);
    }

    IEnumerator QuestStateTimeLine(int state)
    {
        StopCoroutine(speechCO);

        if (state == 1)
        {
            inputOn = false;
            ResetHidingSpots();
            panda.SetActive(true);
            panda.transform.position = bearOriginalPos;
            panda.transform.rotation = bearOriginalRot;
            fairySpeechAS.clip = acm.fairyDialoqueClips[188];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Johan löytyi! Haluatko, Panda, keksiä uuden piilon?", 1f));
            fairySpeechAS.Stop();

            pandaAnim.SetTrigger("nod");
            yield return new WaitForSeconds(3f);

            yield return StartCoroutine(MoveCurtainsFront());
            yield return StartCoroutine(HidingCountDown());
            panda.SetActive(false);
            inputOn = true;
            ChooseRandomHideoutForPanda();
            fairySpeechAS.clip = acm.fairyDialoqueClips[187];
            fairySpeechAS.Play();
            speechCO = StartCoroutine(ShowFairySpeech("ETSIKÄÄ PANDA!", 5f));
        }
        else if (state == 2)
        {
            inputOn = false;
            ResetHidingSpots();
            panda.SetActive(true);
            panda.transform.position = bearOriginalPos;
            fairySpeechAS.clip = acm.fairyDialoqueClips[189];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Oivallista! Kerta kiellon päälle!", 1f));
            fairySpeechAS.Stop();
            yield return StartCoroutine(MoveCurtainsFront());
            yield return StartCoroutine(HidingCountDown());
            panda.SetActive(false);
            inputOn = true;
            ChooseRandomHideoutForPanda();
            fairySpeechAS.clip = acm.fairyDialoqueClips[187];
            fairySpeechAS.Play();
            speechCO = StartCoroutine(ShowFairySpeech("ETSIKÄÄ PANDA!", 5f));
        }
        else if (state == 3)
        {
            inputOn = false;
            ResetHidingSpots();
            panda.SetActive(true);
            panda.transform.position = bearOriginalPos;
            bearAnim.SetBool("thinking", false);
            fairySpeechAS.Stop();
            questState++;
        }

        if (questState == 4)
        {

            ResetHidingSpots();

            panda.transform.position = bearOriginalPos;
            bearAnim.SetBool("thinking", false);
            panda.SetActive(true);
            panda.transform.rotation = pandaOriginalRot;
            StartCoroutine(IncQuestStateAfterDelay());
        }
    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState++;
    }

    private void OnEnable()
    {
        ResetHidingSpots();
        ResetStartingSpot();

        menu.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.transform.rotation = bearOriginalRot;
        panda.transform.rotation = pandaOriginalRot;

        bear.SetActive(true);
        bear.transform.position = bearOriginalPos;
        bearAnim.SetBool("thinking", false);
        bearAnim.Play("idleanim");
        bear.SetActive(false);

        panda.transform.position = pandaOriginalPos;
        panda.SetActive(false);

        engine.LoadGame();

        star.SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(10).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(17, true);

            completed = false;
            inputOn = false;
            //bearClicked = false;

            ResetHidingSpots();
            ResetStartingSpot();
            StopAllCoroutines();

            if (questState == 0)
            {
                panda.SetActive(true);
                transform.GetChild(9).gameObject.SetActive(true);
                transform.GetChild(10).gameObject.SetActive(false);
                transform.GetChild(11).gameObject.SetActive(false);
                bear.SetActive(true);
                StartCoroutine(StartTimeLine());
            }
            else
            {
                bear.SetActive(true);
                bear.transform.position = pandaOriginalPos;
                bear.transform.rotation = bearOriginalRot;
                bearAnim.SetBool("thinking", true);
                StartCoroutine(QuestStateTimeLine(questState));
            }

        }
        else
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[201];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Otso ja Panda tutustuivat jo, ja Panda löytyi piilosta! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));
        }
    }

    private void OnDisable()
    {
        inputOn = false;
        if (menu != null)
        {
            menu.SetActive(true);
        }
        if (localFairy != null)
        {
            localFairy.SetActive(true);
        }
        trackableFairySpeechBubble.SetActive(false);
        trackableFairy.SetActive(false);
        curtains.SetActive(false);
        StopAllCoroutines();

        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    IEnumerator MoveCurtainsFront()
    {
        curtainText.text = "";
        curtains.SetActive(true);
        curtainLeft.transform.position = curtainLOriginalPos;
        curtainRight.transform.position = curtainROriginalPos;

        bool curtainsAtFront = false;

        while (curtainsAtFront != true)
        {
            yield return new WaitForSeconds(0.01f);

            curtainLeft.transform.localPosition += new Vector3(1f, 0, 0) * Time.deltaTime * curtainSpeed;
            curtainRight.transform.localPosition -= new Vector3(1f, 0, 0) * Time.deltaTime * curtainSpeed;

            if (curtainLeft.transform.localPosition.x > -0.25 && curtainRight.transform.localPosition.x < 0.25)
            {
                curtainsAtFront = true;
            }
        }
    }

    IEnumerator HidingCountDown()
    {
        int countDownNumber;
        string countDownString;

        for (int i = 0; i < 10; i++)
        {

            countDownNumber = i + 1;
            countDownString = countDownNumber.ToString();
            curtainText.text = countDownString;

            fairySpeechAS.clip = acm.fairyDialoqueClips[191 + i];
            fairySpeechAS.Play();

            yield return new WaitForSeconds(1f);
        }

        fairySpeechAS.Stop();
    }

    void ResetHidingSpots()
    {
        transform.GetChild(8).gameObject.SetActive(false);
        transform.GetChild(7).gameObject.SetActive(true);
        transform.GetChild(10).gameObject.SetActive(false);
        transform.GetChild(9).gameObject.SetActive(true);
        transform.GetChild(13).gameObject.SetActive(false);
        transform.GetChild(12).gameObject.SetActive(true);
        transform.GetChild(15).gameObject.SetActive(false);
        transform.GetChild(14).gameObject.SetActive(true);
    }

    void ResetStartingSpot()
    {
        transform.GetChild(9).gameObject.SetActive(false);
        transform.GetChild(10).gameObject.SetActive(true);
        transform.GetChild(10).GetChild(1).gameObject.SetActive(true);
        transform.GetChild(10).GetChild(2).gameObject.SetActive(false);
        transform.GetChild(11).gameObject.SetActive(false);
    }
}
