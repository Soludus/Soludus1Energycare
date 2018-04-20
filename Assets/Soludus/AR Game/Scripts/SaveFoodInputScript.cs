using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFoodInputScript : MonoBehaviour {

    public GameObject OkButton;
    public GameObject ruokaTarget;
    public DialogueEnergy3 de3;

	void OnDisable() {
        OkButton.gameObject.SetActive(false);
        gameObject.GetComponent<InputField> ().text = "";
	}

	public void SaveWastedFood() {

		Debug.Log (gameObject.GetComponent<InputField>().text);
        //ruokaTarget.GetComponent<RuokaTargetScript> ().textInput = true;
        de3.textInput = true;
	}

    public void ShowOKButton()
    {
        if (gameObject.GetComponent<InputField>().text != "")
        {
            OkButton.gameObject.SetActive(true);
        }
        else if (gameObject.GetComponent<InputField>().text == "")
        {
            OkButton.gameObject.SetActive(false);
        }
    }
}
