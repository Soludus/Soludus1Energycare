using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

    public void SelectState(int state)
    {

        if (state == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 3; i < 9; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (state == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 3; i < 6; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = 6; i < 9; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (state == 1)
        {
            for (int i = 0; i < 6; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }

            for (int i = 6; i < 9; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
