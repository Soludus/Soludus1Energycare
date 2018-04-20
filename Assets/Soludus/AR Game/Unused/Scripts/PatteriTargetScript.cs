using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;

public class PatteriTargetScript: MonoBehaviour, ITrackableEventHandler {

	private TrackableBehaviour mTrackableBehaviour;
	public GameObject trackableFairy;
	public GameObject localFairy;
	public GameObject trackableFairySpeechBubble;

	private bool completed;

	public GameEngine engine;

	public GameObject bear;
	public GameObject bearSpeechBubble;
	public Animator bearAnim;

	Vector3 bearOriginalPos;

	public bool inputOn;

	public GameObject star;

	public Animator trackableFairyAnim;

	public GameObject window, door, radiator;

	public int doorTimesOpened, windowTimesOpened;
	private int questState;

	public Material radiatorMat;
	public int radiatorOverHeating;

	public bool windowOpen, doorOpen;

	public GameObject thermoCanvas;
	public Text thermometerText;

	public DataFetcher dataFetcher;


	// Use this for initialization
	void Start () {
		mTrackableBehaviour = GetComponent<TrackableBehaviour> ();
		mTrackableBehaviour.RegisterTrackableEventHandler (this);

		completed = false;
		inputOn = true;

		// init
		trackableFairyAnim = trackableFairy.GetComponent<Animator>();
		bearAnim = bear.GetComponent<Animator> ();

		radiatorOverHeating = 0;

		radiatorMat.color = Color.white;

		windowOpen = doorOpen = false;

		bearOriginalPos = bear.transform.position;

		doorOpen = windowOpen = true;
	}

