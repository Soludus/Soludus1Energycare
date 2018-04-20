using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugHotelManager : MonoBehaviour {

	int finishedTasks;

	private HotelliTargetScript hotelTarget;

	public GameObject tubes;
	public GameObject bark;
	public GameObject wood;

	private Vector3 tubesOriginalPos;
	private Vector3 barkOriginalPos;
	private Vector3 woodOriginalPos;

	private void OnEnable()
	{
		finishedTasks = 0;
		transform.GetChild (0).GetChild (1).gameObject.SetActive (false);
		transform.GetChild (0).GetChild (2).gameObject.SetActive (false);
		transform.GetChild (0).GetChild (3).gameObject.SetActive (false);
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	// Use this for initialization
	void Start () {
		hotelTarget = GameObject.Find ("hotelliTarget").GetComponent<HotelliTargetScript> ();
	}

	private void Awake() {

		tubesOriginalPos = tubes.transform.position;
		barkOriginalPos = bark.transform.position;
		woodOriginalPos = wood.transform.position;

		gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {

		if (finishedTasks == 3)
		{
			finishedTasks = 0;
			StartCoroutine(FinishAfterDelay());
		}

	}

	public void FinishTask()
	{
		finishedTasks++;
	}

	IEnumerator FinishAfterDelay()
	{
		yield return new WaitForSeconds(1f);
		hotelTarget.StartEndTimeLine ();
	}

	public void RestartBugHotel() {

		tubes.SetActive (true);
		bark.SetActive (true);
		wood.SetActive (true);

		tubes.transform.position = tubesOriginalPos;
		bark.transform.position = barkOriginalPos;
		wood.transform.position = woodOriginalPos;

		tubes.GetComponent<BugHotelDragScript>().allowDragging = true;
		bark.GetComponent<BugHotelDragScript>().allowDragging = true;
		wood.GetComponent<BugHotelDragScript>().allowDragging = true;

	}
}
