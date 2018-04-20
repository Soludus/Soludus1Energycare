using System.Collections;
using UnityEngine;

public class KatkaisijaTargetScript : BaseTargetScript
{
    public bool inputOn;
    public GameObject star;
    public Animator trackableFairyAnim;

    private bool completed;
    private bool showNatureOnce;
    private bool switchingIsActive;
    private bool lampsAreOff;

    [Header("Lights")]
    public GameObject[] lightSwitches;
    public GameObject[] lightCones;
    public GameObject[] lightBulbs;
    public Material matLightBulbOn;
    public Material matLightBulbOff;

    private bool[] lightStates;

    [Header("Bear")]
    public GameObject bearSpeechBubble;
    public Animator bearAnim;
    public Transform bearHand;

    private Vector3 bearStartPos;

    [Header("Trash")]
    public GameObject trash;

    private Vector3 trashOriginalPos;
    private Quaternion trashOriginalRot;


    private void Awake()
    {
        Debug.Assert(lightSwitches.Length == lightBulbs.Length && lightSwitches.Length == lightCones.Length);
        lightStates = new bool[lightSwitches.Length];
        for (int i = 0; i < lightSwitches.Length; i++)
        {
            SetLightState(i, true);
        }

        completed = false;
        inputOn = true;
        switchingIsActive = true;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();

        bearStartPos = bear.transform.position;
        trashOriginalPos = trash.transform.position;
        trashOriginalRot = trash.transform.rotation;

        showNatureOnce = false;
    }

    private void Update()
    {
        if (switchingIsActive)
        {
            Vector3 inputVector;
            if (inputOn && TouchScreenScript.GetTouch(out inputVector))
            {
                Ray ray = Camera.main.ScreenPointToRay(inputVector);
                RaycastHit hit;

                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
                {
                    var hitGO = hit.collider.gameObject;
                    for (int i = 0; i < lightSwitches.Length; i++)
                    {
                        if (hitGO == lightSwitches[i])
                        {
                            StartCoroutine(BearWalkAndTriggerLight(new Vector3(hitGO.transform.position.x, bear.transform.position.y, bear.transform.position.z), i));
                        }
                    }
                }
            }
        }
    }

    private IEnumerator VictorySpeech()
    {
        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(19, false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[34];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Huh, tilanne pelastettu. Kiitos, lapset! Nyt sopii sinunkin, Otso, lähteä hoitamaan toimiasi. Muista vain jatkossa sammuttaa turhat valot!", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    private void TriggerLight(int light)
    {
        SetLightState(light, !lightStates[light]);

        soundEffectAS.clip = acm.energyRoom2Clips[0];
        soundEffectAS.Play();
    }

    private void SetLightState(int light, bool state)
    {
        lightStates[light] = state;
        var bulb = lightBulbs[light].GetComponentInChildren<Renderer>();
        if (lightStates[light])
        {
            bulb.sharedMaterial = matLightBulbOn;
            lightCones[light].SetActive(true);
        }
        else
        {
            bulb.sharedMaterial = matLightBulbOff;
            lightCones[light].SetActive(false);
        }
    }

    public IEnumerator BearWalkOut()
    {
        yield return new WaitForSeconds(2f);
        bearAnim.Play("pickupidleanim");
        trash.transform.SetParent(bearHand);
        trash.transform.localPosition = new Vector3(0, 0.01f, -0.026f);
        trash.SetActive(true);
        yield return StartCoroutine(WalkBearToPos(bearStartPos));
        bear.SetActive(false);
    }

    public IEnumerator BearWalkAndTriggerLight(Vector3 pos, int lampNumber)
    {
        inputOn = false;
        yield return StartCoroutine(WalkBearToPos(pos));

        bear.transform.rotation = Quaternion.identity;
        bearAnim.SetTrigger("pressButton");
        yield return new WaitForSeconds(0.75f);

        TriggerLight(lampNumber);

        yield return new WaitForSeconds(1f);
        inputOn = true;
    }

    public IEnumerator WalkBearToPos(Vector3 pos)
    {
        bearAnim.SetBool("walking", true);
        while (Vector3.Distance(bear.transform.position, pos) > 0.01f * transform.localScale.x)
        {
            bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            bear.transform.LookAt(pos);
            yield return null;
        }

        bearAnim.SetBool("walking", false);
    }

    public void OnEnable()
    {
        menu.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.SetActive(true);
        bear.transform.position = bearStartPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bearAnim.SetBool("sitting", false);
        bear.gameObject.SetActive(false);

        engine.LoadGame();
        inputOn = true;
        trackableFairySpeechBubble.SetActive(false);

        star.SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(1).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(21, true);

            for (int i = 0; i < lightSwitches.Length; i++)
            {
                SetLightState(i, true);
            }

            lampsAreOff = false;

            completed = false;
            inputOn = true;

            trash.SetActive(true);
            trash.transform.position = trashOriginalPos;
            trash.transform.rotation = trashOriginalRot;
            trash.transform.SetParent(gameObject.transform);

            StartCoroutine(StartTimeLine());
        }
        else
        {
            inputOn = false;

            for (int i = 0; i < lightSwitches.Length; i++)
            {
                SetLightState(i, false);
            }

            fairySpeechAS.clip = acm.fairyDialoqueClips[35];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Turhat lamput on jo sammutettu! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));
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
        StopAllCoroutines();
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private IEnumerator StartTimeLine()
    {
        if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
        {
            dataActionController.RunAction(dataAction[0]);
        }

        trash.SetActive(false);
        bear.gameObject.SetActive(true);

        var speechCO = StartCoroutine(ShowFairySpeech("AUTTAKAA OTSOA SAMMUTTAMAAN VALOT KOSKETTAMALLA KATKAISIJOITA.", 5f));

        inputOn = true;
        switchingIsActive = true;

        yield return StartCoroutine(WaitUntilLampsAreOff());

        StopCoroutine(speechCO);

        switchingIsActive = false;

        yield return new WaitForSeconds(1f);
        bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));

        lampsAreOff = true;
        completed = true;
        engine.IncrementScore(1);

        bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
        star.SetActive(true);
        trackableFairyAnim.SetTrigger("celebrate");
        bearAnim.SetTrigger("celebrate");
        StartCoroutine(BearWalkOut());
        StartCoroutine(VictorySpeech());
    }

    private IEnumerator WaitUntilLampsAreOff()
    {
        while (!AllLightsAreOff())
        {
            yield return null;
        }
    }

    private bool AllLightsAreOff()
    {
        for (int i = 0; i < lightSwitches.Length; i++)
        {
            if (lightStates[i])
                return false;
        }
        return true;
    }
}
