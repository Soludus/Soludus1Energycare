using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingTextScript : MonoBehaviour {

    Text loadingText;
    public float loadingPointFrequency;
    float cooldown;

	// Use this for initialization
	void Start () {

        loadingText = GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {

        cooldown += Time.deltaTime;

        if (cooldown > loadingPointFrequency)
        {
            cooldown = 0;
            if (loadingText.text == "Ladataan.")
                loadingText.text = "Ladataan..";
            else if (loadingText.text == "Ladataan..")
                loadingText.text = "Ladataan...";
            else if (loadingText.text == "Ladataan...")
                loadingText.text = "Ladataan.";
            else loadingText.text = "Ladataan.";
        }

	}
}
