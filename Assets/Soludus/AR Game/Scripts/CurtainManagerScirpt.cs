using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainManagerScirpt : MonoBehaviour {

    public GameObject curtainLeft;
    public GameObject curtainRight;

    Vector3 curtainLOriginalPos;
    Vector3 curtainROriginalPos;

	// Use this for initialization
	void Start () {

        curtainLOriginalPos = curtainLeft.transform.position;
        curtainROriginalPos = curtainRight.transform.position;
		
	}

    private void OnEnable()
    {
        curtainLeft.transform.position = curtainLOriginalPos;
        curtainRight.transform.position = curtainROriginalPos;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
