using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragBunnyScript : MonoBehaviour {

	private bool dragging = false;
	public bool allowDragging = true;

	public Vector3 originalPos;

	public Camera localCamera;

	private MissaOlenHyvaScript missaOlenHyvaTarget;

    public float z;

    private void Awake()
	{
        z = transform.position.z;
        originalPos = transform.localPosition;
		missaOlenHyvaTarget = GameObject.Find ("missaOlenHyvaTarget").GetComponent<MissaOlenHyvaScript> ();
	}

	void OnMouseDown()
	{
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
			transform.localPosition = originalPos;
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

		if (other.gameObject.name == "Paper") {

			transform.position = new Vector3 (other.gameObject.transform.position.x, other.gameObject.transform.position.y, transform.position.z);
			allowDragging = false;
			dragging = false;
			missaOlenHyvaTarget.drawingIsActive = false;
		}
	}
}
