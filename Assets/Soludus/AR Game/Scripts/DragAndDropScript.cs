using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DragAndDropScript : MonoBehaviour
{

    private bool dragging = false;
    public bool allowDragging = true;
    public bool noFoodObject = false;

    public float z;

    Vector3 originalPos;

    public Camera localCamera = null;

    private PlateTaskManager plateTaskManager;

    private void OnEnable()
    {
        transform.position = originalPos;
        allowDragging = true;
        dragging = false;
    }

    private void Awake()
    {
        // set initial references
        z = transform.position.z;
        originalPos = transform.position;
        plateTaskManager = GameObject.Find("ServingPlate").GetComponent<PlateTaskManager>();
    }

    void OnMouseDown()
    {

        // this is for handling throw to trash and save food options
        if (noFoodObject)
        {
            allowDragging = false;
            plateTaskManager.FinishLeftoverTask(gameObject.name);

        }
        // this is for dragging food to the plate
        else if (allowDragging)
        {
            dragging = true;
        }
    }

    void OnMouseUp()
    {

        dragging = false;

        if (allowDragging == true)
        {
            transform.position = originalPos;
        }
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -localCamera.transform.position.z + z;
            transform.position = localCamera.ScreenToWorldPoint(mousePos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
		Debug.Log (other.gameObject.name);

        // meat
		if (other.gameObject.name == "Placeholder_spotForFood1" && gameObject.name == "salmon")
        {
            transform.position = other.gameObject.transform.position;
            allowDragging = false;
            dragging = false;
            plateTaskManager.FinishTask();
        }
        // meat smaller portion
        else if (other.gameObject.name == "Placeholder_spotForFood1" && gameObject.name == "salmonSmallPortion")
        {
            transform.position = other.gameObject.transform.position;
            allowDragging = false;
            dragging = false;
            plateTaskManager.FinishTask();
        }
        // potatoes
        else if (other.gameObject.name == "Placeholder_spotForFood2" && gameObject.name == "potato")
        {
            transform.position = other.gameObject.transform.position;
            allowDragging = false;
            dragging = false;
            plateTaskManager.FinishTask();
        }
        // potatoes smaller portion
        else if (other.gameObject.name == "Placeholder_spotForFood2" && gameObject.name == "potatoSmallPortion")
        {
            transform.position = other.gameObject.transform.position;
            allowDragging = false;
            dragging = false;
            plateTaskManager.FinishTask();
        }
        // vegetables
        else if (other.gameObject.name == "Placeholder_spotForFood3" && gameObject.name == "salad")
        {
            transform.position = other.gameObject.transform.position;
            allowDragging = false;
            dragging = false;
            plateTaskManager.FinishTask();
        }
        // vegetables smaller portion
        else if (other.gameObject.name == "Placeholder_spotForFood3" && gameObject.name == "saladSmallPortion")
        {
            transform.position = other.gameObject.transform.position;
            allowDragging = false;
            dragging = false;
            plateTaskManager.FinishTask();
        }
    }
}