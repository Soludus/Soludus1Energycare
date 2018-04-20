using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KannustamisTargetScript : BaseTargetScript
{
    private bool completed;
    private bool inputOn;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public GameObject drapes;
    public GameObject bearHiding;
    public GameObject bearPeeking;

    public GameObject kannustamisCanvas;
    private int optionsUsed = 0;

    private int questState;

    public Camera localCamera;

    public GameObject star;

    public Animator trackableFairyAnim;

    Vector3 bearOriginalPos;
    Vector3 fairyOriginalPos;
    Quaternion fairyOriginalRot;

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
        fairyOriginalPos = trackableFairy.transform.position;
        fairyOriginalRot = trackableFairy.transform.rotation;
        bearAnim = bear.GetComponent<Animator>();
        bearOriginalPos = bear.transform.position;

        showNatureOnce = false;
    }

    void Update()
    {
        Vector3 inputVector;
        if (TouchScreenScript.GetTouch(out inputVector) && inputOn)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputVector);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                if (hit.collider.gameObject.name == "drapesHiding" && inputOn)
                {
                    questState = 1;
                    StartCoroutine(QuestStateTimeLine(questState, 0));

                    inputOn = false;
                }
            }
        }

        if (questState == 4 && completed == false)
        {
            completed = true;

            engine.IncrementScore(9);

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

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(14, false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[173];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Huikeaa! Kiitoksia sinulle Otso esityksestä, ja teille, lapset, mahtavasta kannustamisesta!", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    // the level starts with this dialogue
    IEnumerator StartTimeLine()
    {
        fairySpeechAS.clip = acm.fairyDialoqueClips[165];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa! Satuimmekin sopivasti näkemään Otson tanssiesityksen! Mutta missäköhän hän on, näettekö häntä?", 1f));
        fairySpeechAS.Stop();

        speechCO = StartCoroutine(ShowFairySpeech("ETSIKÄÄ OTSO!", 5f));

        inputOn = true;
    }

    IEnumerator QuestStateTimeLine(int state, int cheerDecision)
    {
        bearHiding.SetActive(false);
        bearPeeking.SetActive(false);
        drapes.SetActive(true);
        bear.SetActive(true);

        if (optionsUsed == 0)
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }
        }

        if (optionsUsed < 3)
        {
            speechCO = StartCoroutine(ShowFairySpeech("KANNUSTAKAA OTSOA KOSKETTAMALLA VAIHTOEHTOJA", 5f));
            kannustamisCanvas.SetActive(true);
        }

        replayDialog.SetActive(false);

        if (cheerDecision == 1)
        {
            kannustamisCanvas.SetActive(false);
            StopCoroutine(speechCO);

            trackableFairyAnim.SetTrigger("clap");
            fairySpeechAS.clip = acm.fairyDialoqueClips[169];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvä! Taputetaan yhdessä Otsolle!", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            kannustamisCanvas.transform.GetChild(0).gameObject.SetActive(false);
            optionsUsed++;
            questState++;

            if (optionsUsed < 3)
            {
                kannustamisCanvas.SetActive(true);
            }
        }
        else if (cheerDecision == 2)
        {
            kannustamisCanvas.SetActive(false);
            StopCoroutine(speechCO);

            trackableFairyAnim.SetTrigger("cheer");
            fairySpeechAS.clip = acm.fairyDialoqueClips[170];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa, tuuletetaan oikein kunnolla!", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            kannustamisCanvas.transform.GetChild(1).gameObject.SetActive(false);
            optionsUsed++;
            questState++;

            if (optionsUsed < 3)
            {
                kannustamisCanvas.SetActive(true);
            }
        }
        else if (cheerDecision == 3)
        {
            kannustamisCanvas.SetActive(false);
            StopCoroutine(speechCO);

            trackableFairyAnim.SetTrigger("celebrate");
            fairySpeechAS.clip = acm.fairyDialoqueClips[171];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Oivallista, halataan toisiamme, niin Otso huomaa, ettei ole mitään pelättävää! Sopiiko sinulle Otso, että minä tulen halaamaan sinua?", 1f));
            fairySpeechAS.Stop();

            replayDialog.SetActive(false);

            bearAnim.SetTrigger("nod");
            yield return new WaitForSeconds(2f);

            yield return StartCoroutine(WalkFairy(bear.transform.position));

            kannustamisCanvas.transform.GetChild(2).gameObject.SetActive(false);
            optionsUsed++;
            questState++;


            if (optionsUsed < 3)
            {
                kannustamisCanvas.SetActive(true);
            }
        }

        if (optionsUsed >= 3)
        {
            yield return new WaitForSeconds(0.5f);

            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));

            yield return new WaitForSeconds(1f);

            // bear's "dance"
            bearAnim.Play("handwashanim");
            yield return new WaitForSeconds(2f);
            bearAnim.Play("handwashanim");
            yield return new WaitForSeconds(2f);
            bearAnim.Play("handwashanim");
            yield return new WaitForSeconds(2f);
            bearAnim.SetTrigger("celebrate");
            yield return new WaitForSeconds(3f);

            bearAnim.SetTrigger("nod");
            yield return new WaitForSeconds(2f);

            GameObject.Find("MusicManager").GetComponent<MusicManager>().ChangeMusicAfterLoop(13);

            trackableFairyAnim.SetTrigger("clap");
            fairySpeechAS.clip = acm.fairyDialoqueClips[172];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa, Otso! Olipa oikein mainio tanssi! Taputetaan vielä yhdessä Otsolle kiitokseksi!", 1f));
            fairySpeechAS.Stop();

            StartCoroutine(IncQuestStateAfterDelay());
        }
    }

    public void ApplaudDecision()
    {
        kannustamisCanvas.SetActive(false);

        StartCoroutine(QuestStateTimeLine(questState, 1));
    }

    public void CheeringDecision()
    {
        StopCoroutine(speechCO);

        kannustamisCanvas.SetActive(false);

        StartCoroutine(QuestStateTimeLine(questState, 2));
    }

    public void HugDecision()
    {
        kannustamisCanvas.SetActive(false);

        StartCoroutine(QuestStateTimeLine(questState, 3));
    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState++;
    }

    private void OnEnable()
    {
        menu.SetActive(false);
        trackableFairy.transform.position = fairyOriginalPos;
        trackableFairy.transform.rotation = fairyOriginalRot;

        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.SetActive(true);
        bear.transform.position = bearOriginalPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bear.SetActive(false);

        engine.LoadGame();

        star.SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(9).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(17, true);

            bearHiding.SetActive(true);
            bearPeeking.SetActive(false);
            drapes.SetActive(false);

            completed = false;
            inputOn = false;

            StartCoroutine(QuestStateTimeLine(questState, 0));
        }
        else
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[174];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Otso rohkaistui jo, voitte kokeilla tätä rastia huomenna uudestaan!", 5f));

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
        kannustamisCanvas.SetActive(false);
        trackableFairyAnim.SetBool("walking", false);
        trackableFairy.SetActive(false);
        bear.SetActive(false);
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
        StopAllCoroutines();
        tss.touchScreenTouched = false;
        tss.allowInput = false;
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(3).gameObject.SetActive(true);
        }
    }

    IEnumerator WalkFairy(Vector3 pos)
    {
        pos.x -= 0.25f * transform.localScale.x;
        pos.z -= 0.125f * transform.localScale.x;

        trackableFairyAnim.SetBool("walking", true);

        while (Vector3.Distance(trackableFairy.transform.position, pos) > 0.01f * transform.localScale.x)
        {

            trackableFairy.transform.position += (pos - trackableFairy.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            trackableFairy.transform.LookAt(pos);
            yield return null;
        }

        trackableFairyAnim.SetBool("walking", false);
        yield return new WaitForSeconds(0.5f);

        trackableFairyAnim.SetTrigger("hug");
        bearAnim.SetTrigger("hug");
        yield return new WaitForSeconds(3f);

        trackableFairyAnim.SetBool("walking", true);

        while (Vector3.Distance(trackableFairy.transform.position, fairyOriginalPos) > 0.01f * transform.localScale.x)
        {

            trackableFairy.transform.position += (fairyOriginalPos - trackableFairy.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            trackableFairy.transform.LookAt(fairyOriginalPos);
            yield return null;

        }
        trackableFairyAnim.SetBool("walking", false);

        trackableFairy.transform.position = fairyOriginalPos;
        trackableFairy.transform.rotation = fairyOriginalRot;
        yield return new WaitForSeconds(1f);
    }
}
