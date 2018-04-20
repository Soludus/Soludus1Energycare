using UnityEngine;
using System.Collections;

public class RoskaTargetScript : BaseTargetScript
{
    public GameObject trackableFairySpeech;

    private bool completed;

    public Animator bearAnim;
    public GameObject bearSpeech;
    public Transform bearHand;

    public GameObject[] trash;
    public GameObject[] bins;

    private Vector3[] originalPositions;
    private Quaternion[] originalRots;
    private Vector3 bearOriginalPos;

    public GameObject pickedTrash;

    public bool inputOn;

    public GameObject star;

    public Animator trackableFairyAnim;
    private int questState;

    private bool showNatureOnce;

    public GameObject saladBall;

    public TrashBioManagerScript trashManager;

    public bool trashDragged;

    public AudioSource trashAS;

    // Use this for initialization
    void Awake()
    {
        questState = 0;

        completed = false;
        originalPositions = new Vector3[5];
        originalRots = new Quaternion[5];
        for (int i = 0; i < 5; i++)
        {
            originalPositions[i] = trash[i].transform.position;
            originalRots[i] = trash[i].transform.rotation;
        }
        bearOriginalPos = bear.transform.position;

        pickedTrash = null;
        inputOn = false;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();

        showNatureOnce = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (questState == 6 && completed == false)
        {
            completed = true;

            engine.IncrementScore(6);

            star.SetActive(true);

            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");

            StartCoroutine(VictorySpeech());

            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
        }

        Vector3 inputVector;
        if (TouchScreenScript.GetTouch(out inputVector) && inputOn)
        {
            if (pickedTrash != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(inputVector);
                RaycastHit hit;

                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && inputOn)
                {
                    if (hit.collider.gameObject.name == "binMetal" && pickedTrash.name == "trashMetal")
                    {
                        StartCoroutine(MoveDropAndPick(3, 2, 2, 10));
                        inputOn = false;
                    }

                    if (hit.collider.gameObject.name == "binBio" && pickedTrash.name == "trashBio")
                    {
                        StartCoroutine(MoveDropAndPick(2, 1, 3, 0));
                        inputOn = false;
                    }

                    if (hit.collider.gameObject.name == "binProblem" && pickedTrash.name == "trashProblem")
                    {
                        StartCoroutine(MoveDropAndPick(1, 0, 4, 2));
                        inputOn = false;
                    }

                    if (hit.collider.gameObject.name == "binPaper" && pickedTrash.name == "trashPaper")
                    {
                        StartCoroutine(MoveAndDropLast(0, 0, 5, 9));
                        inputOn = false;
                    }

                    if (hit.collider.gameObject.name == "binEnergy" && pickedTrash.name == "trashEnergy")
                    {
                        StartCoroutine(MoveDropAndPick(4, 3, 1, 1));
                        inputOn = false;

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

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(6, false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[116];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kaikki lajiteltu! Hyvää työtä!", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    public void OnEnable()
    {
        trackableFairySpeech.SetActive(false);
        trackableFairy.SetActive(false);
        bearAnim.Rebind();
        bear.SetActive(false);
        trashManager.gameObject.SetActive(false);

        menu.SetActive(false);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);
        bear.SetActive(true);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(6).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(8, true);

            transform.GetChild(0).gameObject.SetActive(true);
            completed = false;

            star.SetActive(false);

            inputOn = false;
            InitializeTask();
        }
        else
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[117];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Roskat on jo lajiteltu! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));
        }
    }

