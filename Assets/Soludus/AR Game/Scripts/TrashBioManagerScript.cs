using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBioManagerScript : MonoBehaviour {

    public GameObject[] screenTrash;
    public GameObject[] screenBins;

    private void OnEnable()
    {
        for (int i = 0; i < screenTrash.Length; i++)
        {
            screenTrash[i].SetActive(false);
            screenBins[i].SetActive(false);
        }
    }

    public void ShowTrashAndBin(int index)
    {
        screenTrash[index].SetActive(true);
        screenBins[index].SetActive(true);
    }

    public void HideTrashAndBin(int index)
    {
        screenTrash[index].SetActive(false);
        screenBins[index].SetActive(false);
    }
}
