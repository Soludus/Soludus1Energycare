using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PiirtoTargetScript : BaseTargetScript
{
    [SerializeField] Transform localPaper = null;
    [SerializeField] GameObject readyLocalButton = null;
    [SerializeField] Transform drawingLocal = null;

    private int questState;
    private bool allowDrawingTurning;
    private bool allowCatColoring;

    public GameObject bunnyPic;
    public GameObject catPic;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public bool inputOn;

    private bool firstTimeTurned;

    public GameObject star;

    public bool completed;

    public Animator trackableFairyAnim;

    float localPaperY;

    Vector3 bearStartPos;

    Color selectedColor;

    bool pictureMode;
    [SerializeField] Camera localCam = null;

    bool aSideDone;

    private bool showNatureOnce;

    Coroutine speechCO;

    Quaternion drawingStartRot;

    public GameObject replayDialog;

    void Awake()
    {
        inputOn = false;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();

        bearStartPos = bear.transform.position;
        drawingStartRot = drawingLocal.transform.rotation;

        localPaperY = 0;
        pictureMode = true;

        selectedColor = Color.white;

        completed = false; ;

        // testing quest state
        questState = 0;

        showNatureOnce = false;
    }

    void Update()
    {
        if (questState == 3 && completed == false)
        {
            completed = true;

            StopCoroutine(speechCO);
            trackableFairySpeechBubble.SetActive(false);

            engine.IncrementScore(8);

            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
            star.SetActive(true);
            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");
            StartCoroutine(VictorySpeech());
        }

        Vector3 inputVector;
        // Raycasting, both for picturemode and level view (currently no use for level view raycasting)
        if (TouchScreenScript.GetTouch(out inputVector) && inputOn)
        {
            if (!pictureMode)
            {
                //Ray ray = Camera.main.ScreenPointToRay(inputVector);
                //RaycastHit hit;

                //if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && inputOn)
                //{
                //    print(hit.collider.gameObject.name);
                //}
            }
            else
            {
                Ray ray = localCam.ScreenPointToRay(inputVector);
                RaycastHit hit;


                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, (1 << 8)) && inputOn)
                {
                    if (hit.collider.tag == "ColorCube")
                    {
                        selectedColor = hit.collider.GetComponent<MeshRenderer>().material.color;
                    }

                    if (hit.collider.tag == "ColoringPicture")
                    {

                        if (hit.collider.transform.parent.parent.parent.name == "BunnyCanvas")
                        {

                            hit.collider.transform.parent.GetComponent<UnityEngine.UI.Image>().color = selectedColor;

                            aSideDone = true;

                            readyLocalButton.SetActive(true);

                        }
                        if (hit.collider.transform.parent.parent.parent.name == "CatCanvas" && allowCatColoring)
                        {

                            hit.collider.transform.parent.GetComponent<UnityEngine.UI.Image>().color = selectedColor;

                            if (aSideDone)
                            {
                                readyLocalButton.SetActive(true);
                            }
                        }
                    }

                    if (hit.collider.name == "ReadyButton")
                    {
                        readyLocalButton.SetActive(false);
                        drawingLocal.gameObject.SetActive(false);

                        if (questState == 0)
                        {
                            StartCoroutine(AfterFirstDrawingTimeLine());
                        }
                        else if (questState == 2)
                        {
                            StartCoroutine(IncQuestStateAfterDelay());
                        }
                    }

                    print(hit.collider.transform.parent.name);
                    print(selectedColor);
                }
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && allowDrawingTurning)
        {
            inputOn = false;
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            localPaperY += touchDeltaPosition.y * 4;
            localPaperY = Mathf.Clamp(localPaperY, 0, 180);

            localPaper.rotation = Quaternion.Euler(0, localPaperY, 0);

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
                StartCoroutine(RotateToClamp());
        }

        if (Input.GetMouseButton(0) && allowDrawingTurning)
        {
            inputOn = false;
            localPaperY -= Input.GetAxis("Mouse X") * 5;
            localPaperY = Mathf.Clamp(localPaperY, 0, 180);

            localPaper.rotation = Quaternion.Euler(0, localPaperY, 0);
        }

        if (Input.GetMouseButtonUp(0))
        {
            StartCoroutine(RotateToClamp());
        }
    }

    IEnumerator RotateToClamp()
    {
        if (localPaperY >= 70)
        {
            while (localPaperY < 180)
            {
                localPaperY += Time.deltaTime * 200;
                localPaper.rotation = Quaternion.Euler(0, localPaperY, 0);
                yield return null;
            }
            if (!firstTimeTurned)
            {
                localPaper.rotation = Quaternion.Euler(0, 180, 0);
                firstTimeTurned = true;
                allowDrawingTurning = false;
                yield return new WaitForSeconds(1f);
                drawingLocal.gameObject.SetActive(false);
                StartCoroutine(AfterDrawingTurningTimeLine());
            }
        }
        else
        {
            while (localPaperY > 0)
            {
                localPaperY -= Time.deltaTime * 200;
                localPaper.rotation = Quaternion.Euler(0, localPaperY, 0);
                yield return null;
            }
        }
    }

    IEnumerator VictorySpeech()
    {
        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(6, false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[159];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvää työtä! Tämä piirustus voi jäädä Otsolle muistoksi, jotta hän ei enää unohda piirtää paperin molemmille puolille.", 1f));
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    IEnumerator StartTimeLine()
    {
        bear.gameObject.SetActive(true);

        if (questState == 0)
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }

            drawingLocal.gameObject.SetActive(true);
            localPaper.rotation = Quaternion.Euler(0, 0, 0);

            fairySpeechAS.clip = acm.fairyDialoqueClips[152];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Komea kuva! Nyt on teidän vuoronne, lapset! Koskettakaa väriä ja sitten aluetta, jonka haluatte sillä värittää.", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            inputOn = true;
            pictureMode = true;

            speechCO = StartCoroutine(ShowFairySpeech("VALITKAA VÄRI JA KOSKETTAKAA VÄRITETTÄVÄÄ ALUETTA.", 5f));

        }
        else if (questState == 1)
        {
            StartCoroutine(AfterFirstDrawingTimeLine());
        }
        else if (questState == 2)
        {
            StartCoroutine(AfterDrawingTurningTimeLine());
        }
    }

    IEnumerator AfterFirstDrawingTimeLine()
    {
        StopCoroutine(speechCO);

        questState = 1;

        fairySpeechAS.clip = acm.fairyDialoqueClips[153];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Siitäpä tuli hieno!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        bearAnim.SetBool("sitting", false);
        yield return new WaitForSeconds(1f);

        fairySpeechAS.clip = acm.fairyDialoqueClips[154];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso!", 1f));
        fairySpeechAS.Stop();
        replayDialog.SetActive(false);

        bearAnim.SetTrigger("wondering");
        yield return new WaitForSeconds(3f);

        fairySpeechAS.clip = acm.fairyDialoqueClips[155];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Muistuttakaapa, lapset, Otsolle, mitä hänen pitäisi nyt tehdä, jos hän haluaa piirtää uuden kuvan?", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[156];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kääntäkää kuva ottamalla laidasta kiinni ja vetämällä se toiselle puolelle.", 1f));
        fairySpeechAS.Stop();
        replayDialog.SetActive(false);

        speechCO = StartCoroutine(ShowFairySpeech("KÄÄNTÄKÄÄ KUVA KOSKETTAMALLA REUNAA JA VETÄMÄLLÄ SE TOISELLE PUOLELLE.", 5f));

        inputOn = false;
        allowDrawingTurning = true;
        drawingLocal.gameObject.SetActive(true);

    }

    IEnumerator AfterDrawingTurningTimeLine()
    {
        StopCoroutine(speechCO);

        questState = 2;

        fairySpeechAS.clip = acm.fairyDialoqueClips[157];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mainiota! Nyt Otso voi käyttää paperin kokonaan hyödyksi, eikä se mene hukkaan!", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[158];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Auttakaa Otsoa värittämään vielä tämä kuva!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        bearAnim.SetBool("sitting", false);

        speechCO = StartCoroutine(ShowFairySpeech("VALITKAA VÄRI JA KOSKETTAKAA VÄRITETTÄVÄÄ ALUETTA.", 5f));

        allowCatColoring = true;
        drawingLocal.gameObject.SetActive(true);
        localPaper.rotation = Quaternion.Euler(0, 180, 0);
        allowDrawingTurning = false;
        inputOn = true;

    }

    public IEnumerator WalkBear(Vector3 pos, int lampNumber)
    {
        inputOn = false;
        bearAnim.SetBool("walking", true);
        while (Vector3.Distance(bear.transform.position, pos) > 0.01f)
        {
            bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f;
            bear.transform.LookAt(pos);
            yield return null;
        }

        bear.transform.rotation = Quaternion.identity;
        bearAnim.SetBool("walking", false);
        bearAnim.SetTrigger("pressButton");
        yield return new WaitForSeconds(0.75f);
        inputOn = true;
    }

    private void OnEnable()
    {
        menu.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.SetActive(true);
        bear.transform.position = bearStartPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bear.gameObject.SetActive(false);

        engine.LoadGame();
        inputOn = true;

        star.SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(8).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(8, true);

            localPaper.gameObject.SetActive(true);
            localPaperY = 0;

            completed = false;
            firstTimeTurned = false;
            inputOn = true;

            StartCoroutine(StartTimeLine());

        }
        else
        {

            inputOn = false;

            localPaper.gameObject.SetActive(false);

            fairySpeechAS.clip = acm.fairyDialoqueClips[160];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Otso on piirtänyt tänään tarpeeksi! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));
        }
    }

    private void OnDisable()
    {
        if (menu != null)
        {
            menu.SetActive(true);
        }
        trackableFairySpeechBubble.SetActive(false);
        drawingLocal.gameObject.SetActive(false);
        drawingLocal.rotation = drawingStartRot;
        localPaper.rotation = Quaternion.Euler(0, 0, 0);
        localPaper.gameObject.SetActive(false);
        if (localFairy != null)
        {
            localFairy.SetActive(true);
        }
        trackableFairyAnim.SetBool("walking", false);
        trackableFairyAnim.Play("AnimIdle");
        trackableFairy.SetActive(false);
        bear.SetActive(false);
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
        StopAllCoroutines();
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState++;
    }
}
