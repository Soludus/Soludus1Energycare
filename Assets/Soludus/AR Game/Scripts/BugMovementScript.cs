using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovementScript : MonoBehaviour {

	Rigidbody rigid;
	public float movementSpeed;
	private bool allowMovement = true;
	private bool dragging = false;
	private float distance = 0;
	private Vector3 direction;

	private Camera localCamera;

    private AudioSource bugDestroyAudio;

	void Awake() {

		localCamera = GameObject.Find ("LocalCamera").GetComponent<Camera>();
		StartCoroutine(DestroyBug(1));
	}

	// Use this for initialization
	void Start () {

        bugDestroyAudio = GetComponent<AudioSource>();
		rigid = GetComponent<Rigidbody> ();
		movementSpeed = Random.Range (0.020f, 0.030f);
	}

	// Update is called once per frame
	void FixedUpdate () {

		if (allowMovement)
			rigid.MovePosition (transform.position + transform.forward * Time.deltaTime * movementSpeed);

		if (dragging) {

			Ray ray = localCamera.ScreenPointToRay (Input.mousePosition);

			Vector3 oldPos = transform.position;
			Vector3 newPos = ray.GetPoint (distance);
			newPos.z = transform.position.z;

			rigid.MovePosition (newPos);

			Vector3 velocity = (newPos - oldPos);
			direction = velocity.normalized;
		}

	}

	void OnMouseDown() {

		dragging = true;
		allowMovement = false;
		StopAllCoroutines ();

	}

	void OnMouseUp() {

		// sends bug flying to the direction of the last drag update, activates rigidbody's gravity and starts the destruction of the gameobject
		dragging = false;
		rigid.velocity = direction;
		rigid.useGravity = true;
		StartCoroutine (DestroyBug (0));

	}

	// destroys bug 30s after spawn or 5s after released from player's drag
	IEnumerator DestroyBug(int action) {

        if (action == 1)
        {
            yield return new WaitForSeconds(25f);
        }
        else bugDestroyAudio.Play();
		yield return new WaitForSeconds (5f);
		Destroy (gameObject);

	}
}
