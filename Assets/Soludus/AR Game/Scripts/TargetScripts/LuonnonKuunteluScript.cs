using UnityEngine;
using System.Collections;

public class LuonnonKuunteluScript : BaseTargetScript
{
    public bool inputOn;

    [SerializeField] Transform drawingLocal = null;
    [SerializeField] Transform drawingArea = null;

    Color selectedColor;

    bool pictureMode;
    [SerializeField] Camera localCam = null;

    private bool completed;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public GameObject checkbutton;
    public GameObject buttons;

    private int questState;

    public Camera localCamera;
    public GameObject star;

    public Animator trackableFairyAnim;

    Vector3 bearOriginalPos;

    Texture2D textuuri;
    Vector2 oldPixelUV;

    private bool showNatureOnce;

    Coroutine speechCO;

    void Awake()
    {
        // texture for the drawing quest of the map
        textuuri = new Texture2D(1024, 1024);
        drawingArea.GetComponent<Renderer>().material.mainTexture = textuuri;

        // setting the texture color to white
        Color32[] resetColorArray = textuuri.GetPixels32();

        for (int i = 0; i < resetColorArray.Length; i++)
        {
            resetColorArray[i] = Color.white;
        }
        textuuri.SetPixels32(resetColorArray);
        textuuri.Apply();

        completed = false;

        // testing quest state
        questState = 0;

        inputOn = true;

        pictureMode = true;

        selectedColor = Color.black;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();
        bearOriginalPos = bear.transform.position;

        showNatureOnce = false;
    }

