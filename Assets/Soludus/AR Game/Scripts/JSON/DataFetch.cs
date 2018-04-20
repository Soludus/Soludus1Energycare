using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;


public class DataFetch : MonoBehaviour {

	public string jsonData;
	public RoomData[] roomDataList;

	private System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding ();
	private string url = "https://139bd523-f0ff-40e0-a1c2-2712ab008bdc-bluemix.cloudant.com/sensordata/_find/";
	private Dictionary<string,string> postHeader = new Dictionary<string,string> ();

	// Use this for initialization
	void Start () {
		jsonData = null;
		postHeader.Add ("content-type", "application/json");

		StartCoroutine ( UpdateAllRooms());



	}
		

	IEnumerator FetchText(){
		string jsonRequest = "{\"selector\":{\"Room\":\"Ruokala\"},\"fields\":[\"Room\",\"Temperature\",\"Humidity\",\"timestamp\"],\"limit\":20}";

		WWW www = new WWW (url, encoding.GetBytes(jsonRequest),postHeader);

		yield return www;
		if (www.error != null) {
			Debug.Log (www.error);
		} else {
			jsonData = www.text;
			Debug.Log (jsonData);
			roomDataList = JsonHelper.FromJson<RoomData> (jsonData);
		}
	}

	IEnumerator FetchRoomText(string roomName){
		string jsonRequest = "{\"selector\":{\"Room\":\""+ roomName +"\"},\"fields\":[\"Room\",\"Temperature\",\"Humidity\",\"timestamp\"],\"limit\":100}";

		WWW www = new WWW (url, encoding.GetBytes(jsonRequest),postHeader);

		yield return www;
		if (www.error != null) {
			Debug.Log (www.error);
		} else {
			jsonData = www.text;
			Debug.Log (jsonData);
			roomDataList = JsonHelper.FromJson<RoomData> (jsonData);
		}
	}

	IEnumerator UpdateAllRooms(){
		yield return StartCoroutine( UpdatePanel (0, "Ruokala"));
		yield return StartCoroutine( UpdatePanel (1, "Projektitila"));
		yield return StartCoroutine( UpdatePanel (2, "Eteinen"));
		yield return StartCoroutine( UpdatePanel (3, "katto"));

	}

	IEnumerator UpdatePanel(int i, string roomName ){
		yield return StartCoroutine (FetchRoomText (roomName)); 


		string panelText = "Huone: " + roomName + "\nLämpötila: " + roomDataList [0].Temperature + "°C\nIlmankosteus: " + roomDataList [0].Humidity + "%\nMittauksen Aika: " + ParseDateTime(roomDataList [0].timestamp).ToString("HH:mm") + "\nMittauksen Päivä: " + ParseDateTime(roomDataList [0].timestamp).ToString("dd/MM/yyyy");

		transform.GetChild (i).GetChild (0).GetComponent<Text> ().text = panelText;

	}

	public DateTime ParseDateTime(string timeString){
		return (DateTime.Parse (timeString));
	}
		
		
}
