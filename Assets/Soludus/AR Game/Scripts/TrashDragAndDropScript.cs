using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDragAndDropScript : MonoBehaviour
{

    private bool dragging = false;
    public bool allowDragging = true;

    public float z;

    Vector3 originalPos;

    public Camera localCamera;
    public DialoguePollution1 dl1;

    // Use this for initialization
    void Awake()
    {
        z = transform.position.z;
        originalPos = transform.position;
    }

    private void OnEnable()
    {
        allowDragging = true;
        dragging = false;
        transform.position = originalPos;
    }

    void OnMouseDown()
    {
        // this is for dragging food to the plate
        if (allowDragging)
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

    // Update is called once per frame
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
        if (other.gameObject.tag == "bin")
        {
            dl1.trashDragged = true;
        }
    }
}
