using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateTaskManager : MonoBehaviour {

    private int finishedTasks;
    private RuokaTargetScript ruokaTarget;

    public GameObject plate;

    public GameObject food1;
    public GameObject food2;
    public GameObject food3;

    private Vector3 food1OriginalPos;
    private Vector3 food2OriginalPos;
    private Vector3 food3OriginalPos;

    public GameObject food1SmallPortion;
    public GameObject food2SmallPortion;
    public GameObject food3SmallPortion;

    private Vector3 food1SmallPortionOriginalPos;
    private Vector3 food2SmallPortionOriginalPos;
    private Vector3 food3SmallPortionOriginalPos;

    public GameObject food1Leftovers;
    public GameObject food2Leftovers;
    public GameObject food3Leftovers;

    public GameObject plateTrash;
    public GameObject plateSavings;

    public GameObject pot;

    public int state = 0;
    public int leftoverDecisionState = 0;

    private void OnEnable()
    {
        finishedTasks = 0;

        if (state == 2)
        {
			food1.SetActive(false);
			food2.SetActive(false);
			food3.SetActive(false);

            food1Leftovers.SetActive(true);
            food2Leftovers.SetActive(true);
            food3Leftovers.SetActive(true);

            plateTrash.SetActive(true);
            plateSavings.SetActive(true);
        }
        else if (state == 3)
        {
            plate.SetActive(false);

            food1Leftovers.SetActive(false);
            food2Leftovers.SetActive(false);
            food3Leftovers.SetActive(false);

            plateTrash.SetActive(true);
            plateSavings.SetActive(true);

            pot.SetActive(true);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // Use this for initialization
    void Start () {

		ruokaTarget = GameObject.Find("ruokaTarget").GetComponent<RuokaTargetScript>();

    }

    private void Awake()
    {
        food1OriginalPos = food1.transform.position;
        food2OriginalPos = food2.transform.position;
        food3OriginalPos = food3.transform.position;

        food1SmallPortionOriginalPos = food1SmallPortion.transform.position;
        food2SmallPortionOriginalPos = food2SmallPortion.transform.position;
        food3SmallPortionOriginalPos = food3SmallPortion.transform.position;

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {

        if (finishedTasks == 3)
        {
            finishedTasks = 0;

			state = 2;
            StartCoroutine(FinishAfterDelay());
        }
	}

    public void FinishTask()
    {
        finishedTasks++;
    }

    public void FinishLeftoverTask(string objectName)
    {

        // leftovers - throw to trash
        if (objectName == "PlateTrash" && state == 2)
        {
            ruokaTarget.leftoverDecision = 1;
        }
        // leftovers - save
        else if (objectName == "PlateSavings" && state == 2)
        {
            ruokaTarget.leftoverDecision = 2;
        }
        // potatoes - throw to trash
        else if (objectName == "PlateTrash" && state == 3)
        {
            ruokaTarget.leftoverDecision = 3;
        }
        // potatoes - save
        else if (objectName == "PlateSavings" && state == 3)
        {
            ruokaTarget.leftoverDecision = 4;
        }

        state++;
        ruokaTarget.StartPlateTaskFinished();
    }

    // used to reset plate's food positions when going back to this level
    public void RestartPlate()
    {
        state = 0;
        leftoverDecisionState = 0;

        food1.transform.position = food1OriginalPos;
        food2.transform.position = food2OriginalPos;
        food3.transform.position = food3OriginalPos;

        food1SmallPortion.transform.position = food1SmallPortionOriginalPos;
        food2SmallPortion.transform.position = food2SmallPortionOriginalPos;
        food3SmallPortion.transform.position = food3SmallPortionOriginalPos;

        plate.SetActive(true);

        food1.SetActive(true);
        food2.SetActive(true);
        food3.SetActive(true);

        food1SmallPortion.SetActive(false);
        food2SmallPortion.SetActive(false);
        food3SmallPortion.SetActive(false);

        food1Leftovers.SetActive(false);
        food2Leftovers.SetActive(false);
        food3Leftovers.SetActive(false);

        plateTrash.SetActive(false);
        plateSavings.SetActive(false);

        pot.SetActive(false);

        food1.GetComponent<DragAndDropScript>().allowDragging = true;
        food2.GetComponent<DragAndDropScript>().allowDragging = true;
        food3.GetComponent<DragAndDropScript>().allowDragging = true;

        food1SmallPortion.GetComponent<DragAndDropScript>().allowDragging = true;
        food2SmallPortion.GetComponent<DragAndDropScript>().allowDragging = true;
        food3SmallPortion.GetComponent<DragAndDropScript>().allowDragging = true;

        gameObject.SetActive(false);
    }

    IEnumerator FinishAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        ruokaTarget.StartPlateTaskFinished();
    }
}
