using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LiikkumisTargetScript : BaseTargetScript
{
    private bool completed;

    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public GameObject car;
    public GameObject bicycle;

    public GameObject liikkumisVehicles;

    private int questState;

    public Camera localCamera;

    public GameObject star;

    public Animator trackableFairyAnim;

    Vector3 bearOriginalPos;
    private bool inputOn;

    private bool showNatureOnce;

    Coroutine speechCO;

    public AudioSource carAudioSource;

    public GameObject replayDialog;

    // Use this for initialization
    void Awake()
    {

        completed = false;

        // testing quest state
        questState = 0;

        // init
        trackableFairyAnim = trackableFairy.GetComponent<Animator>();
        bearAnim = bear.GetComponent<Animator>();
        bearOriginalPos = bear.transform.position;

        showNatureOnce = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (questState == 4 && completed == false)
        {
            completed = true;

            engine.IncrementScore(7);

            star.SetActive(true);

            car.SetActive(false);
            bicycle.SetActive(true);

            trackableFairyAnim.SetTrigger("celebrate");
            bearAnim.SetBool("thinking", false);
            bearAnim.Play("idleanim");
            bearAnim.SetTrigger("celebrate");
            bear.transform.LookAt(new Vector3(Camera.main.transform.position.x, bear.transform.position.y, Camera.main.transform.position.z));
            StartCoroutine(VictorySpeech());
        }

        if (inputOn)
        {
            Vector3 inputVector;
            if (TouchScreenScript.GetTouch(out inputVector))
            {
                Ray ray = localCamera.ScreenPointToRay(inputVector);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, (1 << 8)) && inputOn)
                {

                    if (hit.collider.name == "bike")
                    {

                        inputOn = false;
                        StartSelectionA();

                    }
                    else if (hit.collider.name == "kupla")
                    {

                        inputOn = false;
                        StartSelectionB();

                    }
                    else if (hit.collider.name == "submarine")
                    {

                        inputOn = false;
                        StartSelectionC();

                    }
                    else if (hit.collider.name == "kupla2")
                    {

                        inputOn = false;
                        StartSelectionA();

                    }
                    else if (hit.collider.name == "kupla3")
                    {

                        inputOn = false;
                        StartSelectionA();

                    }
                    else if (hit.collider.name == "train")
                    {

                        inputOn = false;
                        StartSelectionB();

                    }
                    else if (hit.collider.name == "airplane")
                    {

                        inputOn = false;
                        StartSelectionC();

                    }
                    else if (hit.collider.name == "bike2")
                    {

                        inputOn = false;
                        StartSelectionB();

                    }
                    else if (hit.collider.name == "wheelbarrow")
                    {

                        inputOn = false;
                        StartSelectionC();

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

        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(6, false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[141];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienosti tehty, nyt Otso tietää paljon enemmän menopeleistä ja siitä, millä kannattaa mihinkin matkustaa! Kiitos!", 1f));
        fairySpeechAS.Stop();
        yield return StartCoroutine(ShowFairySpeech("Siirtäkää laite pois rastilta!", 30f));
    }

    IEnumerator StartTimeLine()
    {
        car.SetActive(true);
        bear.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[230];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tepä taidattekin, lapset, tietää nykymaailmassa liikennöimisestä vielä enemmän kuin minä! Opetetaan yhdessä Otsolle, mikä olisi paras menopeli erilaisille matkoille!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);

        if (questState == 0)
        {
            if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
            {
                dataActionController.RunAction(dataAction[0]);
            }

            bearAnim.Play("wonderinganim");
            yield return new WaitForSeconds(3f);

        }

        StartCoroutine(PlayerChooses(questState));
    }

    IEnumerator PlayerChooses(int state)
    {
        inputOn = true;

        if (state == 0)
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[125];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Otsolla on mummola naapurimetsässä. Jos hän lähtee siellä vierailemaan, millä hänen kannattaa kulkea? Sinne on melkomoisen pitkä matka!", 1f));
            fairySpeechAS.Stop();

            liikkumisVehicles.SetActive(true);
            liikkumisVehicles.GetComponent<SelectionManager>().SelectState(state);
            speechCO = StartCoroutine(ShowFairySpeech("KOSKETTAKAA MATKALLE SOPIVINTA KULKUNEUVOA.", 5f));
        }
        else if (state == 1)
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[126];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso käy keskiviikkoisin karhujen kuvataidekerhossa metsän toisella laidalla. Milläpä teidän mielestänne Otson olisi sinne paras kulkea?", 1f));
            fairySpeechAS.Stop();

            liikkumisVehicles.SetActive(true);
            liikkumisVehicles.GetComponent<SelectionManager>().SelectState(state);
            speechCO = StartCoroutine(ShowFairySpeech("KOSKETTAKAA MATKALLE SOPIVINTA KULKUNEUVOA.", 5f));
        }
        else if (state == 2)
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[127];
            fairySpeechAS.Play();
            replayDialog.SetActive(true);
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso asuu aivan päiväkodin lähimailla. Jos hän tulisi tänne vierailulle, mikä teidän mielestänne olisi paras tapa saapua?", 1f));
            fairySpeechAS.Stop();

            liikkumisVehicles.SetActive(true);
            liikkumisVehicles.GetComponent<SelectionManager>().SelectState(state);
            speechCO = StartCoroutine(ShowFairySpeech("KOSKETTAKAA MATKALLE SOPIVINTA KULKUNEUVOA.", 5f));
        }
    }

    public void StartSelectionA()
    {
        StopCoroutine(speechCO);
        liikkumisVehicles.SetActive(false);
        StartCoroutine(PlayerSelectionA());
    }

    IEnumerator PlayerSelectionA()
    {

        if (questState == 0)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[7];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[128];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Auto tulisi kyllä kysymykseen, jos naapurimetsään ei kulkisi junarataa.", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[129];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Se on ihan hyvä vaihtoehto, mutta onneksi Otso pääsee mummolaan junalla! Vähemmän saasteita syntyy, jos Otso matkustaa autotien sijasta raiteita pitkin.", 1f));
            soundEffectAS.Stop();
        }
        else if (questState == 1)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[7];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[130];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Kyllä! Kerhoon on niin pitkä matka, että sinne täytyy matkustaa autolla. Otson kannattaa kuitenkin miettiä menevänsä linja-autolla!", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[131];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Siihen mahtuu useampi henkilöautollinen väkeä, mutta se saastuttaa vähemmän kuin monta pientä autokyytiä yhteensä.", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[132];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Toinen vaihtoehto olisi kerätä kavereista kimppakyyti, joka myös vie useamman kulkijan perille samaa matkaa.", 1f));
            soundEffectAS.Stop();
        }
        else if (questState == 2)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[0];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[133];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Juuri niin! Otso asuu niin lähellä päiväkotia, että jos hän hyppäisi pyörän satulaan hän ehtisi paikalle hujauksessa.", 1f));
            fairySpeechAS.Stop();
            soundEffectAS.Stop();
        }

        questState++;

        if (questState < 3)
        {
            StartCoroutine(PlayerChooses(questState));
        }
        else
        {
            replayDialog.SetActive(false);
            StartCoroutine(IncQuestStateAfterDelay());
        }
    }

    public void StartSelectionB()
    {
        StopCoroutine(speechCO);
        liikkumisVehicles.SetActive(false);
        StartCoroutine(PlayerSelectionB());
    }

    IEnumerator PlayerSelectionB()
    {
        if (questState == 0)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[1];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[134];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoa! Juna olisi todellakin paras vaihtoehto! Juna ei saastuta yhtä paljon kuin auto tai lentokone.", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[135];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Aina kun mahdollista kannattaa muutenkin käyttää julkista liikennettä, jossa kulkee suurempi joukko samalla kertaa!", 1f));
            soundEffectAS.Stop();
        }
        else if (questState == 1)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[0];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[136];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Polkupyöräily olisi kyllä muuten paras vaihtoehto, mutta Otson kunto ei taida aivan riittää metsän toiselle laidalle polkemiseen. Tällä kertaa hänen on käytettävä autoa.", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[231];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Hän voi silti säästää energiaa ja saastuttaa vähemmän, jos hän kokoaakin autoonsa kimppakyydin useammalle kerholaiselle tai menee bussilla.", 1f));
            fairySpeechAS.Stop();
            soundEffectAS.Stop();
        }
        else if (questState == 2)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[7];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[232];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Auton kyydissä Otso varmaan mieluusti köröttelisi. Jos hän asuisi vähän kauempana hän voisi tullakin autolla, mutta näin lyhyelle matkalle pyörä olisi kyllä parempi.", 1f));
            fairySpeechAS.Stop();
            soundEffectAS.Stop();
        }

        questState++;

        if (questState < 3)
        {
            StartCoroutine(PlayerChooses(questState));
        }
        else
        {
            replayDialog.SetActive(false);
            StartCoroutine(IncQuestStateAfterDelay());
        }

    }

    public void StartSelectionC()
    {
        StopCoroutine(speechCO);
        liikkumisVehicles.SetActive(false);
        StartCoroutine(PlayerSelectionC());
    }

    IEnumerator PlayerSelectionC()
    {
        if (questState == 0)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[6];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[137];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Onneksi Otson mummola ei ole sentään niin kaukana, että sinne joutuisi lentokoneella lentämään! Naapurimetsään matkatessa juna on kyllä lentokonetta parempi kulkupeli.", 1f));
            soundEffectAS.Stop();
        }
        else if (questState == 1)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[4];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[138];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Sepäs vasta olisikin hauskaa matkustaa kottikärryillä! Mutta niihin tarvitsee jonkun työntäjäksi!", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[233];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Otson täytyy siis tällä kertaa hypätä auton kyytiin! Hänen kannattaa kuitenkin koota autoonsa kerhokavereille kimppakyyti, jossa kaikki kulkevat samaa matkaa, tai mennä bussilla!", 1f));
            soundEffectAS.Stop();
        }
        else if (questState == 2)
        {
            soundEffectAS.clip = acm.pollutionRoom2Clips[2];
            soundEffectAS.Play();
            fairySpeechAS.clip = acm.fairyDialoqueClips[139];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Sepä se vasta olisikin näky, jos Otso koettaisi sukellusveneellä saapua! Onkos tässä edes merta lähellä?", 1f));
            fairySpeechAS.clip = acm.fairyDialoqueClips[140];
            fairySpeechAS.Play();
            yield return StartCoroutine(ShowFairySpeechWaitForInput("Eiköhän kehoteta Otsoa hyppäämään sukellusveneen sijasta pyörän satulaan, jos hän päättää lähteä tänne vierailulle!", 1f));
            fairySpeechAS.Stop();
            soundEffectAS.Stop();
        }

        questState++;

        if (questState < 3)
        {
            StartCoroutine(PlayerChooses(questState));
        }
        else
        {
            replayDialog.SetActive(false);
            StartCoroutine(IncQuestStateAfterDelay());
        }

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

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-engine.CoolDownInSeconds), engine.GetScore(7).updateTimestamp) > 0)
        {
            GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusicClip(8, true);

            completed = false;

            car.SetActive(false);
            bicycle.SetActive(false);
            bear.SetActive(false);

            StartCoroutine(StartTimeLine());

        }
        else
        {
            fairySpeechAS.clip = acm.fairyDialoqueClips[142];
            fairySpeechAS.Play();
            StartCoroutine(ShowFairySpeech("Otsolle on jo löytynyt sopiva menopeli! Voitte kokeilla tätä rastia huomenna uudestaan!", 5f));

        }
    }

    private void OnDisable()
    {
        liikkumisVehicles.SetActive(false);
        if (menu != null)
        {
            menu.SetActive(true);
        }
        if (menu != null)
        {
            localFairy.SetActive(true);
        }
        trackableFairySpeechBubble.SetActive(false);
        bear.SetActive(false);
        soundEffectAS.Stop();
        StopAllCoroutines();
        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;
        if (completed && !showNatureOnce)
        {
            showNatureOnce = true;
            GameObject.Find("NatureViews").transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}