    public void OnDisable()
    {
        if (menu != null)
        {
            menu.SetActive(true);
        }
        if (localFairy != null)
            localFairy.SetActive(true);

        StopAllCoroutines();

        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void InitializeTask()
    {

        for (int i = 0; i < 5; i++)
        {
            if (pickedTrash == trash[i])
            {
                trash[i].transform.SetParent(transform);
                trash[i].transform.position = originalPositions[i];
                trash[i].transform.rotation = originalRots[i];
                trash[i].SetActive(true);
            }
        }

        completed = false;

        StartCoroutine(StartTimeLine());
    }

    IEnumerator StartTimeLine()
    {

        saladBall.SetActive(false);

        bear.transform.LookAt(new Vector3(trackableFairy.transform.position.x, bear.transform.position.y, trackableFairy.transform.position.z));

        Vector3 desPos = Vector3.zero;

        if (questState == 0)
        {
            bear.transform.position = bearOriginalPos;
        }
        else if (questState == 1)
        {
            desPos = new Vector3(bins[4].transform.position.x, bear.transform.position.y, bins[4].transform.position.z - 0.1f);
            bear.transform.position = desPos;
        }
        else if (questState == 2)
        {
            desPos = new Vector3(bins[3].transform.position.x, bear.transform.position.y, bins[3].transform.position.z - 0.1f);
            bear.transform.position = desPos;
        }
        else if (questState == 3)
        {
            desPos = new Vector3(bins[2].transform.position.x, bear.transform.position.y, bins[2].transform.position.z - 0.1f);
            bear.transform.position = desPos;
        }
        else if (questState == 4)
        {
            desPos = new Vector3(bins[1].transform.position.x, bear.transform.position.y, bins[1].transform.position.z - 0.1f);
            bear.transform.position = desPos;
        }

        yield return StartCoroutine(ShowFairySpeechWaitForInput("LAJITELKAA ROSKAT KOSKETTAMALLA OIKEAA JÄTEASTIAA, KUN OTSO NOSTAA ROSKAN.", 3f));

        if (questState == 0)
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }

            bear.transform.position = bearOriginalPos;
            StartCoroutine(MoveAndPick(4));
        }
        else if (questState == 1)
        {

            StartCoroutine(MoveAndPick(3));
        }
        else if (questState == 2)
        {
            StartCoroutine(MoveAndPick(2));
        }
        else if (questState == 3)
        {
            StartCoroutine(MoveAndPick(1));
        }
        else if (questState == 4)
        {
            StartCoroutine(MoveAndPick(0));
        }
        else if (questState == 5)
        {
            StartCoroutine(MoveAndDropLast(0, 0, 0, 0));
        }
    }

    IEnumerator WaitForTrashDrag()
    {
        yield return new WaitUntil(() => trashDragged);
    }

    IEnumerator MoveAndPick(int trashIndex)
    {
        inputOn = false;
        bearAnim.SetBool("walking", true);

        Vector3 desPos = trash[trashIndex].transform.position;
        desPos.y = bear.transform.position.y;

        while (Vector3.Distance(bear.transform.position, desPos) > 0.1f * transform.localScale.x)
        {
            bear.transform.position += (desPos - bear.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            bear.transform.LookAt(desPos);
            yield return null;
        }

        bearAnim.SetBool("walking", false);

        bearAnim.SetTrigger("pickUp");
        yield return new WaitForSeconds(1f);
        trash[trashIndex].transform.SetParent(bearHand);
        trash[trashIndex].transform.localPosition = new Vector3(0, 0.01f, -0.026f);
        pickedTrash = trash[trashIndex];
        soundEffectAS.clip = acm.pollutionRoom1Clips[11];
        soundEffectAS.Play();

        yield return new WaitForSeconds(1.5f);
        inputOn = true;
    }

    IEnumerator MoveAndDrop(int binIndex, int _questState, int soundClipIndex)
    {
        bearAnim.SetBool("walking", true);

        Vector3 desPos = bins[binIndex].transform.position;
        desPos.y = bear.transform.position.y;

        while (Vector3.Distance(bear.transform.position, desPos) > 0.1f * transform.localScale.x)
        {
            bear.transform.position += (desPos - bear.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            bear.transform.LookAt(desPos);
            yield return null;
        }

        bearAnim.SetBool("walking", false);

        bearAnim.SetTrigger("drop");
        trash[binIndex].transform.parent = bear.transform.parent;

        while (trash[binIndex].transform.localPosition.z > 0.1f)
        {
            trash[binIndex].transform.position -= Vector3.up * Time.deltaTime;
            yield return null;
        }
        trash[binIndex].gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);
        soundEffectAS.clip = acm.pollutionRoom1Clips[soundClipIndex];
        soundEffectAS.Play();
        yield return new WaitForSeconds(0.5f);

        pickedTrash = null;
        questState = _questState;
    }

    IEnumerator MoveDropAndPick(int binIndex, int trashIndex, int _questState, int soundClipIndex)
    {
        yield return StartCoroutine(MoveAndDrop(binIndex, _questState, soundClipIndex));
        StartCoroutine(MoveAndPick(trashIndex));
    }

    IEnumerator MoveAndDropLast(int binIndex, int trashIndex, int _questState, int soundClipIndex)
    {
        if (questState < 5)
        {
            yield return StartCoroutine(MoveAndDrop(binIndex, _questState, soundClipIndex));
        }

        bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
        bearAnim.SetTrigger("shrug");
        yield return new WaitForSeconds(2.0f);
        questState = 6;
    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState = 5;
    }
}
