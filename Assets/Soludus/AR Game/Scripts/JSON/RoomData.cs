using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class RoomData {

	public string Room;
	public int Temperature;
	public int Humidity;
	public string timestamp;

	public static RoomData CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<RoomData> (jsonString);
	}

	public RoomData(){

	}

}