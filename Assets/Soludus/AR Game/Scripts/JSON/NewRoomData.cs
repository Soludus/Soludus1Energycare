using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class NewRoomData {
	
	public int Temperature;
	public int Humidity;
	public string Light;
	public string timestamp;

	public static RoomData CreateFromJSON(string jsonString){
		return JsonUtility.FromJson<RoomData> (jsonString);
	}
		

}