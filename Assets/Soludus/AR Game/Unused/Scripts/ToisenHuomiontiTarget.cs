using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;

public class ToisenHuomiontiTarget : MonoBehaviour, ITrackableEventHandler {

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

	public GameObject huomiointiCanvas;

	public GameObject toy1;
	public GameObject toy2;

	public Camera localCamera;
	public GameObject star;

	public Animator trackableFairyAnim;

	Vector3 bearOriginalPos;

	// Use this for initialization
	void Start()
	{

		mTrackableBehaviour = GetComponent<TrackableBehaviour>();
		mTrackableBehaviour.RegisterTrackableEventHandler(this);
		completed = false;

		// testing quest state
		questState = 0;

		// init
		trackableFairyAnim = trackableFairy.GetComponent<Animator>();
		bearAnim = bear.GetComponent<Animator>();
		bearOriginalPos = bear.transform.position;
	}

	// Update is called once per frame
	void Update()
	{

		// level completed
		if (questState == 1 && completed == false)
		{
			completed = true;

			//engine.IncrementScore14 ();

			star.SetActive(true);

			trackableFairyAnim.SetTrigger("celebrate");
//			bearAnim.SetTrigger("celebrate");
//			bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
			//StartCoroutine(ShowFairySpeechWaitForInput("Hyvin tehty!"));
		}
	}

	// the level starts with this dialogue
	IEnumerator StartTimeLine()
	{
        // HUOM! Vasta Erilaisuustehtävän jälkeen!

        trackableFairyAnim.SetTrigger("celebrate");
		bear.SetActive(true);

		yield return StartCoroutine(ShowFairySpeechWaitForInput("Otsosta ja Pandasta on tullut jo ystäviä, Panda on tullut Otson luokse leikkimään. Mutta mitäs nyt?"));

        // Otso menee Pandan luokse, ottaa toisen auton Pandalta ja alkaa leikkiä sillä. Panda painaa päänsä.

        yield return StartCoroutine(ShowFairySpeechWaitForInput("No mutta Otso! Olikos tuo nyt kiltisti tehty?"));

        // Otso pysähtyy ja kallistaa päänsä. Pandakin on ymmällään.

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Annapa se auto nyt ihan ensimmäiseksi takaisin Pandalle."));

        // Otso antaa Pandalle auton takaisin. Panda nyökkää kiitokseksi.

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Jos haluaa leikkiä jonkun kanssa, pitää muistaa ensin pyytää lupa! Yhdessä leikkiminen on kaikille paljon hauskempaa, kun otetaan toiset huomioon."));
		yield return StartCoroutine(ShowFairySpeechWaitForInput("Sovitaan yhdessä, mitä ja miten leikitään, eikä oteta toisilta tavaroita kysymättä."));
		yield return StartCoroutine(ShowFairySpeechWaitForInput("Jos joku näyttää jäävän yksin, on hyvä kysyä, haluaisiko hän leikkiin mukaan, vai haluaako hän mieluummin leikkiä itsekseen kaikessa rauhassa."));
		yield return StartCoroutine(ShowFairySpeechWaitForInput("On toki kilttiä jakaa lelut toisten kanssa, mutta ei niitä sovi väkisin mennä ottamaan."));

		yield return StartCoroutine(ShowFairySpeechWaitForInput("Miten te lapset tykkäättte leikkiä yhdessä? Miten te sovitte leikeistä ja leluista?"));

        // OHJE: KESKUSTELKAA: MITEN TE LEIKITTE YHDESSÄ? MITEN SOVITTE LEIKEISTÄ JA LELUISTA?

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa, hyviä ajatuksia! Otso haluaisi vieläkin leikkiä Pandan kanssa. Opetetaankos hänelle yhdessä, miten hänen pitäisi nyt toimia?"));
		yield return StartCoroutine(ShowFairySpeechWaitForInput("Ottaako hän auton?"));
        // (Näytölle kuva auton ottamisesta käpälällä.)
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Vai kysyykö hän lupaa tulla mukaan leikkiin?"));
        // (Ottamiskuvan viereen kuva kysymysmerkistä puhekuplassa ?.)

        // OHJE: OPETTAKAA OTSOLLE MITEN TOISTEN KANSSA KUULUU LEIKKIÄ KOSKETTAMALLA OIKEAA KUVAA.

        huomiointiCanvas.SetActive (true);

		huomiointiCanvas.transform.GetChild (0).gameObject.SetActive (true);
		huomiointiCanvas.transform.GetChild (1).gameObject.SetActive (true);
		huomiointiCanvas.transform.GetChild (2).gameObject.SetActive (false);
		huomiointiCanvas.transform.GetChild (3).gameObject.SetActive (false);

	}

