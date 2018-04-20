using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPSLocationService : MonoBehaviour {

	//public bool GPS;
	//public string lastDataString;

	//public Place place1, place2;



	//// Use this for initialization
	//IEnumerator Start () {
	//	// initialize the gps 
	//	Input.location.Start (5,5);
	//	lastDataString = "0 0";
	//	GPS = false;

	//	int maxWait = 5;
	//	while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
	//		yield return new WaitForSeconds (1);
	//		maxWait--;
	//	}

	//	if (maxWait < 0) {
	//		print ("GPS Timed Out");
	//		yield break;
	//	}
	//	place1 = new Place ();
	//	place2 = new Place ();
	//	//
	//	place1.name = "koillinen";
	//	place1.locationInfo.x = Input.location.lastData.latitude + 0.01f;
	//	place1.locationInfo.y = Input.location.lastData.latitude + 0.01f;

	//	place2.name = "lounas";
	//	place2.locationInfo.x = Input.location.lastData.latitude - 0.01f;
	//	place2.locationInfo.y = Input.location.lastData.latitude - 0.01f;
	//}

	//// Update is called once per frame
	//void Update () {
	//	lastDataString = Input.location.lastData.latitude +" "+ Input.location.lastData.longitude;
	//}

	//void OnGUI(){
	//	string str = "Latitude: " + Input.location.lastData.latitude + " Longitude: " + Input.location.lastData.longitude;
	//	GUI.Label (new Rect (100, 10, 300, 50), str );
	//	GUI.Label (new Rect (100, 30, 300, 50), "Timestamp: " + Input.location.lastData.timestamp.ToString() );
	//	GUI.Label (new Rect (100, 50, 300, 50), "Accuracy: " + Input.location.lastData.horizontalAccuracy );
	//	GUI.Label (new Rect (100, 70, 300, 50), "Closest place: " + GetClosestPlace().name );


	//}

	//public Place GetClosestPlace(){
	//	float distToPlace1 = Mathf.Abs (Input.location.lastData.latitude - place1.locationInfo.x) + Mathf.Abs (Input.location.lastData.longitude - place1.locationInfo.y);
	//	float distToPlace2 = Mathf.Abs (Input.location.lastData.latitude - place1.locationInfo.x) + Mathf.Abs (Input.location.lastData.longitude - place1.locationInfo.y);

	//	if (distToPlace1 < distToPlace2)
	//		return place1;
	//	if (distToPlace1 >= distToPlace2)
	//		return place2;
		
	//	return new Place();
	//}
    //public struct Place{
	   // public string name;
	   // public Vector2 locationInfo;
    //}

}