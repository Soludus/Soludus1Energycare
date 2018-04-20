using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragForestObjectScript : MonoBehaviour {

	private bool dragging = false;
	public bool allowDragging = true;

	public GameObject statueReadyButton;
    public GameObject[] statueSetButtons;

	public GameObject statueConstruct;

	public GameObject constructArea;
	private bool stoneInConstructArea = false;
	private bool decorationInConstructArea = false;

	public Collider meshCollider;

	public MetsanTargetScript metsanTarget;

	public bool onStatueArea = false;

	public bool startRotate = true;
	public bool allowRotation = false;
	public bool rotating = false;
	public float rotatingSpeed;

    public static bool objectPlaced;

	Vector3 originalPos;
	Quaternion originalRot;

	public Camera localCamera;

    public int indexNumber;

    public float z;

    public AudioClipManagerScript acm;
    public AudioSource statueAS;

	// Use this for initialization
	void Awake () {

        z = transform.position.z;

		// set initial references
		originalPos = transform.position;
		originalRot = transform.rotation;

        if (gameObject.name == "stick")
            indexNumber = 0;
        else if (gameObject.name == "stick2")
            indexNumber = 1;
        else if (gameObject.name == "stick3")
            indexNumber = 2;
        else if (gameObject.name == "pinecone")
            indexNumber = 3;
        else if (gameObject.name == "pinecone2")
            indexNumber = 4;
        else if (gameObject.name == "rock1")
            indexNumber = 5;
        else if (gameObject.name == "rock2")
            indexNumber = 6;
        else if (gameObject.name == "rock3")
            indexNumber = 7;

    }

    public void PlaceStatuePart()
    {
        rotating = false;

        if (onStatueArea)
        {

            gameObject.transform.parent = constructArea.transform.GetChild(0).transform;
            meshCollider.isTrigger = true;
            gameObject.transform.tag = "statuePart";
            allowRotation = false;
            objectPlaced = true;
            metsanTarget.DragSpeech("Hyvä lisäys! Vetäkää lisää esineitä patsaaseen jos haluatte tai painakaa Valmis-nappia kun patsas on mielestänne valmis.", 30f);
            statueReadyButton.SetActive(true);

        }
        else
        {

            transform.position = originalPos;
            startRotate = true;
            allowRotation = false;
            objectPlaced = true;
            transform.rotation = originalRot;
            metsanTarget.DragSpeech("Esineen pitää olla kiinni patsaassa! Kokeile uudestaan!", 30f);
        }

        statueSetButtons[indexNumber].SetActive(false);
    }

    private void OnEnable()
    {
        if (gameObject.transform.tag != "statuePart")
        {
            transform.position = originalPos;
            transform.rotation = originalRot;
            startRotate = true;
            allowRotation = false;
            allowDragging = true;
            dragging = false;
            objectPlaced = true;
        }
    }

    void OnDisable() {
		StopAllCoroutines ();
        metsanTarget.ResetTouchScreen();
        if (gameObject.transform.tag != "statuePart")
        {
            ResetPosition();
            statueSetButtons[indexNumber].SetActive(false);
        }
    }

	void OnMouseDown()
	{

		if (allowDragging && objectPlaced)
        {

            if (gameObject.name == "Rock")
            {
                statueAS.clip = acm.natureRoom2Clips[4];
                statueAS.Play();
            }
            else if (indexNumber == 5 || indexNumber == 6 || indexNumber == 7)
            {
                statueAS.clip = acm.natureRoom2Clips[4];
                statueAS.Play();
            }
            else if (indexNumber == 0 || indexNumber == 1 || indexNumber == 2)
            {
                statueAS.clip = acm.natureRoom2Clips[7];
                statueAS.Play();
            }
            else if (indexNumber == 3 || indexNumber == 4)
            {
                statueAS.clip = acm.natureRoom2Clips[0];
                statueAS.Play();
            }

			dragging = true;
		}

		if (allowRotation) {

			rotating = true;

		}
	}

	void OnMouseUp()
	{

		dragging = false;

		if (decorationInConstructArea) {

			if (startRotate) {

                if (indexNumber == 5 || indexNumber == 6 || indexNumber == 7)
                {
                    statueAS.clip = acm.natureRoom2Clips[3];
                    statueAS.Play();
                }
                else if (indexNumber == 0 || indexNumber == 1 || indexNumber == 2)
                {
                    statueAS.clip = acm.natureRoom2Clips[6];
                    statueAS.Play();
                }
                else if (indexNumber == 3 || indexNumber == 4)
                {
                    statueAS.clip = acm.natureRoom2Clips[2];
                    statueAS.Play();
                }

                statueSetButtons[indexNumber].SetActive(true);

				startRotate = false;
				allowRotation = true;
                objectPlaced = false;
				metsanTarget.DragSpeech("Voitte vielä pyörittää esinettä vetämällä sitä tai asettaa sen paikoilleen Aseta esine-nappulalla.", 30f);
			}
		}

		if (rotating) {

			rotating = false;


			if (onStatueArea) {

                statueSetButtons[indexNumber].SetActive(false);

                gameObject.transform.parent = constructArea.transform.GetChild (0).transform;
				meshCollider.isTrigger = true;
				gameObject.transform.tag = "statuePart";
				allowRotation = false;
                objectPlaced = true;
				metsanTarget.DragSpeech("Hyvä lisäys! Vetäkää lisää esineitä patsaaseen jos haluatte tai painakaa Valmis-nappia kun patsas on tarpeeksi hieno.", 30f);
				statueReadyButton.SetActive (true);

            } else {

                statueSetButtons[indexNumber].SetActive(false);

                transform.position = originalPos;
				startRotate = true;
				allowRotation = false;
                objectPlaced = true;
				transform.rotation = originalRot;
				metsanTarget.DragSpeech("Esineen pitää olla kiinni patsaassa! Kokeile uudestaan!", 30f);
            }
		}
			

		if (allowDragging == true)
		{
			transform.position = originalPos;
		}

		if (stoneInConstructArea) {

            statueAS.clip = acm.natureRoom2Clips[3];
            statueAS.Play();

			transform.position = constructArea.transform.position;

			gameObject.transform.parent = constructArea.transform.GetChild (0).transform;

			meshCollider.isTrigger = true;
			gameObject.transform.tag = "statuePart";
			metsanTarget.DragSpeech("Hyvä, nyt meillä on patsaan pohja! Lisätään siihen vielä koristeita vetämällä ruudulla näkyviä esineitä siihen.", 30f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		if (dragging)
		{
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -localCamera.transform.position.z + z;
            transform.position = localCamera.ScreenToWorldPoint(mousePos);
		}

		if (rotating) {

			Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);

			Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint (Input.mousePosition);

			float angle = Vector2.Angle (positionOnScreen, mouseOnScreen);

			transform.rotation = Quaternion.Euler(new Vector3(0,0, angle * rotatingSpeed));

		}
	}

	public void ResetPosition() {

		meshCollider.isTrigger = false;
		transform.position = originalPos;
		transform.rotation = originalRot;
		transform.parent = statueConstruct.transform;
		onStatueArea = false;
		allowDragging = true;
		startRotate = true;
		stoneInConstructArea = false;
		decorationInConstructArea = false;
		tag = "Untagged";
	}

	private void OnTriggerStay(Collider other) {

		// construct area for stone
		if (other.gameObject.name == "constructArea" && gameObject.name == "Rock") {

			stoneInConstructArea = true;
			allowDragging = false;

		}
		// stick
		if (other.gameObject.name == "constructArea" && gameObject.name == "stick") {

			decorationInConstructArea = true;
			allowDragging = false;

		}
		// stick2
		if (other.gameObject.name == "constructArea" && gameObject.name == "stick2") {

			decorationInConstructArea = true;
			allowDragging = false;

		}
		// stick3
		if (other.gameObject.name == "constructArea" && gameObject.name == "stick3") {

			decorationInConstructArea = true;
			allowDragging = false;

		}
		// cone
		if (other.gameObject.name == "constructArea" && gameObject.name == "pinecone") {

			decorationInConstructArea = true;
			allowDragging = false;

		}
		// cone2
		if (other.gameObject.name == "constructArea" && gameObject.name == "pinecone2") {

			decorationInConstructArea = true;
			allowDragging = false;

		}
		// rock
		if (other.gameObject.name == "constructArea" && gameObject.name == "rock2") {

			decorationInConstructArea = true;
			allowDragging = false;

		}
		// rock2
		if (other.gameObject.name == "constructArea" && gameObject.name == "rock1") {

			decorationInConstructArea = true;
			allowDragging = false;

		}
		// rock3
		if (other.gameObject.name == "constructArea" && gameObject.name == "rock3") {

			decorationInConstructArea = true;
			allowDragging = false;

		}


		// stone on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "Rock") {

			onStatueArea = true;

		}
		// stick on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "stick") {

			onStatueArea = true;

		}
		// stick2 on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "stick2") {

			onStatueArea = true;

		}
		// stick3 on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "stick3") {

			onStatueArea = true;

		}
		// cone on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "pinecone") {

			onStatueArea = true;

		}
		// cone2 on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "pinecone2") {

			onStatueArea = true;

		}
		// rock on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "rock2") {

			onStatueArea = true;

		}
		// rock2 on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "rock1") {

			onStatueArea = true;

		}
		// rock3 on statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "rock3") {

			onStatueArea = true;

		}
	}

	private void OnTriggerExit(Collider other) {

		// stone
		if (other.gameObject.name == "constructArea" && gameObject.name == "Rock") {

			stoneInConstructArea = false;
			allowDragging = true;
		}
		// stick
		if (other.gameObject.name == "constructArea" && gameObject.name == "stick") {

			decorationInConstructArea = false;
			allowDragging = true;
		}
		// stick2
		if (other.gameObject.name == "constructArea" && gameObject.name == "stick2") {

			decorationInConstructArea = false;
			allowDragging = true;
		}
		// stick3
		if (other.gameObject.name == "constructArea" && gameObject.name == "stick3") {

			decorationInConstructArea = false;
			allowDragging = true;
		}

		// cone
		if (other.gameObject.name == "constructArea" && gameObject.name == "pinecone") {

			decorationInConstructArea = false;
			allowDragging = true;
		}
		// cone2
		if (other.gameObject.name == "constructArea" && gameObject.name == "pinecone2") {

			decorationInConstructArea = false;
			allowDragging = true;
		}
		// rock
		if (other.gameObject.name == "constructArea" && gameObject.name == "rock2") {

			decorationInConstructArea = false;
			allowDragging = true;
		}
		// rock2
		if (other.gameObject.name == "constructArea" && gameObject.name == "rock1") {

			decorationInConstructArea = false;
			allowDragging = true;
		}
		// rock3
		if (other.gameObject.name == "constructArea" && gameObject.name == "rock3") {

			decorationInConstructArea = false;
			allowDragging = true;
		}


		// stone off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "Rock") {

			onStatueArea = false;

		}
		// stick off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "stick") {

			onStatueArea = false;

		}
		// stick2 off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "stick2") {

			onStatueArea = false;

		}
		// stick3 off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "stick3") {

			onStatueArea = false;

		}
		// cone off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "pinecone") {

			onStatueArea = false;

		}
		// cone2 off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "pinecone2") {

			onStatueArea = false;

		}
		// rock off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "rock2") {

			onStatueArea = false;

		}
		// rock2 off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "rock1") {

			onStatueArea = false;

		}
		// rock3 off statue
		if (other.transform.parent.tag == "statuePart" && gameObject.name == "rock3") {

			onStatueArea = false;

		}
	}
}