    void Update()
    {
        // level completed
        if (questState == 1 && completed == false)
        {
            completed = true;

            engine.IncrementScore(5);

            star.SetActive(true);

            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetTrigger("celebrate");
            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
            StartCoroutine(VictorySpeech());
        }

        Vector3 inputVector;
        //Raycasting, both for picturemode and level view (currently no use for level view raycasting)
        if (TouchScreenScript.GetTouch(out inputVector) && inputOn)
        {
            if (!pictureMode)
            {
                //Ray ray = Camera.main.ScreenPointToRay(inputVector);
                //RaycastHit hit;

                //if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity) && inputOn)
                //{
                //    print(hit.collider.gameObject.name);
                //}
            }
            else
            {
                Ray ray = localCam.ScreenPointToRay(inputVector);
                RaycastHit hit;


                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, (1 << 8)) && inputOn)
                {
                    if (hit.collider.tag == "ColorCube")
                    {
                        selectedColor = hit.collider.GetComponent<MeshRenderer>().material.color;
                    }

                    if (hit.collider.name == "CheckButton")
                    {
                        CheckIfDrawingFinished();
                    }
                    if (hit.collider.name == "ConfirmButton")
                    {
                        fairySpeechAS.Stop();
                        buttons.SetActive(false);
                        StartCoroutine(EndTimeLine());
                    }
                    if (hit.collider.name == "DenyButton")
                    {
                        buttons.SetActive(false);
                        checkbutton.SetActive(true);
                        drawingLocal.gameObject.SetActive(true);
                        StopCoroutine(speechCO);
                        trackableFairySpeechBubble.SetActive(false);
                        fairySpeechAS.Stop();
                    }
                }
            }
        }
    }

    IEnumerator VictorySpeech()
    {
        if (dataActionController != null && dataAction.Length > 1 && dataAction[1] != null)
        {
            dataActionController.RunAction(dataAction[1]);
        }

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(27, false);
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    void FixedUpdate()
    {
        // this is used to draw the line to the texture
        if (Input.GetMouseButton(0))
        {
            Vector3 inputVector = Vector3.zero;
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
                inputVector = Input.touches[0].position;
            inputVector = Input.mousePosition;

            Ray ray = localCam.ScreenPointToRay(inputVector);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, (1 << 8)) && inputOn)
            {
                if (hit.collider.name == "boxCollider")
                {
                    oldPixelUV.x = 0;
                    oldPixelUV.y = 0;
                }

                if (hit.collider.name == "DrawingArea")
                {
                    Vector2 currentPixelUV = hit.textureCoord;

                    // new circle is drawn if the selected coordination is different from the old one
                    if (oldPixelUV.x != 0 && currentPixelUV != oldPixelUV)
                    {
                        Vector2 tempPixelUV = currentPixelUV;
                        tempPixelUV.x *= textuuri.width;
                        tempPixelUV.y *= textuuri.height;

                        oldPixelUV.x *= textuuri.width;
                        oldPixelUV.y *= textuuri.height;

                        int changeX = (int)(oldPixelUV.x - tempPixelUV.x);
                        int changeY = (int)(oldPixelUV.y - tempPixelUV.y);

                        changeX = Mathf.Abs(changeX);
                        changeY = Mathf.Abs(changeY);

                        float kulmakerroin;
                        int muutosX = 1;

                        if (changeX != 0)
                        {
                            kulmakerroin = (float)changeY / changeX;
                        }
                        else
                        {
                            kulmakerroin = 1;
                            muutosX = 0;
                        }

                        float yFloat = 0;

                        int drawTimes = 0;
                        // circles are drawn for the longer distance of the x and y
                        while (drawTimes < changeX || drawTimes < changeY)
                        {
                            if ((int)oldPixelUV.x != (int)tempPixelUV.x)
                            {
                                if ((int)oldPixelUV.x < (int)tempPixelUV.x)
                                {
                                    tempPixelUV.x -= muutosX;
                                }
                                else
                                {
                                    tempPixelUV.x += muutosX;
                                }
                            }

                            if ((int)oldPixelUV.y != (int)tempPixelUV.y)
                            {

                                if ((int)oldPixelUV.y < (int)tempPixelUV.y)
                                {

                                    if (kulmakerroin >= 1f)
                                    {
                                        tempPixelUV.y -= Mathf.Round(kulmakerroin);
                                    }
                                    else
                                    {
                                        yFloat += kulmakerroin;
                                        if (yFloat >= 1f)
                                        {
                                            tempPixelUV.y -= Mathf.Round(yFloat);
                                            yFloat = 0;
                                        }
                                    }

                                }
                                else
                                {

                                    if (kulmakerroin >= 1f)
                                    {
                                        tempPixelUV.y += Mathf.Round(kulmakerroin);
                                    }
                                    else
                                    {
                                        yFloat += kulmakerroin;
                                        if (yFloat >= 1f)
                                        {
                                            tempPixelUV.y += Mathf.Round(yFloat);
                                            yFloat = 0;
                                        }
                                    }
                                }
                            }
                            // the longer the distance, the more seldom is the circle drawing frequency. This reduces lag with long distances, but greatly reduces line drawing accuracy
                            if (changeX <= 20 && changeY <= 20)
                            {
                                if (drawTimes % 1 == 0)
                                {
                                    Circle(textuuri, (int)tempPixelUV.x, (int)tempPixelUV.y, 32, selectedColor);
                                }
                            }
                            else if (changeX > 20 && changeX <= 40 && changeY <= 40 || changeY > 20 && changeY <= 40 && changeX <= 40)
                            {
                                if (drawTimes % 2 == 0)
                                {
                                    Circle(textuuri, (int)tempPixelUV.x, (int)tempPixelUV.y, 32, selectedColor);
                                }
                            }
                            else if (changeX > 40 && changeX <= 80 && changeY <= 80 || changeY > 40 && changeY <= 80 && changeX <= 80)
                            {
                                if (drawTimes % 4 == 0)
                                {
                                    Circle(textuuri, (int)tempPixelUV.x, (int)tempPixelUV.y, 32, selectedColor);
                                }
                            }
                            else if (changeX > 80 && changeX <= 160 && changeY <= 160 || changeY > 80 && changeY <= 160 && changeX <= 160)
                            {
                                if (drawTimes % 8 == 0)
                                {
                                    Circle(textuuri, (int)tempPixelUV.x, (int)tempPixelUV.y, 32, selectedColor);
                                }
                            }
                            else if (changeX > 160 && changeX <= 320 && changeY <= 320 || changeY > 160 && changeY <= 320 && changeX <= 320)
                            {
                                if (drawTimes % 16 == 0)
                                {
                                    Circle(textuuri, (int)tempPixelUV.x, (int)tempPixelUV.y, 32, selectedColor);
                                }
                            }
                            else if (changeX > 320 && changeX <= 640 && changeY <= 640 || changeY > 320 && changeY <= 640 && changeX <= 640)
                            {
                                if (drawTimes % 32 == 0)
                                {
                                    Circle(textuuri, (int)tempPixelUV.x, (int)tempPixelUV.y, 32, selectedColor);
                                }
                            }
                            else if (changeX > 640 && changeX <= 1280 && changeY <= 1280 || changeY > 640 && changeY <= 1280 && changeX <= 1280)
                            {
                                if (drawTimes % 64 == 0)
                                {
                                    Circle(textuuri, (int)tempPixelUV.x, (int)tempPixelUV.y, 32, selectedColor);
                                }
                            }
                            else
                            {
                                if (drawTimes % 16 == 0)
                                {
                                    Circle(textuuri, (int)tempPixelUV.x, (int)tempPixelUV.y, 32, selectedColor);
                                }
                            }

                            drawTimes += 1;
                        }
                        textuuri.Apply();
                    }

                    oldPixelUV = currentPixelUV;
                }
            }
        }

        if (!Input.GetMouseButton(0))
        {
            oldPixelUV.x = 0;
            oldPixelUV.y = 0;
        }
    }

    public void Circle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        int x, y, px, nx, py, ny, d;

        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                tex.SetPixel(px, py, col);
                tex.SetPixel(nx, py, col);
                tex.SetPixel(px, ny, col);
                tex.SetPixel(nx, ny, col);
            }
        }
    }

    IEnumerator StartTimeLine()
    {
        if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
        {
            dataActionController.RunAction(dataAction[0]);
        }

        bear.SetActive(true);

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Piirtäkää sormella ja vaihtakaa väriä, kun haluatte. Kun kuva on valmis, painakaa merkkiä alakulmassa!", 1f));

        inputOn = true;
        pictureMode = true;
        drawingLocal.gameObject.SetActive(true);
        checkbutton.SetActive(true);

        speechCO = StartCoroutine(ShowFairySpeech("PIIRTÄKÄÄ KUULEMANNE ÄÄNIMAISEMA ERI VÄREILLÄ.", 5f));
    }

    public void StartEndTimeLine()
    {
        StopCoroutine(speechCO);
        StartCoroutine(EndTimeLine());
    }

    IEnumerator EndTimeLine()
    {
        StopCoroutine(speechCO);
        fairySpeechAS.clip = acm.fairyDialoqueClips[226];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kiitos! Laitapa, Otso, kuva hyvään talteen!", 1f));

        StartCoroutine(IncQuestStateAfterDelay());
    }

    void CheckIfDrawingFinished()
    {
        StopCoroutine(speechCO);
        drawingLocal.gameObject.SetActive(false);
        checkbutton.SetActive(false);
        fairySpeechAS.clip = acm.fairyDialoqueClips[97];
        fairySpeechAS.Play();
        speechCO = StartCoroutine(ShowFairySpeech("Onko kuva valmis?", 3f));
        buttons.SetActive(true);
    }

    IEnumerator IncQuestStateAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        questState++;
    }

    private void OnEnable()
    {
        menu.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
        localFairy.SetActive(false);
        trackableFairy.SetActive(true);

        bear.SetActive(true);
        bear.transform.position = bearOriginalPos;
        bear.transform.LookAt(trackableFairy.transform.position);
        bear.SetActive(false);

        engine.LoadGame();

        star.SetActive(false);

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(5).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(28, true);

            completed = false;

            StartCoroutine(StartTimeLine());

            questState = 0;
        }
        else
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[98];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Otso sai jo kuvan äänimaisemasta! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));

        }
    }

    private void OnDisable()
    {
        if (menu != null)
        {
            menu.SetActive(true);
        }
        drawingLocal.gameObject.SetActive(false);
        checkbutton.SetActive(false);
        if (localFairy != null)
        {
            localFairy.SetActive(true);
        }
        trackableFairySpeechBubble.SetActive(false);
        buttons.SetActive(false);
        StopAllCoroutines();
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
