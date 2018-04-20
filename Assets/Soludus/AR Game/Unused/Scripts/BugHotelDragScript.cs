using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugHotelDragScript : MonoBehaviour {

	private bool dragging = false;
	public bool allowDragging = true;
	private float distance;

	public GameObject bugHotel;

	private BugHotelManager bugHotelMng;

	Vector3 originalPos;

	public Camera localCamera;

	// Use this for initialization
	void Start () {
		
		// set initial references
		originalPos = transform.position;
		bugHotelMng = bugHotel.GetComponent<BugHotelManager> ();
	}

	void OnEnable() {
		dragging = false;
		allowDragging = true;
	}

	void OnMouseDown()
	{
		
		// this is for dragging food to the plate
		if (allowDragging)
		{
			distance = Vector3.Distance(transform.position, localCamera.transform.position);
			dragging = true;
		}
	}

	void OnMouseUp()
	{

		dragging = false;

		if (allowDragging == true)
		{
			transform.position = originalPos;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (dragging)
		{
			// dragging position = mouse position on local camera. The dragable object can move only on X and Y-axis.
			Ray ray = localCamera.ScreenPointToRay(Input.mousePosition);
			Vector3 rayPoint = ray.GetPoint(distance);
			rayPoint.z = transform.position.z;

			transform.position = rayPoint;

		}
	}

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log (other.gameObject.name);


		if (other.gameObject.name == "Placeholder_dragSpot" && gameObject.name == "bark_light")
		{
//			transform.position = other.gameObject.transform.position;
//			allowDragging = false;
//			dragging = false;
			bugHotel.transform.GetChild (0).GetChild (1).gameObject.SetActive (true);
			bugHotelMng.FinishTask ();
			gameObject.SetActive (false);
		}

		else if (other.gameObject.name == "Placeholder_dragSpot2" && gameObject.name == "woodTubes")
		{
//			transform.position = other.gameObject.transform.position;
//			allowDragging = false;
//			dragging = false;
			bugHotel.transform.GetChild (0).GetChild (2).gameObject.SetActive (true);
			bugHotelMng.FinishTask ();
			gameObject.SetActive (false);
		}

		else if (other.gameObject.name == "Placeholder_dragSpot3" && gameObject.name == "woodBig")
		{
//			transform.position = other.gameObject.transform.position;
//			allowDragging = false;
//			dragging = false;
			bugHotel.transform.GetChild (0).GetChild (3).gameObject.SetActive (true);
			bugHotelMng.FinishTask ();
			gameObject.SetActive (false);
		}
	}
}
