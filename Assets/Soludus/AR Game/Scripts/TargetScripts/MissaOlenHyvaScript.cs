using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissaOlenHyvaScript : BaseTargetScript
{
    public bool inputOn = false;

    private bool completed;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    private int questState;
    private bool memoryGameIsActive = false;
    public bool cardIsRotating = false;
    public GameObject currentCard;
    public int currentIndex;
    public GameObject lastCard;
    public int lastIndex;

    public bool jumpingIsActive = false;
    public int timesJumped = 0;

    public int cardPairsFound;

    public int[] spawnPositionIndexArray;

    public GameObject drawingForOtso;
    public bool drawingIsActive = false;
    public DragBunnyScript dragBunny;
    public DragBunnyScript dragCat;

    public Camera localCamera;
    public GameObject star;

    public GameObject missaOlenHyvaCanvas;
    public GameObject muistiPeliCanvas;
    public GameObject muistiPeli;

    public Animator trackableFairyAnim;

    Vector3 bearOriginalPos;
    Vector3 fairyOriginalPos;
    Quaternion fairyOriginalRot;

    private bool showNatureOnce;

    Coroutine speechCO;

    public AudioSource localFairyAudioSource;

    public GameObject replayDialog;

    void Awake()
    {
        completed = false;

        // testing quest state
        questState = 0;

        cardPairsFound = 0;
        spawnPositionIndexArray = new int[] { 8, 9, 10, 11, 12, 13, 14, 15 };

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();
        bearOriginalPos = bear.transform.position;
        fairyOriginalPos = trackableFairy.transform.position;
        fairyOriginalRot = trackableFairy.transform.rotation;

        showNatureOnce = false;
    }

    void Update()
    {
        // level completed
        if (questState == 4 && completed == false)
        {
            completed = true;

            engine.IncrementScore(11);

            star.SetActive(true);

            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");
            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
            StartCoroutine(VictorySpeech());
        }

        if (inputOn)
        {
            Vector3 inputVector;

            if (TouchScreenScript.GetTouch(out inputVector))
            {
                if (jumpingIsActive)
                {
                    Ray ray = Camera.main.ScreenPointToRay(inputVector);
                    RaycastHit hit;
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && inputOn)
                    {

                        if (hit.collider.name == "otso")
                        {

                            inputOn = false;

                            StartCoroutine(BearIsJumping());
                        }
                    }
                }
                else if (memoryGameIsActive)
                {

                    Ray ray = localCamera.ScreenPointToRay(inputVector);
                    RaycastHit hit;
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, (1 << 8)) && inputOn)
                    {

                        if (hit.collider.tag == "memoryGameCard")
                        {

                            if (hit.collider.transform.parent.transform.rotation.eulerAngles.y == 180f)
                            {

                                if (cardIsRotating == false)
                                {
                                    cardIsRotating = true;

                                    if (hit.collider.transform.parent.name == "placeholder_palaA1")
                                    {
                                        lastIndex = currentIndex;
                                        currentIndex = 0;
                                        StartCoroutine(RotateCard(0));
                                    }
                                    else if (hit.collider.transform.parent.name == "placeholder_palaB1")
                                    {
                                        lastIndex = currentIndex;
                                        currentIndex = 1;
                                        StartCoroutine(RotateCard(1));
                                    }
                                    else if (hit.collider.transform.parent.name == "placeholder_palaC1")
                                    {
                                        lastIndex = currentIndex;
                                        currentIndex = 2;
                                        StartCoroutine(RotateCard(2));
                                    }
                                    else if (hit.collider.transform.parent.name == "placeholder_palaD1")
                                    {
                                        lastIndex = currentIndex;
                                        currentIndex = 3;
                                        StartCoroutine(RotateCard(3));
                                    }
                                    else if (hit.collider.transform.parent.name == "placeholder_palaA2")
                                    {
                                        lastIndex = currentIndex;
                                        currentIndex = 4;
                                        StartCoroutine(RotateCard(4));
                                    }
                                    else if (hit.collider.transform.parent.name == "placeholder_palaB2")
                                    {
                                        lastIndex = currentIndex;
                                        currentIndex = 5;
                                        StartCoroutine(RotateCard(5));
                                    }
                                    else if (hit.collider.transform.parent.name == "placeholder_palaC2")
                                    {
                                        lastIndex = currentIndex;
                                        currentIndex = 6;
                                        StartCoroutine(RotateCard(6));
                                    }
                                    else if (hit.collider.transform.parent.name == "placeholder_palaD2")
                                    {
                                        lastIndex = currentIndex;
                                        currentIndex = 7;
                                        StartCoroutine(RotateCard(7));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator VictorySpeech()
    {
        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(14, false);
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    void StartTimeLine()
    {
        if (questState == 0)
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }
        }

        bear.SetActive(true);

        if (questState < 3)
        {
            speechCO = StartCoroutine(ShowFairySpeech("SELVITETÄÄN MISSÄ OTSO ON HYVÄ. KOSKETTAKAA HALUAMAANNE VAIHTOEHTOA.", 5f));

            missaOlenHyvaCanvas.SetActive(true);
        }
        else
        {
            StartCoroutine(EndTimeLine());
        }
    }

    public void StartJumpTimeLine()
    {
        StopCoroutine(speechCO);
        StartCoroutine(JumpTimeLine());
    }

    IEnumerator JumpTimeLine()
    {
        missaOlenHyvaCanvas.SetActive(false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[207];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kokeilemme hyppäämistä! Napauttakaa ruutua Otson kohdalla, niin Otso koettaa hypätä.", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        speechCO = StartCoroutine(ShowFairySpeech("KOSKETTAKAA OTSOA, NIIN HÄN HYPPÄÄ.", 5f));

        inputOn = true;
        jumpingIsActive = true;

        yield return StartCoroutine(JumpingOn());

        replayDialog.SetActive(false);
        inputOn = false;
        questState += 1;
        missaOlenHyvaCanvas.transform.GetChild(0).gameObject.SetActive(false);

        if (questState < 3)
        {
            replayDialog.SetActive(false);
            missaOlenHyvaCanvas.SetActive(true);
        }
        else
            StartCoroutine(EndTimeLine());
    }

    public void StartDrawingTimeLine()
    {
        StopCoroutine(speechCO);
        StartCoroutine(DrawingTimeLine());
    }

    IEnumerator JumpingOn()
    {
        while (jumpingIsActive)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);
    }

    IEnumerator BearIsJumping()
    {
        if (timesJumped < 1)
        {
            bearAnim.SetBool("jumpfails", true);
            bearAnim.SetBool("sitting", true);
        }
        else bearAnim.SetBool("jumpfails", false);

        bearAnim.SetTrigger("jump");

        yield return new WaitForSeconds(1f);

        if (timesJumped < 1)
        {
            yield return new WaitForSeconds(1.5f);
            bearAnim.SetBool("sitting", false);
            yield return new WaitForSeconds(1.625f);
            bearAnim.SetBool("headStayDown", true);
            bearAnim.SetTrigger("headDown");

            StopCoroutine(speechCO);

            fairySpeechAS.clip = acm.fairyDialoqueClips[208];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            yield return StartCoroutine(ShowFairySpeechWaitForInput("No se oli melko omintakeinen hyppy, mutta ei se huono ollut! Eikä kaikessa tarvitse tai edes voi olla heti hyvä.", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[209];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Taidot karttuvat vasta ajan myötä, kun niitä harjoittelee. Hyvin kokeiltu, Otso!", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            bearAnim.SetBool("headStayDown", false);
        }

        jumpingIsActive = false;
    }

    IEnumerator DrawingTimeLine()
    {
        missaOlenHyvaCanvas.SetActive(false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[210];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kokeilemme piirtämistä!", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[211];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Auttakaa Otsoa valitsemaan aihe! Vetäkää haluamanne kuva Otson edessä olevan paperin päälle, niin hän koettaa piirtää sen.", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        speechCO = StartCoroutine(ShowFairySpeech("AUTTAKAA OTSOA VALITSEMAAN AIHE VETÄMÄLLÄ HALUAMANNE KUVA PAPERIN PÄÄLLE.", 5f));

        drawingIsActive = true;
        drawingForOtso.SetActive(true);

        yield return StartCoroutine(DrawingOn());

        StopCoroutine(speechCO);
        drawingForOtso.SetActive(false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[212];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa Otso! Sinä olet tosi hyvä piirtämään. Se taitaa johtua siitä, että olet käynyt niin paljon kuvataidekerhossa ja opiskellut ja harjoitellut ahkerasti!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        questState += 1;
        missaOlenHyvaCanvas.transform.GetChild(1).gameObject.SetActive(false);

        if (questState < 3)
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[213];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Mitäs seuraavaksi?", 1f));
            fairySpeechAS.Stop();
            replayDialog.SetActive(false);
            speechCO = StartCoroutine(ShowFairySpeech("KOSKETTAKAA HALUAMAANNE VAIHTOEHTOA.", 5f));
            missaOlenHyvaCanvas.SetActive(true);
        }
        else
            StartCoroutine(EndTimeLine());

    }

    IEnumerator DrawingOn()
    {
        while (drawingIsActive)
        {
            yield return null;
        }

        dragBunny.GetComponent<Collider>().enabled = false;
        dragCat.GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(2f);
    }

    public void StartMemoryGameTimeLine()
    {
        StopCoroutine(speechCO);
        StartCoroutine(MemoryGameTimeLine());
    }

    IEnumerator MemoryGameTimeLine()
    {
        missaOlenHyvaCanvas.SetActive(false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[214];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kokeilemme muistitaitoja!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        speechCO = StartCoroutine(ShowFairySpeech("MUISTIPELI: KÄÄNTÄKÄÄ KORTTEJA KOSKETTAMALLA NIITÄ. YRITTÄKÄÄ LÖYTÄÄ KAIKILLE RASTEILLE PARI!", 5f));

        muistiPeli.SetActive(true);

        List<int> indexList = new List<int>();
        indexList.AddRange(spawnPositionIndexArray);

        for (int i = 0; i < 8; i++)
        {

            int rand = Random.Range(0, indexList.Count);
            int value = indexList[rand];

            muistiPeli.transform.GetChild(i).transform.position = muistiPeli.transform.GetChild(value).transform.position;
            indexList.RemoveAt(rand);
        }

        memoryGameIsActive = true;
        inputOn = true;
        yield return StartCoroutine(MemoryGameOn());

        StopCoroutine(speechCO);
        muistiPeli.SetActive(false);
        inputOn = false;
        fairySpeechAS.clip = acm.fairyDialoqueClips[215];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa, lapset ja Otso! Taitoja voi harjoitella myös yhdessä ja toinen toistaan auttaen!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        questState += 1;
        missaOlenHyvaCanvas.transform.GetChild(2).gameObject.SetActive(false);

        if (questState < 3)
        {
            missaOlenHyvaCanvas.SetActive(true);
            replayDialog.SetActive(false);
            speechCO = StartCoroutine(ShowFairySpeech("KOSKETTAKAA HALUAMAANNE VAIHTOEHTOA.", 5f));
        }
        else
            StartCoroutine(EndTimeLine());
    }

    IEnumerator MemoryGameOn()
    {
        while (memoryGameIsActive)
            yield return null;

        yield return new WaitForSeconds(2f);
    }

    IEnumerator RotateCard(int index)
    {
        GameObject selectedCard = muistiPeli.transform.GetChild(index).gameObject;

        while (selectedCard.transform.rotation.eulerAngles.y <= 180f)
        {
            selectedCard.transform.Rotate(0, -Time.deltaTime * 300f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        selectedCard.transform.localEulerAngles = new Vector3(0, 0, 0);

        lastCard = currentCard;
        currentCard = selectedCard;

        yield return StartCoroutine(CheckGameState(selectedCard));
    }

    IEnumerator ResetCardRotation(int index)
    {
        GameObject selectedCard = muistiPeli.transform.GetChild(index).gameObject;

        while (selectedCard.transform.rotation.eulerAngles.y < 180f)
        {
            selectedCard.transform.Rotate(0, Time.deltaTime * 300f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        selectedCard.transform.localEulerAngles = new Vector3(0, 180f, 0);
    }

    void ResetAllCards()
    {
        for (int i = 0; i < muistiPeli.transform.childCount - 8; i++)
        {
            muistiPeli.transform.GetChild(i).transform.eulerAngles = new Vector3(0, 180f, 0);
        }
    }

    IEnumerator CheckGameState(GameObject selectedCard)
    {
        if (lastCard != null)
        {
            if (currentCard.name == "placeholder_palaA1" && lastCard.name == "placeholder_palaA2" || currentCard.name == "placeholder_palaA2" && lastCard.name == "placeholder_palaA1")
            {
                lastCard = null;
                currentCard = null;
                lastIndex = 0;
                currentIndex = 0;
                Debug.Log(lastCard);
                Debug.Log(currentCard);
                cardPairsFound++;
            }
            else if (currentCard.name == "placeholder_palaB1" && lastCard.name == "placeholder_palaB2" || currentCard.name == "placeholder_palaB2" && lastCard.name == "placeholder_palaB1")
            {
                lastCard = null;
                currentCard = null;
                lastIndex = 0;
                currentIndex = 0;
                Debug.Log(lastCard);
                Debug.Log(currentCard);
                cardPairsFound++;
            }
            else if (currentCard.name == "placeholder_palaC1" && lastCard.name == "placeholder_palaC2" || currentCard.name == "placeholder_palaC2" && lastCard.name == "placeholder_palaC1")
            {
                lastCard = null;
                currentCard = null;
                lastIndex = 0;
                currentIndex = 0;
                Debug.Log(lastCard);
                Debug.Log(currentCard);
                cardPairsFound++;
            }
            else if (currentCard.name == "placeholder_palaD1" && lastCard.name == "placeholder_palaD2" || currentCard.name == "placeholder_palaD2" && lastCard.name == "placeholder_palaD1")
            {
                lastCard = null;
                currentCard = null;
                lastIndex = 0;
                currentIndex = 0;
                Debug.Log(lastCard);
                Debug.Log(currentCard);
                cardPairsFound++;
            }
            else
            {
                StartCoroutine(ResetCardRotation(lastIndex));
                yield return StartCoroutine(ResetCardRotation(currentIndex));

                lastCard = null;
                currentCard = null;
                lastIndex = 0;
                currentIndex = 0;
            }
        }

        if (cardPairsFound >= 4)
        {
            memoryGameIsActive = false;
        }

        cardIsRotating = false;
    }

    public void StartEndTimeLine()
    {
        StartCoroutine(EndTimeLine());
    }

    IEnumerator EndTimeLine()
    {
        fairySpeechAS.clip = acm.fairyDialoqueClips[216];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Sinähän osasit vaikka mitä, Otso! Ja te myös, lapset! Me osaamme yhteensä melkomoisen määrän asioita! Ja jatkamme myös harjoittelua, eikö vain?", 1f));
        fairySpeechAS.Stop();
        replayDialog.SetActive(false);

        StartCoroutine(IncQuestStateAfterDelay());
    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState++;
    }

    private void OnEnable()
    {
        //acm.currentFairyAudioSource = fairyAudioSource;
        //acm.currentBearAudioSource = bearAudioSource;
        menu.SetActive(false);

        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.SetActive(true);
        bear.transform.position = bearOriginalPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bear.SetActive(false);

        engine.LoadGame();

        star.SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(11).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(17, true);

            completed = false;
            cardPairsFound = 0;
            timesJumped = 0;
            drawingForOtso.SetActive(true);
            dragBunny.allowDragging = true;
            dragBunny.transform.localPosition = dragBunny.originalPos;
            dragCat.allowDragging = true;
            dragCat.transform.localPosition = dragCat.originalPos;
            dragBunny.GetComponent<Collider>().enabled = true;
            dragCat.GetComponent<Collider>().enabled = true;
            drawingForOtso.SetActive(false);
            trackableFairy.transform.position = fairyOriginalPos;
            trackableFairy.transform.rotation = fairyOriginalRot;

            StartTimeLine();
        }
        else
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[218];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Otso löysi jo vahvuutensa! Voitte kokeilla tätä rastia huomenna uudestaan", 5f));
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
        trackableFairySpeechBubble.SetActive(false);
        trackableFairyAnim.SetBool("walking", false);
        trackableFairyAnim.Play("AnimIdle");
        bear.SetActive(false);
        missaOlenHyvaCanvas.SetActive(false);
        ResetAllCards();
        muistiPeli.SetActive(false);
        cardIsRotating = false;
        memoryGameIsActive = false;
        inputOn = false;
        lastCard = null;
        lastIndex = 0;
        currentCard = null;
        currentIndex = 0;
        drawingForOtso.SetActive(false);
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        StopAllCoroutines();
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    IEnumerator WalkFairy(Vector3 targetPosition)
    {
        Vector3 desPos = trackableFairy.transform.position;
        desPos.x = targetPosition.x;

        Debug.Log(desPos);

        trackableFairy.transform.LookAt(desPos);

        Vector3 direction = (desPos - trackableFairy.transform.position).normalized;

        while (Vector3.Distance(trackableFairy.transform.position, desPos) > 0.1f * transform.localScale.x)
        {
            trackableFairy.transform.position += direction * Time.deltaTime * 0.3f * transform.localScale.x;
            yield return null;
        }
    }
}
