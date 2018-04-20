using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PohjaTargetScript : BaseTargetScript
{
    public bool inputOn;
    public Camera localCamera;

    public GameObject star;
    public GameObject bearSpeechBubble;
    public Animator bearAnim;
    public Animator trackableFairyAnim;
    public AudioSource localFairyAudioSource;
    public AudioSource bearAudioSource;

    private bool completed;
    private int questState;
    private Vector3 bearOriginalPos;
    private bool showNatureOnce;
    private bool rayCastMainCamera;
    private bool rayCastLocalCamera = false;

    private void Awake()
    {
        // set initial references
        //acm = GameObject.Find("ARTargets").GetComponent<AudioClipManagerScript>();
        //localFairy = GameObject.Find("/LocalCamera/Keiju");
        //tss = GameObject.Find("TouchPanel").GetComponent<TouchScreenScript>();
        //engine = GameObject.Find("Engine").GetComponent<GameEngine>();
        //localCamera = GameObject.Find("LocalCamera").GetComponent<Camera>();
        //localFairyAudioSource = localFairy.GetComponent<AudioSource>();
        //trackableFairy = transform.Find("Keiju (1)").gameObject;
        //trackableFairySpeechBubble = trackableFairy.transform.Find("speechBubble").gameObject;
        //bear = transform.Find("otso").gameObject;
        //bearSpeechBubble = bear.transform.Find("speechBubble").gameObject;
        //trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        //bearAnim = bear.GetComponent<Animator>();
        //star = transform.Find("Star").gameObject;
        //bearAudioSource = bear.GetComponent<AudioSource>();
        //fairyAudioSource = trackableFairy.GetComponent<AudioSource>();

        bearOriginalPos = bear.transform.position;
    }

    private void OnEnable()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.SetActive(false);

        bear.transform.position = bearOriginalPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bear.SetActive(false);

        engine.LoadGame();

        star.SetActive(false);

        // checks if the room's cooldown has already passed. The cooldown this checks must correspond with the image target you chose for this room.
        // engine.GetScore(0).updateTimestamp = Energia1, engine.GetScore(1).updateTimestamp = Energia2, engine.GetScore(2).updateTimestamp = Energia3
        // engine.GetScore(3).updateTimestamp = Luonto1, engine.GetScore(4).updateTimestamp = Luonto2, engine.GetScore(5).updateTimestamp = Luonto3
        // engine.GetScore(6).updateTimestamp = Roskat1, engine.GetScore(7).updateTimestamp = Roskat2, engine.GetScore(8).updateTimestamp = Roskat3
        // engine.GetScore(9).updateTimestamp = Sosiaalisuus1, engine.GetScore(10).updateTimestamp = Sosiaalisuus2, engine.GetScore(11).updateTimestamp = Sosiaalisuus3
        //if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(0).updateTimestamp) > 0)
        {
            // if it has, start the room
            completed = false;

            StartCoroutine(StartTimeLine());

            questState = 0;
        }
        //else
        //{
        //    // if not, inform the player the cooldown has not yet passed
        //    StartCoroutine(ShowFairySpeech("This speech is shown if the player tracks the target before engine.CoolDownInSeconds amount of time has passed", 5f));
        //}
    }

    private void OnDisable()
    {
        // activate the local fairy (the fairy which follows the player between tracking images)
        if (localFairy != null)
        {
            localFairy.SetActive(true);
        }

        transform.GetChild(0).gameObject.SetActive(false);
        trackableFairySpeechBubble.SetActive(false);
        StopAllCoroutines();

        // this shows the progress of the game after tracking is stopped and the room is completed. You must select the right questline nature view depending on your chose of an image target.
        // ENERGY: GameObject.Find("NatureViews").transform.GetChild(0).gameObject.SetActive(true);
        // NATURE: GameObject.Find("NatureViews").transform.GetChild(1).gameObject.SetActive(true);
        // POLLUTION: GameObject.Find("NatureViews").transform.GetChild(2).gameObject.SetActive(true);
        // SOCIAL: GameObject.Find("NatureViews").transform.GetChild(3).gameObject.SetActive(true);

        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        completed = false;
        questState = 0;
    }

    private void CompleteLevel()
    {
        completed = true;

        // adds point and cooldown in the engine. Uncomment this and place the right IncrementScoreX corresponding with the image target you chose.
        engine.IncrementScore(0);

        star.SetActive(true);

        trackableFairyAnim.SetTrigger("celebrate");
        bearAnim.SetTrigger("celebrate");
        bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
        StartCoroutine(ShowFairySpeechWaitForInput("Fairy speech.", 1f));
    }

    private void Update()
    {
        // level completed
        if (questState == 1 && completed == false)
        {
            //CompleteLevel();
        }

        // With this you can check if you clicked/touched something in front of main camera or local camera.
        if (inputOn)
        {
            Vector3 inputVector;
            if (TouchScreenScript.GetTouch(out inputVector))
            {
                // raycast from main camera. Used to raycast objects in the room itself
                if (rayCastMainCamera)
                {
                    Ray ray = Camera.main.ScreenPointToRay(inputVector);
                    RaycastHit hit;
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && inputOn)
                    {
                        if (hit.collider.name != null)
                        {
                            StartCoroutine(WalkBear(hit.point, 0));
                            Debug.Log("Something in mainCam is clicked!");
                        }
                    }
                }
                // raycast from local camera. Used to raycast objects in the localObjects layer.
                if (rayCastLocalCamera)
                {
                    Ray ray = localCamera.ScreenPointToRay(inputVector);
                    RaycastHit hit;
                    // check only "localObjects" layer for hit
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, (1 << 8)) && inputOn)
                    {
                        if (hit.collider.name != null)
                        {
                            Debug.Log("Something in localCam is clicked!");
                        }
                    }
                }
            }
        }
    }

    // the level starts with this coroutine
    private IEnumerator StartTimeLine()
    {
        trackableFairyAnim.SetTrigger("celebrate");
        bear.SetActive(true);

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Fairy speech.", 1f));

        inputOn = true;
        rayCastMainCamera = true;

        // add some custom content here! A puzzle perhaps?

        yield return new WaitForSeconds(1f);

        //StartEndTimeLine();
    }

    // funtion to start EndTimeLine coroutine
    public void StartEndTimeLine()
    {
        StartCoroutine(EndTimeLine());
    }

    // the level ends with this coroutine before completion
    private IEnumerator EndTimeLine()
    {
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Fairy speech.", 1f));

        StartCoroutine(IncQuestStateAfterDelay(1.5f));
    }

    // Increases the quest state after certain amount of time
    private IEnumerator IncQuestStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        questState++;
    }

    // makes the fairy walk to certain position, after that the bear can perform actions
    private IEnumerator WalkFairy(Vector3 targetPosition, int action)
    {
        //inputOn = false;

        Vector3 desPos = trackableFairy.transform.position;
        desPos.x = targetPosition.x;

        trackableFairyAnim.SetBool("walking", true);

        trackableFairy.transform.LookAt(desPos);

        Vector3 direction = (desPos - trackableFairy.transform.position).normalized;

        while (Vector3.Distance(trackableFairy.transform.position, desPos) > 0.1f)
        {
            trackableFairy.transform.position += direction * Time.deltaTime * 0.3f * transform.localScale.x;
            yield return null;
        }

        trackableFairy.transform.rotation = Quaternion.identity;
        trackableFairyAnim.SetBool("walking", false);

        if (action == 1)
        {
            // do something cool!
        }
        else if (action == 2)
        {
            // do something cool!
        }
        else if (action == 3)
        {
            // do something cool!
        }

        yield return new WaitForSeconds(1f);
        //inputOn = true;
    }

    // makes the bear walk to certain position, after that the bear can perform actions
    public IEnumerator WalkBear(Vector3 pos, int action)
    {
        //inputOn = false;

        pos.y = bear.transform.position.y;
        bearAnim.SetBool("walking", true);
        while (Vector3.Distance(bear.transform.position, pos) > 0.01f)
        {
            bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            bear.transform.LookAt(pos);
            yield return null;
        }

        bear.transform.rotation = Quaternion.identity;
        bearAnim.SetBool("walking", false);

        if (action == 1)
        {
            // do something cool!
        }
        else if (action == 2)
        {
            // do something cool!
        }
        else if (action == 3)
        {
            // do something cool!
        }

        yield return new WaitForSeconds(1f);
        //inputOn = true;
    }

    // set inputOn to false, wait certain time and set it true
    private IEnumerator InputCooldown(float waitTime)
    {
        inputOn = false;
        yield return new WaitForSeconds(waitTime);
        inputOn = true;
    }
}