	// Update is called once per frame
	void Update () {
		
		//TODO win conditions
		if (!completed && questState > 0) {
			completed = true;

			engine.IncrementScore(3);
			//bear.transform.LookAt (trackableFairy.transform.position);
			bear.transform.LookAt( new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
			star.SetActive (true);
			trackableFairyAnim.SetTrigger ("celebrate");
			bearAnim.SetTrigger ("celebrate");
			StartCoroutine (ShowFairySpeech ("Hienoa! Nyt lämpö pysyy tallessa, eikä patterin tarvitse huhkia liikaa. Säästämme energiaa!", 5f));
		}

		if (!doorOpen && !windowOpen) {
			if (radiatorOverHeating != 0) {
				radiatorOverHeating = 0;
				if (radiatorMat.color != Color.white) {
					radiatorMat.color = Color.white;
					//dataFetcher.UpdateThermoText (thermometerText,0);
				}
			}
		}

		if (Input.touchCount > 0 || Input.GetMouseButtonDown (0)) {
			Vector3 inputVector = Vector3.zero;
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
				inputVector = Input.touches [0].position;
			if ( Input.GetMouseButtonDown(0))
				inputVector = Input.mousePosition;

			Ray ray = Camera.main.ScreenPointToRay (inputVector);
			RaycastHit hit;

			if ( Physics.Raycast( ray.origin, ray.direction, out hit, Mathf.Infinity) && inputOn) {
				if (hit.collider.gameObject.name == "Window" && windowOpen) {
					StartCoroutine (CloseWindow (hit.point));
				}
				if (hit.collider.gameObject.name == "Door" && doorOpen) {
					StartCoroutine (CloseDoor (hit.point));
				}
			}
		}

		if (radiatorOverHeating != 0) {
			Color tempColor = radiatorMat.color;

			if (radiatorOverHeating > 0) {
				tempColor.g -= Time.deltaTime / 2;
				tempColor.b -= Time.deltaTime / 2;
				if (tempColor.g < 0.01f) {
					radiatorOverHeating = -1;
				}
			} else if (radiatorOverHeating < 0) {
				tempColor.g += Time.deltaTime / 2;
				tempColor.b += Time.deltaTime / 2;
				if (tempColor.g > 0.99f) {
					radiatorOverHeating = 1;
				}
			}
			radiatorMat.color = tempColor;
		}
	}

		public IEnumerator WalkBear(Vector3 pos){
		inputOn = false;
		bearAnim.SetBool ("walking", true);
		pos.z -= 0.2f;
		pos.y = bear.transform.position.y;
		while (Vector3.Distance (bear.transform.position, pos) > 0.01f) {
			bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f;
			bear.transform.LookAt (pos);
			yield return null;
		}

		bear.transform.rotation = Quaternion.identity;
		bearAnim.SetBool ("walking", false);
		bearAnim.SetTrigger ("pressButton");

		yield return new WaitForSeconds (1.4f);
		inputOn = true;
	}

	IEnumerator CloseWindow(Vector3 pos){
		yield return StartCoroutine (WalkBear (pos));
		windowOpen = false;
		window.transform.localRotation = Quaternion.identity;
		windowTimesOpened++;

		if (doorOpen == false && doorTimesOpened < 2) {
			StartCoroutine (OpenDoor ());
		} else if (!doorOpen && !windowOpen) {
			StartCoroutine(IncQuestStateAfterDelay (1f));
		}
	}

	IEnumerator CloseDoor( Vector3 pos ){
		yield return StartCoroutine (WalkBear (pos));
		doorOpen = false;
		door.transform.localRotation = Quaternion.identity;
		doorTimesOpened++;

		if (windowOpen == false && windowTimesOpened < 2) {
			StartCoroutine (OpenWindow ());
		} else if (!doorOpen && !windowOpen) {
			StartCoroutine(IncQuestStateAfterDelay (1f));
		}
	}

	IEnumerator OpenWindow(){
		float timer = 2;
		while (timer > 0) {
			window.transform.Rotate (Vector3.up * Time.deltaTime * 75);
			timer -= Time.deltaTime;
			yield return null;
		}
		windowOpen = true;

		//dataFetcher.UpdateThermoText (thermometerText,-5);
		radiatorOverHeating = 1;
	}

	IEnumerator OpenDoor(){
		float timer = 2;
		while (timer > 0) {
			door.transform.Rotate (-Vector3.up * Time.deltaTime * 75);
			timer -= Time.deltaTime;
			yield return null;
		}
		doorOpen = true;

		//dataFetcher.UpdateThermoText (thermometerText,-5);
		radiatorOverHeating = 1;
	}

	public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus){
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED) {

			transform.GetChild(0).gameObject.SetActive (true);
			localFairy.SetActive (false);
			bear.SetActive (false);

			bear.transform.position = bearOriginalPos;
			bear.transform.LookAt (trackableFairy.transform.position);

			engine.LoadGame ();
			inputOn = true;

			thermoCanvas.SetActive (true);

			//dataFetcher.UpdateThermoText (thermometerText,0);


			doorOpen = windowOpen = true;

			window.transform.localRotation = Quaternion.Euler (new Vector3 (0, 130, 0));
			door.transform.localRotation = Quaternion.Euler (new Vector3 (0, -130, 0));

			if (System.DateTime.Compare (System.DateTime.Now.AddSeconds (-engine.CoolDownInSeconds), engine.GetScore(0).updateTimestamp) > 0 && engine.GetScore(0).score < 5) {


				completed = false;
				doorTimesOpened = 0;
				windowTimesOpened = 0;
				questState = 0;

				StartCoroutine (StartTimeLine());

			} else {


				StartCoroutine (ShowFairySpeech ("INSERT TASK DELAY TEXT", 5f));
			}

		} else {
			//hide fairy, redundant? gives null error
			//transform.GetChild(0).gameObject.SetActive (false);
			trackableFairySpeechBubble.SetActive (false);
			bearSpeechBubble.SetActive (false);
			localFairy.SetActive (true);
			thermoCanvas.SetActive (false);

			StopAllCoroutines ();
		}
	}

	IEnumerator ShowFairySpeech(string inputText, float showDuration){
		trackableFairySpeechBubble.SetActive (true);
		trackableFairySpeechBubble.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = inputText;
		yield return new WaitForSeconds (showDuration);
		trackableFairySpeechBubble.SetActive (false);
	}

	IEnumerator StartTimeLine(){
		inputOn = false;
		radiatorOverHeating = 1;

        try
        {
            //string roomTemp = dataFetcher.roomDataList[0].Temperature.ToString();
        }
        catch
        {
            Debug.Log("index error");
        }

        yield return StartCoroutine (ShowFairySpeechWaitForInput ("Missäköhän Otso on taas kuljeksimassa? Hän on jättänyt akkunan auki. Ja ovenkin!"));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Hyvät hyssykät, patteriparka yrittää lämmittää huonetta aivan punakkana, mutta tuvassa tulee silti vilu!"));

		yield return StartCoroutine (FairyClose ());

		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Huh! Meillä täällä Suomessa on suuren osan vuodesta niin vilpoisaa, että tarvitsemme taloihin lämmitystä."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Mutta jos jätämme oven tai ikkunan pitkäksi aikaa auki, karkaa lämpö pois."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Patteri yrittää korjata tilannetta paiskimalla hommia oikein olan takaa!"));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Lämmittämiseen patteri käyttää luonnon energiavaroja, ja mitä kuumempi patteri, sitä enemmän niitä kuluu."));
		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Siksi kylminä vuodenaikoina pitääkin muistaa sulkea ovet ja ikkunat aina kun mahdollista, ettei lämpö mene hukkaan."));

		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Kuulkaas, tarkistakaahan, ettei teidän ympärillänne ole turhaan jätetty ovia tai ikkunoita auki!"));

		// OHJE: ovatko ovet ja ikkunat kiinni?

		StartCoroutine( OpenWindow ());
		StartCoroutine( OpenDoor ());
		bear.SetActive (true);

		yield return StartCoroutine (ShowFairySpeechWaitForInput ("Kauhistus! Kohta patteri alkaa posottaa liikaa lämpöä! Auttakaa Otsoa sulkemaan ovi ja ikkuna!"));

		// OHJE: auttakaa otsoa sulkemaan ovi ja ikkuna koskettamalla niitä

		inputOn = true;
	}

	IEnumerator FairyClose(){
		Vector3 originalPos = trackableFairy.transform.position;
		Quaternion originalRot = trackableFairy.transform.rotation;
		trackableFairyAnim.SetBool ("walking", true);
		yield return StartCoroutine (WalkFairy (door.transform.position.x));
		trackableFairy.transform.LookAt (new Vector3(door.transform.position.x, trackableFairy.transform.position.y, door.transform.position.y));
        trackableFairyAnim.SetBool("walking", false);
        yield return new WaitForSeconds (0.25f);
        trackableFairyAnim.SetTrigger("touch");
        yield return new WaitForSeconds(0.5f);
		door.transform.localRotation = Quaternion.identity;
		yield return new WaitForSeconds (1f);
        trackableFairyAnim.SetBool("walking", true);

        radiatorOverHeating = 0;
		radiatorMat.color = Color.white;

		yield return StartCoroutine (WalkFairy (window.transform.position.x));
		trackableFairy.transform.LookAt (new Vector3(window.transform.position.x, trackableFairy.transform.position.y, window.transform.position.y));
        trackableFairyAnim.SetBool("walking", false);
        yield return new WaitForSeconds (0.25f);
        trackableFairyAnim.SetTrigger("touch");
        yield return new WaitForSeconds(0.5f);
        window.transform.localRotation = Quaternion.identity;
		yield return new WaitForSeconds (1f);
        trackableFairyAnim.SetBool("walking", true);

        yield return StartCoroutine(WalkFairy(originalPos.x));
		trackableFairyAnim.SetBool ("walking", false);
		trackableFairy.transform.rotation = originalRot;
		yield return new WaitForSeconds (0.5f);
	}

	IEnumerator WalkFairy(float x){
		Vector3 desPos = trackableFairy.transform.position;
		desPos.x = x;

		trackableFairy.transform.LookAt (desPos);

		Vector3 direction = (desPos - trackableFairy.transform.position).normalized;

		while (Vector3.Distance (trackableFairy.transform.position, desPos) > 0.1f) {
			trackableFairy.transform.position += direction * Time.deltaTime * 0.3f;
			yield return null;
		}
	}

	IEnumerator IncQuestStateAfterDelay(float time) {

		yield return new WaitForSeconds (time);

		questState++;
	}

	IEnumerator ShowFairySpeechWaitForInput(string inputText){
		trackableFairySpeechBubble.SetActive (true);
		trackableFairySpeechBubble.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = inputText;

		yield return StartCoroutine (WaitForInput ());

		trackableFairySpeechBubble.SetActive (false);
	}

	IEnumerator WaitForInput(){
		yield return new WaitForSeconds (1f);
		while (Input.touchCount == 0 && !Input.GetMouseButtonDown (0) ) {
			yield return null;
		}
	}
}
