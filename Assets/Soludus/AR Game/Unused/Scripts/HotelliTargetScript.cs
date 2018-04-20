using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;

public class HotelliTargetScript : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour mTrackableBehaviour;
    public GameObject trackableFairy;
    public GameObject localFairy;
    public GameObject trackableFairySpeechBubble;

    private bool completed;

    public GameEngine engine;

    public GameObject bear;
    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    private int questState;

    public GameObject bugHotel;
    public GameObject bugHotelPrefab;
    public GameObject hotelPosition;

    public GameObject bugSpray;

    public Camera localCamera;
    public GameObject star;

    public Animator trackableFairyAnim;

    Vector3 bearOriginalPos;
    Vector3 fairyOriginalPos;
    Quaternion fairyOriginalRot;

    // Use this for initialization
    void Start()
    {

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        mTrackableBehaviour.RegisterTrackableEventHandler(this);
        completed = false; ;

        // testing quest state
        questState = 0;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();
        bearOriginalPos = bear.transform.position;
        fairyOriginalPos = trackableFairy.transform.position;
        fairyOriginalRot = trackableFairy.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {

        // level completed
        if (questState == 4 && completed == false)
        {
            completed = true;

            //engine.IncrementScore13 ();

            star.SetActive(true);

            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");
            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
            StartCoroutine(ShowFairySpeechWaitForInput("Hienoa! Nyt Otson puutarha alkaa varmasti kukoistaa, ja ötökkäystävätkin viihtyvät!"));
        }
    }

    // the level starts with this dialogue
    IEnumerator StartTimeLine()
    {

        trackableFairyAnim.SetTrigger("celebrate");
        bear.SetActive(true);
        bearAnim.SetBool("thinking", true);
        bearAnim.Play("thinkinganim");

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mitäköhän se otso pohtii? Hän näyttää olevan huolissaan."));
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Oletko sinä, Otso, huolestunut maa-artisokistasi?"));

        bearAnim.SetBool("thinking", false);
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(WalkFairy(bear.transform.position - new Vector3(0.25f, 0, 0)));
        trackableFairy.transform.LookAt(new Vector3(Camera.main.transform.position.x, trackableFairy.transform.position.y, Camera.main.transform.position.z));

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hmm. Ötökät ovat päässeet kasvien kimppuun, ne taitavat olla kovasti mieltyneet sinun artisokkiisi"));
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Harmi vaan, että kukat eivät ole houkutelleet puutarhaan hyödyllisiä ötökkäkavereita vaan tuhohyönteisiä."));

        // Otso nostaa esille myrkkypullon
        bearAnim.SetTrigger("pickUp");
        yield return new WaitForSeconds(1f);
        bugSpray.SetActive(true);
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(WalkFairy(fairyOriginalPos));
        trackableFairy.transform.rotation = fairyOriginalRot;

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Ei ei, Otso, Ei myrkkyjä!"));

        // Otso pudottaa pullon ja kallistaa päätään
        bearAnim.SetTrigger("drop");
        yield return new WaitForSeconds(1f);
        bugSpray.SetActive(false);
        yield return new WaitForSeconds(1f);
        bearAnim.SetTrigger("wondering");
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Myrkyt ovat kyllä haitaksi tuhohyönteisille, mutta ne ovat haitallisia myös hyödyllisille hyönteisystäville ja sinulle itsellesikin!"));
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Onneksi on olemassa paljon hauskempikin ratkaisu sinun artisokkaongelmaasi!"));
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Myrkyttämisen sijasta sinun kannattaa houkutella puutarhaasi pieniä tuholaistorjujia: pistiäisiä ja leppäkerttuja."));
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Sen lisäksi voit kutsua vaikkapa erakkomehiläisiä hoitamaan marjapensaiden pölyttämisen, jotta saat niistäkin satoa."));

        // Otso näyttää mietteliäältä
        bearAnim.SetBool("thinking", true);
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Sinä taidat miettiä, millä konstilla saisit houkuteltua noita pikku työntekijöitä puutarhaasi?"));

        bearAnim.SetBool("thinking", false);
        yield return new WaitForSeconds(1f);
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mitä te lapset tuumitte? Tiedättekö onko teidän puutarhassanne hyötyötököitä? Miksi ne viihtyvät siellä, tai miten ne voisi saada viihtymään?"));

        // OHJE: keskustelua hyötyötököistä

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Yksi hyvä vaihtoehto on rakentaa ötököille hotelli, jonka sitten laittaa oikein mukavaksi ja sopivaksi juuri sellaisille hyönteisystäville, joita haluaa houkuttaa."));
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Esimerkiksi kuivat kaarnanpalat sopivat kodiksi leppäkertuille, jotka syövät kasveja tuhoavia kirvoja."));

        // näytölle kuva kaarnanpaloista sekä leppäkertusta

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Myös monet pistiäiset herkuttelevat kirvoilla ja muillakin tuholaisilla ja toimivat myös pölyttäjinä. Hyötypistiäisiä voi houkutella kuivilla korrenpätkillä."));

        // näytölle kuva korsista sekä petopistiäisestä

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Pölyttäjinä verrattomat erakkomehiläiset viihtyvät esimerkiksi puunkoloissa."));

        // näytölle kuva rei'itetystä puunpalasta sekä erakkomehiläisestä

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kuulkaapa lapset! Voisitteko te rakentaa Otsolle ötökkähotellin, jossa viihtyisivät niin leppäkertut, pistiäiset kuin erakkomehiläisetkin?"));

        bugHotel.SetActive(true);

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Vetäkää kullekin ötökälle sopivat sisustustarvikkeet niiden huoneisiin!"));

        // OHJE: Vetäkää ötököille sopivat sisustustarvikkeet niiden huoneisiin.

    }

    public void StartEndTimeLine()
    {

        EndTimeLine();

    }

    void EndTimeLine()
    {

        bugHotel.SetActive(false);

        questState = 4;

    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState++;
    }

    // method of Vuforia - the level starts when the AR camera tracks the target, and level goes away when AR camera is no longer tracking the target
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {

            transform.GetChild(0).gameObject.SetActive(true);
            localFairy.SetActive(false);

            bear.transform.position = bearOriginalPos;
            bear.transform.LookAt(trackableFairy.transform.position);
            bear.SetActive(false);

            engine.LoadGame();

            star.SetActive(false);

            //if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(12).updateTimestamp) > 0 && engine.GetScore(12).score < 5)
            //{

            //	completed = false;
            //	Destroy (spawnedHotel);
            //             bugSpray.SetActive(false);
            //             trackableFairy.transform.position = fairyOriginalPos;
            //             trackableFairy.transform.rotation = fairyOriginalRot;

            //             StartCoroutine(StartTimeLine());

            //	questState = 0;
            //}
            //else
            //{
            //	StartCoroutine(ShowFairySpeech("Patsas on vielä pystyssä. Tarkista huomenna uudestaan!", 5f));

            //}
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            localFairy.SetActive(true);
            trackableFairySpeechBubble.SetActive(false);
            bugHotel.SetActive(true);
            bugHotel.GetComponent<BugHotelManager>().RestartBugHotel();
            bugHotel.SetActive(false);
            StopAllCoroutines();
        }
    }

    public IEnumerator ShowFairySpeech(string inputText, float showDuration)
    {

        trackableFairySpeechBubble.SetActive(true);
        trackableFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;
        yield return new WaitForSeconds(showDuration);
        trackableFairySpeechBubble.SetActive(false);
    }


    public IEnumerator ShowFairySpeechWaitForInput(string inputText)
    {
        trackableFairySpeechBubble.SetActive(true);
        trackableFairySpeechBubble.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = inputText;

        yield return StartCoroutine(WaitForInput());

        trackableFairySpeechBubble.SetActive(false);
    }


    IEnumerator WaitForInput()
    {
        yield return new WaitForSeconds(1f);
        while (Input.touchCount == 0 && !Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
    }

    IEnumerator WalkFairy(Vector3 targetPosition)
    {
        Vector3 desPos = trackableFairy.transform.position;
        desPos.x = targetPosition.x;
        desPos.z = targetPosition.z;

        Debug.Log(desPos);

        trackableFairy.transform.LookAt(desPos);

        Vector3 direction = (desPos - trackableFairy.transform.position).normalized;

        while (Vector3.Distance(trackableFairy.transform.position, desPos) > 0.1f)
        {
            trackableFairy.transform.position += direction * Time.deltaTime * 0.3f;
            yield return null;
        }
    }
}
