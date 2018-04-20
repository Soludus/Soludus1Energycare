using UnityEngine;
using System.Collections;

public class HanaTargetScript : BaseTargetScript
{
    private bool completed;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public bool inputOn;

    private int questState;

    public GameObject[] resultImages;
    public GameObject sink;
    public GameObject soap;
    public GameObject towel;

    public GameObject buttonCanvas;

    public GameObject star;

    public Animator hanaAnimator;

    public Animator trackableFairyAnim;

    Vector3 bearOriginalPos;
    Vector3 bearPos;

    private bool showNatureOnce;

    public Camera playerCamera;

    public AudioSource sinkLoop;

    void Awake()
    {
        completed = false;
        inputOn = true;

        // disable buttoncanvas, so it doesn't get stuck into camera after target is lost
        buttonCanvas.SetActive(false);

        // hide results on startup
        HideResults();

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
        if (questState == 8 && completed == false)
        {
            completed = true;

            engine.IncrementScore(0);

            star.SetActive(true);
            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");
            bear.transform.LookAt(new Vector3(playerCamera.transform.position.x, bear.transform.position.y, playerCamera.transform.position.z));
            StartCoroutine(VictorySpeech());
        }

        Vector3 inputVector;

        if (TouchScreenScript.GetTouch(out inputVector) && inputOn)
        {
            Ray ray = playerCamera.ScreenPointToRay(inputVector);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && inputOn)
            {
                if (hit.collider.gameObject.name == "sink")
                {
                    // Open sink
                    if (questState == 0)
                    {
                        inputOn = false;
                        Vector3 desPos = hit.collider.transform.position;
                        soundEffectAS.clip = acm.energyRoom1Clips[0];
                        soundEffectAS.Play();
                        StartCoroutine(WalkBear(desPos, 1));
                        StartCoroutine(OpenSinkAfterDelay());
                        StartCoroutine(ShowResultImageAfterDelay(2.5f, 0));
                        StartCoroutine(InputCooldown(2.5f));
                        StartCoroutine(IncQuestStateAfterDelay(2.5f));

                        // wash hands
                    }
                    else if (questState == 1)
                    {
                        inputOn = false;
                        soundEffectAS.clip = acm.energyRoom1Clips[2];
                        soundEffectAS.Play();
                        bearAnim.SetTrigger("handwash");

                        StartCoroutine(ShowResultImageAfterDelay(2.5f, 1));
                        StartCoroutine(InputCooldown(2.5f));
                        StartCoroutine(IncQuestStateAfterDelay(2.5f));
                        // close sink
                    }
                    else if (questState == 2)
                    {
                        inputOn = false;
                        sinkLoop.Stop();
                        soundEffectAS.clip = acm.energyRoom1Clips[13];
                        soundEffectAS.Play();
                        bearAnim.SetTrigger("pressButton");

                        hanaAnimator.SetTrigger("openSink");
                        StartCoroutine(ShowResultImageAfterDelay(2.5f, 2));
                        StartCoroutine(InputCooldown(2.5f));
                        StartCoroutine(IncQuestStateAfterDelay(2.5f));
                        // open sink
                    }
                    else if (questState == 4)
                    {
                        inputOn = false;
                        Vector3 desPos = hit.collider.transform.position;

                        StartCoroutine(WalkBear(desPos, 1));
                        StartCoroutine(OpenSinkAfterDelay());
                        StartCoroutine(ShowResultImageAfterDelay(2.5f, 4));
                        StartCoroutine(InputCooldown(2.5f));
                        StartCoroutine(IncQuestStateAfterDelay(2.5f));
                        // wash hands
                    }
                    else if (questState == 5)
                    {
                        inputOn = false;
                        soundEffectAS.clip = acm.energyRoom1Clips[2];
                        soundEffectAS.Play();
                        bearAnim.SetTrigger("handwash");

                        StartCoroutine(ShowResultImageAfterDelay(2.5f, 5));

                        StartCoroutine(InputCooldown(2.5f));
                        StartCoroutine(IncQuestStateAfterDelay(2.5f));
                    }

                    // close sink
                    else if (questState == 6)
                    {
                        inputOn = false;
                        sinkLoop.Stop();
                        soundEffectAS.clip = acm.energyRoom1Clips[13];
                        soundEffectAS.Play();
                        bearAnim.SetTrigger("pressButton");

                        hanaAnimator.SetTrigger("openSink");
                        StartCoroutine(ShowResultImageAfterDelay(2.5f, 6));

                        StartCoroutine(InputCooldown(2.5f));
                        StartCoroutine(IncQuestStateAfterDelay(2.5f));
                    }
                }

                if (hit.collider.gameObject.name == "soap")
                {
                    if (questState == 3)
                    {
                        inputOn = false;
                        Vector3 desPos = hit.collider.transform.position;

                        StartCoroutine(WalkBear(desPos, 2));

                        StartCoroutine(ShowResultImageAfterDelay(2.5f, 3));
                        StartCoroutine(IncQuestStateAfterDelay(2.5f));

                        StartCoroutine(InputCooldown(4f));
                    }
                }
                if (hit.collider.gameObject.name == "towel")
                {
                    if (questState == 7)
                    {
                        inputOn = false;
                        Vector3 desPos = hit.collider.transform.position;

                        StartCoroutine(WalkBear(desPos, 3));
                        StartCoroutine(ShowResultImageAfterDelay(2.5f, 7));
                        StartCoroutine(IncQuestStateAfterDelay(4));
                    }
                }
            }
        }
    }

    IEnumerator VictorySpeech()
    {
        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(1, false);

        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        fairySpeechAS.clip = acm.fairyDialoqueClips[24];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa, osasitte hyvin! Nämä ohjeet kannattaa muistaa aina käsiä pestessä ja soveltaa niitä myös suihkussakäymiseen.", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    void StartTimeLine()
    {
        inputOn = false;
        bear.SetActive(true);

        StartCoroutine(ShowFairySpeechWaitForInput("OPETTAKAA OTSOLLE KÄSIENPESU KOSKETTAMALLA HANAA, SAIPPUAA JA PYYHETTÄ OIKEASSA JÄRJESTYKSESSÄ.", 3f));

        inputOn = true;

        if (questState == 4)
        {
            bearPos = soap.transform.position;
            bearPos.y = bear.transform.position.y;
            bear.transform.position = bearPos + new Vector3(0, 0, -0.75f);
            bear.transform.LookAt(bearPos);

        }
        else if (questState != 0)
        {
            bearPos = sink.transform.position;
            bearPos.y = bear.transform.position.y;
            bear.transform.position = bearPos + new Vector3(0, 0, -0.75f);
            bear.transform.LookAt(bearPos);
        }
        else
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }

            bear.transform.position = bearOriginalPos;
        }
    }

    IEnumerator OpenSinkAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        soundEffectAS.clip = acm.energyRoom1Clips[1];
        soundEffectAS.Play();
        hanaAnimator.SetTrigger("openSink");
    }

    IEnumerator IncQuestStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        questState++;
    }

    IEnumerator ShowResultImageAfterDelay(float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        resultImages[index].SetActive(true);
    }

    // Keeps the bear "grounded" while rotating
    void BearLookAtPosition(Vector3 pos)
    {
        pos.y = bear.transform.position.y;

        bear.transform.LookAt(pos);
    }

    public IEnumerator WalkBear(Vector3 pos, int action)
    {
        pos.y = bear.transform.position.y;
        pos.z -= 0.25f * transform.localScale.x;
        bearAnim.SetBool("walking", true);
        while (Vector3.Distance(bear.transform.position, pos) > 0.01f * transform.localScale.x)
        {
            bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            bear.transform.LookAt(pos);
            yield return null;
        }

        bear.transform.rotation = Quaternion.identity;
        bearAnim.SetBool("walking", false);

        if (action == 1)
        {
            bearAnim.SetTrigger("pressButton");
        }
        else if (action == 2)
        {
            soundEffectAS.clip = acm.energyRoom1Clips[3];
            soundEffectAS.Play();
            bearAnim.SetTrigger("handwash");
        }
        else if (action == 3)
        {
            bearAnim.SetTrigger("towel");
        }

        yield return new WaitForSeconds(2.4f);
    }

    void HideResults()
    {
        foreach (GameObject go in resultImages)
        {
            go.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (questState == 1 || questState == 2 || questState == 5 || questState == 6)
        {
            hanaAnimator.Play("sinkOn");
            sinkLoop.Play();
        }
        else
        {
            hanaAnimator.Play("sinkOff");
            sinkLoop.Stop();
        }
        menu.SetActive(false);

        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.transform.position = bearOriginalPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bear.SetActive(false);

        engine.LoadGame();
        inputOn = true;
        trackableFairySpeechBubble.SetActive(false);

        star.SetActive(false);

        // activate canvas when target is loaded, it's disabled so that it doesn't get stuck to the camera after target is lost.
        buttonCanvas.SetActive(true);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(0).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(3, true);

            completed = false;

            StartTimeLine();
        }
        else
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[25];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Otson käpälät ovat jo puhtaat! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));
        }
    }

    private void OnDisable()
    {
        if (soundEffectAS.clip == acm.energyRoom1Clips[0])
        {
            sinkLoop.Stop();
        }
        hanaAnimator.SetBool("open", false);

        if (menu != null)
        {
            menu.SetActive(true);
        }
        if (localFairy != null)
        {
            localFairy.SetActive(true);
        }
        StopAllCoroutines();
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    IEnumerator InputCooldown(float waitTime)
    {
        inputOn = false;
        yield return new WaitForSeconds(waitTime);
        inputOn = true;
    }
}