	public void StartAskAgain() {

		StartCoroutine (AskAgain ());

	}

	IEnumerator AskAgain() {

		huomiointiCanvas.SetActive (false);
		yield return StartCoroutine(ShowFairySpeechWaitForInput("Lelun ottaminen kysymättä ei kyllä minusta tunnu oikein mukavalta. Pandalle voi tulla paha mieli Kokeilkaapa uudelleen!"));
		huomiointiCanvas.SetActive (true);
	}

	public void StartEndTimeLine() {

		StartCoroutine (EndTimeLine());

	}

	IEnumerator EndTimeLine() {

		huomiointiCanvas.SetActive (false);

		yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa! Otson kannattaa kysyä ensin lupaa leikkiä toisella autolla Pandan kanssa."));

		yield return StartCoroutine(ShowFairySpeechWaitForInput("Entäpä mitä Panda voisi nyt tehdä?"));
		yield return StartCoroutine(ShowFairySpeechWaitForInput("Pitääkö hän molemmat autot itse?"));
        // (Näytölle kuva autojen pitämisestä.)
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Vai antaako hän Otson leikkiä toisella?"));
        // (Pitämiskuvan viereen kuva auton ojentamisesta.)

        // OHJE: OPETTAKAA PANDALLE MITEN TOISTEN KANSSA KUULUU LEIKKIÄ KOSKETTAMALLA OIKEAA KUVAA.

        huomiointiCanvas.SetActive (true);
		huomiointiCanvas.transform.GetChild (0).gameObject.SetActive (false);
		huomiointiCanvas.transform.GetChild (1).gameObject.SetActive (false);
		huomiointiCanvas.transform.GetChild (2).gameObject.SetActive (true);
		huomiointiCanvas.transform.GetChild (3).gameObject.SetActive (true);

	}

	public void StartPandaAskAgain() {

		StartCoroutine (PandaAskAgain ());

	}

	IEnumerator PandaAskAgain() {

		huomiointiCanvas.SetActive (false);
		yield return StartCoroutine(ShowFairySpeechWaitForInput("Hmm.. Olisi kyllä tosi kilttiä päästää toinen mukaan leikkiin, jos tämä kysyy."));
		yield return StartCoroutine(ShowFairySpeechWaitForInput("Ja lelut olisi ainakin reilua jakaa muiden kanssa, vaikka muuten haluaisikin leikkiä hetken ihan itsekseen. Kokeilkaapa uudelleen!"));
		huomiointiCanvas.SetActive (true);
	}

	public void StartLastTimeLine() {

		StartCoroutine (LastTimeLine());

	}

	IEnumerator LastTimeLine() {

		huomiointiCanvas.SetActive (false);

		yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa! On tosi ystävällistä ottaa toiset mukaan leikkiin ja ainakin jakaa lelut muiden kanssa, vaikka joskus haluaisikin leikkiä hetkisen yksin. Hyvin tehty!"));

        // Otso alkaa leikkiä Pandan kanssa tämän ojentamalla autolla.

        StartCoroutine(IncQuestStateAfterDelay ());

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

			//if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(13).updateTimestamp) > 0 && engine.GetScore(13).score < 5)
			//{


			//	completed = false;

			//	StartCoroutine(StartTimeLine());

			//	questState = 0;
			//}
			//else
			//{
			//	StartCoroutine(ShowFairySpeech("Leikit on leikitty tältä päivältä. Tarkista huomenna uudestaan!", 5f));

			//}

		}
		else
		{
			transform.GetChild(0).gameObject.SetActive(false);
			localFairy.SetActive(true);
			huomiointiCanvas.SetActive (false);
			trackableFairySpeechBubble.SetActive (false);
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
}
