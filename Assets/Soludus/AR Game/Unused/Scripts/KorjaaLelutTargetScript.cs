using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;

public class KorjaaLelutTargetScript : MonoBehaviour, ITrackableEventHandler
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

	public GameObject korjaaLelutCanvas;

	public Transform bearHand;
	public GameObject toys;
	private GameObject toy;
	private GameObject pickedToy;

	private Vector3 toysOriginalPos;
	private Quaternion toysOriginalRot;

	public GameObject trashbin;

	public Camera localCamera;

	public GameObject star;

	public Animator trackableFairyAnim;

	Vector3 bearOriginalPos;

	// Use this for initialization
	void Start ()
	{

		mTrackableBehaviour = GetComponent<TrackableBehaviour> ();
		mTrackableBehaviour.RegisterTrackableEventHandler (this);
		completed = false;
		;

		// testing quest state
		questState = 0;

		// init
		trackableFairyAnim = trackableFairy.GetComponent<Animator> ();
		bearAnim = bear.GetComponent<Animator> ();
		bearOriginalPos = bear.transform.position;
		toysOriginalPos = toys.transform.position;
		toysOriginalRot = toys.transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		// level completed
		if (questState == 7 && completed == false) {
			completed = true;

			engine.IncrementScore(8);

			star.SetActive (true);

			trackableFairyAnim.SetTrigger ("celebrate");
			bearAnim.SetTrigger ("celebrate");
			bear.transform.LookAt (new Vector3 (Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
			StartCoroutine (ShowFairySpeechWaitForInput ("Kerrassaan mainiota! Kaikki tavarat on käyty läpi, hienoa lapset ja Otso!"));
		}

	}

	IEnumerator StartTimeLine ()
	{

		trackableFairyAnim.SetTrigger ("celebrate");
		bear.SetActive (true);

		toys.transform.SetParent (bearHand);
		toys.transform.localPosition = new Vector3 (0, 0.01f, -0.026f);

        //bearAnim.SetTrigger ("startWithPickup");
        //yield return null;
        bearAnim.Play("pickupidleanim");

		yield return StartCoroutine (WalkBear (trashbin.transform.position, 1));
		bear.transform.LookAt (trackableFairy.transform.position);

		StartCoroutine (ScaredAndDropToys ());

		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Otso!"));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Oletko sinä heittämässä tuon kaiken menemään?"));

        // Otso nyökyttää ja poimii lattialta rikkinäisen nallen näyttääkseen Hipalle ja lapsille, että se on rikki
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);

		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Mutta millä sinä sitten leikit? Et suinkaan vain aio ostaa kaupasta uutta nallea?"));

        // Otso nyökyttää ja kallistaa päänsä.
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);
        bearAnim.SetTrigger("wondering");
        yield return new WaitForSeconds(3f);

        yield return StartCoroutine (ShowFairySpeechWaitForInput ("Mutta tuon nallenhan voi aivan hyvin vielä korjata! Tarvitsemme vain hieman lankaa ja neulaa."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Monet noista hieman rikkinäisistä tavaroista voi korjata tai paikata."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Aina ei kannata ostaa uutta, sillä mitä enemmän tavaraa ostaa, sitä enemmän sitä päätyy lopulta roskiin ja jätteeksi, joka rasittaa luontoa."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Uusien lelujen tekemiseen kuluu myös energiaa, siksikin on parempi pitää tavaroista huolta ja käyttää ne kunnolla loppuun kuin aina ostaa uusia."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Kaikkea ei tietenkään aina osaa korjata itse, mutta silloin voi pyytää apua, tai vaikka viedä lelun nukke- ja nallesairaalaan."));

		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Onko teillä lapset joskus mennyt jokin lelu rikki?"));

		// OHJE: keskustelkaa onko teillä mennyt jokin lelu rikki

		// Otso pitelee yhä nallea kämmenissään ja katselee sitä
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("On kyllä kurjaa kun rakas esine menee rikki. Mutta joskus kysymys ei olekaan siitä, että lelu menisi rikki, vaan siitä, että se ei enää kiinnosta."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Jos lelu on jäänyt vanhaksi eikä sillä enää leiki, mutta se on muuten ihan kunnossa, sen voi viedä vaikka kirpputorille tai antaa jollekin tutulle."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Myös esimerkiksi pieneksi jääneet, ehjät vaatteet kannattaa viedä kirpputorille tai keräykseen, jolloin joku muu voi löytää ne vielä käyttöön."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Itsellekin kannattaa etsiä uusia tavaroita ensimmäiseksi kirpputoreilta!"));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Oletteko te lapset joskus saaneet jonkin käytetyn tavaran tai vaatteen kirpputorilta tai vanhemmalta sisarukselta tai kaverilta?"));

		// OHJE: keksustelkaa mitä käytettyjä tavaroita tai vaatteita olette saaneet omaksenne?

		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Minä tuumin, että toisen vanha esine voikin olla toiselle aarre!"));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Vasta jos jokin lelu tai vaate on aivan puhki kulutettu tai korjaamattomasti rikki, kannattaa se heittää pois."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Me autamme sinua noiden tavaroiden kanssa, Otso, jotta tiedät, mitkä voit korjata ja mitkä oikeasti pitää heittää jo menemään."));

        // Otso nyökyttää innoissaan
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);

        questState++;

		// nalle-lelu
		StartCoroutine (QuestTimeLine (questState));

	}

	IEnumerator QuestTimeLine (int state)
	{

		// nalle
		if (state == 1) {

			yield return StartCoroutine (PickUpToy (0));
					
			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Aloitetaanpa sitten tuosta nallesta, siitä on sauma vähän ratkennut ja toinen silmä repsottaa. Miettikää yhdessä, korjaammeko, viemmekö kierrätykseen vai heitämmekö roskiin?"));

			// OHJE: valitkaa mitä lelulle tehdään
			// Näytölle kuva nukesta ja vaihtoehdot: Korjataan/Kierrätetään/Roskiin

			korjaaLelutCanvas.SetActive (true);

			// Otso poimii potkulaudan

			// potkulauta
		} else if (state == 2) {

			yield return StartCoroutine (DropPickedToy ());
			yield return StartCoroutine (PickUpToy (1));

			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Entäpä tämä potkulauta, josta on irronnut pyörä? Korjataan, kierrätetään vai heitetään pois?"));

			// OHJE: valitkaa mitä lelulle tehdään
			// Näytölle kuva potkulaudasta ja vaihtoehdot: Korjataan/Kierrätetään/Roskiin

			korjaaLelutCanvas.SetActive (true);

			// Otso poimii tussikynän

			// tussi
		} else if (state == 3) {

			yield return StartCoroutine (DropPickedToy ());
			yield return StartCoroutine (PickUpToy (2));
					
			yield return StartCoroutine (ShowFairySpeechWaitForInput ("No mutta mitäpä sanotte tästä vihreästä tussikynästä, joka on kuivunut niin ettei siitä tule enää väriä?"));

			// OHJE: valitkaa mitä lelulle tehdään
			// Näytölle kuva tussista ja vaihtoehdot: Korjataan/Kierrätetään/Roskiin

			korjaaLelutCanvas.SetActive (true);

			// Otso poimii supersankaripuvun

			// supersankaripuku
		} else if (state == 4) {

			yield return StartCoroutine (DropPickedToy ());
			//yield return StartCoroutine (PickUpToy (3));
					
			yield return StartCoroutine (ShowFairySpeechWaitForInput ("No mutta! Näyttää siltä, että Otson supersankaripuku on jäänyt hänelle vähän pieneksi, Mitä sille kannattaisi tehdä?"));

			// OHJE: valitkaa mitä lelulle tehdään
			// Näytölle kuva supersankaripuvusta ja vaihtoehdot: Korjataan/Kierrätetään/Roskiin

			korjaaLelutCanvas.SetActive (true);

			// Otso poimii palapelin laatikon

			// palapeli
		} else if (state == 5) {

			yield return StartCoroutine (DropPickedToy ());
			//yield return StartCoroutine (PickUpToy (4));
					
			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Hyvät hyssykät! Joku on pureskellut tämän palapelin palat ihan muodottomiksi."));
            // Otso painaa päänsä
            bearAnim.SetTrigger("headDown");
            yield return new WaitForSeconds(2f);

            yield return StartCoroutine (ShowFairySpeechWaitForInput ("Mitäköhän tälle tehtäisi?"));

			// OHJE: valitkaa mitä lelulle tehdään
			// Näytölle kuva palapelilaatikosta ja vaihtoehdot: Korjataan/Kierrätetään/Roskiin

			korjaaLelutCanvas.SetActive (true);

			// pehmoponi
		}
//		} else if (state == 6) {
//
//			yield return StartCoroutine (DropPickedToy ());
//			//yield return StartCoroutine (PickUpToy (5));
//					
//			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Voi pehmoponia, sillähän on mennyt harja ja häntä ihan takkuun. Mietikää vielä yhdessä, mitä sille pitäisi tehdä!"));
//
//			// OHJE: valitkaa mitä lelulle tehdään
//			// Näytölle kuva pehmoponista ja vaihtoehdot: Korjataan/Kierrätetään/Roskiin
//
//			korjaaLelutCanvas.SetActive (true);
//
//		}
	}

	public void StartDecisionResponse ()
	{

		korjaaLelutCanvas.SetActive (false);

		StartCoroutine (DecisionResponse ());

	}

	IEnumerator DecisionResponse ()
	{

		if (questState == 1) {
			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Hyvin ajateltu!"));

		} else if (questState == 2) {
			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Mainiosti mietitty!"));

		} else if (questState == 3) {
			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Selvä, tehdään niin sitten!"));

		} else if (questState == 4) {
			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Oivallisesti tuumittu!"));
		}
//		} else if (questState == 5) {
//			yield return StartCoroutine (ShowFairySpeechWaitForInput ("Hyvä, loppusuoralla ollaan!"));
//		}

		questState++;
		if (questState < 6) {
			StartCoroutine (QuestTimeLine (questState));
		} else {
			yield return StartCoroutine (DropPickedToy ());
			StartCoroutine (IncQuestStateAfterDelay ());
		}

	}

	IEnumerator IncQuestStateAfterDelay ()
	{
		yield return new WaitForSeconds (1.5f);
		questState++;
	}

	public IEnumerator WalkBear (Vector3 pos, int action)
	{
		pos.y = bear.transform.position.y;

		// positioning for trashbin
		if (action == 1) {
			pos.x -= 0.25f;
			pos.z += 0.125f;
		}

		bearAnim.SetBool ("walking", true);
		while (Vector3.Distance (bear.transform.position, pos) > 0.01f) {
			bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f;
			bear.transform.LookAt (pos);
			yield return null;
		}

		bearAnim.SetBool ("walking", false);
	}

	IEnumerator ScaredAndDropToys ()
	{
		
		bearAnim.SetTrigger ("scare");

		yield return new WaitForSeconds (0.3f);

		toys.transform.parent = gameObject.transform;
		while (Vector3.Distance (toys.transform.position, toysOriginalPos) > 0.01f) {
			toys.transform.position += (toysOriginalPos - toys.transform.position).normalized * Time.deltaTime * 0.2f;
			yield return null;
		}

		toys.transform.position = toysOriginalPos;
		toys.transform.rotation = toysOriginalRot;

	}

	IEnumerator PickUpToy (int index)
	{
		
		bearAnim.SetTrigger ("pickUp");
		yield return new WaitForSeconds (0.2f);

		toy = toys.transform.GetChild (index).gameObject;
		pickedToy = Instantiate (toy);
		pickedToy.transform.SetParent (bearHand);
		pickedToy.transform.localPosition = new Vector3 (0, 0.01f, -0.026f);
		toy.SetActive (false);

		yield return new WaitForSeconds (0.5f);
	}

	IEnumerator DropPickedToy() {

		bearAnim.SetTrigger ("drop");
		yield return new WaitForSeconds (0.5f);
		Destroy (pickedToy);
		yield return new WaitForSeconds (0.5f);

	}

	// method of Vuforia - the level starts when the AR camera tracks the target, and level goes away when AR camera is no longer tracking the target
	public void OnTrackableStateChanged (TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
		    newStatus == TrackableBehaviour.Status.TRACKED ||
		    newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {

			transform.GetChild (0).gameObject.SetActive (true);
			localFairy.SetActive (false);

			bear.transform.position = bearOriginalPos;
			bear.transform.LookAt (trackableFairy.transform.position);
			//bearAnim.SetBool ("walking", false);
			bear.SetActive (false);

			engine.LoadGame ();

			star.SetActive (false);



			if (System.DateTime.Compare (System.DateTime.Now.AddSeconds (-engine.CoolDownInSeconds), engine.GetScore(8).updateTimestamp) > 0 && engine.GetScore(8).score < 5) {


				completed = false;

				for (int i = 0; i < toys.transform.childCount; i++) {
					toys.transform.GetChild (i).gameObject.SetActive (true);
				}
				StartCoroutine (StartTimeLine ());

				questState = 0;
			} else {
				StartCoroutine (ShowFairySpeech ("Kädet ovat puhtaat. Tarkista huomenna uudestaan!", 5f));

			}

		} else {
			transform.GetChild (0).gameObject.SetActive (false);
			localFairy.SetActive (true);
			trackableFairySpeechBubble.SetActive (false);
			korjaaLelutCanvas.SetActive (false);
			Destroy (pickedToy);
			bearAnim.SetBool ("walking", false);
			bearAnim.Play ("idleanim");

			StopAllCoroutines ();
		}
	}

	IEnumerator ShowFairySpeech (string inputText, float showDuration)
	{

		trackableFairySpeechBubble.SetActive (true);
		trackableFairySpeechBubble.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = inputText;
		yield return new WaitForSeconds (showDuration);
		trackableFairySpeechBubble.SetActive (false);
	}

	IEnumerator ShowFairySpeechWaitForInput (string inputText)
	{
		trackableFairySpeechBubble.SetActive (true);
		trackableFairySpeechBubble.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = inputText;

		yield return StartCoroutine (WaitForInput ());

		trackableFairySpeechBubble.SetActive (false);
	}

	IEnumerator WaitForInput ()
	{
		yield return new WaitForSeconds (1f);
		while (Input.touchCount == 0 && !Input.GetMouseButtonDown (0)) {
			yield return null;
		}
	}
}
