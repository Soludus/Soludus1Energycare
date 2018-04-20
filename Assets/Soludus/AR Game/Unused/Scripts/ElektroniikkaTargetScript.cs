using UnityEngine;
using System.Collections;
using Vuforia;
using UnityEngine.UI;

public class ElektroniikkaTargetScript : MonoBehaviour, ITrackableEventHandler {
	
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

	public Camera localCamera;
	public GameObject star;

	public Animator trackableFairyAnim;

	Vector3 bearOriginalPos;

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
	}

	// Update is called once per frame
	void Update()
	{

		// level completed
		if (questState == 4 && completed == false)
		{
			completed = true;

			//engine.IncrementScore3 ();

			star.SetActive(true);

			trackableFairyAnim.SetTrigger("celebrate");
			bearAnim.SetTrigger("celebrate");
			bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
			StartCoroutine(ShowFairySpeechWaitForInput("Jee!"));
		}
	}

	// the level starts with this dialogue
	IEnumerator StartTimeLine()
	{

		trackableFairyAnim.SetTrigger("celebrate");
		bear.SetActive(true);

		yield return StartCoroutine(ShowFairySpeechWaitForInput("Elektroniikkaa!"));

	}

	public void StartEndTimeLine() {

		StartCoroutine (EndTimeLine());

	}

	IEnumerator EndTimeLine() {

		yield return StartCoroutine(ShowFairySpeechWaitForInput("Hyvin tehty!"));

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

			if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(2).updateTimestamp) > 0 && engine.GetScore(2).score < 5)
			{


				completed = false;

				StartCoroutine(StartTimeLine());

				questState = 0;
			}
			else
			{
				StartCoroutine(ShowFairySpeech("Patsas on vielä pystyssä. Tarkista huomenna uudestaan!", 5f));

			}

		}
		else
		{
			transform.GetChild(0).gameObject.SetActive(false);
			localFairy.SetActive(true);
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
