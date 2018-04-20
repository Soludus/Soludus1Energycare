using UnityEngine;
using System.Collections;

public class ScoreBoardLoader : MonoBehaviour
{
    public GameEngine ge;

    public void Initialize()
    {
        ge.LoadGame();

        EmptyStars();
        StartCoroutine(SlowFillStars());
    }

    public void EmptyStars()
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).GetChild(j).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator SlowFillStars()
    {
        for (int i = 0; i < 12; i++)
        {
            for (int j = 0; j < ge.GetScore(i).score; j++)
            {
                transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void CloseScoreBoard()
    {
        gameObject.SetActive(false);
    }
}
