using UnityEngine;
using System.Collections;

public class LocalObjectCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (new Vector3 (-Input.gyro.attitude.eulerAngles.y, 0, 0));
	}
}
