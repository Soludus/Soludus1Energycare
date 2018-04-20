using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueEnergy3 : BaseTargetScript
{
    public GameObject bearSpeechBubble;
    public Animator bearAnim;
    public Transform bearHand;

    public bool textInput = false;
    string filePath = "/WastedFood.txt";

    public GameObject ruokaCanvas;

    public GameObject table;
    public GameObject trashBin;
    public GameObject choppingBoard;

    public Camera localCamera;

    public Animator trackableFairyAnim;

    public int leftoverDecision;

    Vector3 bearOriginalPos;

    public GameObject targetSprite;

    public GameObject replayDialog;

    bool setInitReferences = true;

    private void OnEnable()
    {
        if (setInitReferences)
        {
            bearOriginalPos = bear.transform.position;
            setInitReferences = false;
        }
        StartCoroutine(StartTimeLine());
    }

    IEnumerator StartTimeLine()
    {
        if (dataActionController != null && dataAction.Length > 0 && dataAction[0] != null)
        {
            dataActionController.RunAction(dataAction[0]);
        }

        targetSprite.SetActive(false);
        //acm.currentFairyAudioSource = fairyAudioSource;
        //acm.currentBearAudioSource = bearAudioSource;
        bear.transform.position = bearOriginalPos;
        textInput = false;

        localFairy.SetActive(false);

        trackableFairyAnim.SetTrigger("celebrate");
        choppingBoard.SetActive(true);
        bearAnim.Play("pickupidleanim");

        fairySpeechAS.clip = acm.fairyDialoqueClips[36];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Katsoppas tuota, satuimmekin sopivasti paikalle Otson ruoka-aikaan.", 1f));
        StartCoroutine(WalkBear(trashBin.transform.position, 1));

        fairySpeechAS.clip = acm.fairyDialoqueClips[37];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso mitä sinä oikein kokkaat kun noin paljon joudut pois heittämään?", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[38];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Onko sinusta ruuanvalmistus voimia vievää puuhaa?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        choppingBoard.SetActive(false);
        bearAnim.Play("idleanim");
        yield return new WaitForSeconds(1f);
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[39];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mutta tiedätkös, ruoka-aineksien valmistaminen on kovaa hommaa luonnollekin. Se vie paljon aikaa, vaivaa ja voimaa.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[40];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kun heitämme ruokaa pois, kaikki tuo vaiva on mennyt hukkaan. Mitä paremmin hyödynnämme kaiken hankkimamme ja lautaselle ottamamme ruuan, sitä enemmän voimia luonnolta säästyy.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[41];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Meidän kannattaakin siis aina ottaa lautaselle sopivan kokoinen annos, että jaksamme syödä kaiken.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[42];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Jos ruokaa jää aterialta yli, sen voi syödä seuraavana päivänä tai käyttää osana jotakin uutta ruokalajia. Roskiin kannattaa heittää vain sellainen ruokajäte, jota ei pysty käyttämään uudestaan.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[43];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Jaksattekos te yleensä syödä kaiken, minkä otatte lautaselle? Paljonko ruokaa teillä päiväkodissa meni viime aterialta hukkaan?", 1f));
        fairySpeechAS.Stop();

        //speechCO = StartCoroutine(ShowFairySpeech("KESKUSTELKAA: PALJONKO RUOKAA TEILLÄ MENI VIIME ATERIALLA HUKKAAN? TARKISTAKAA PÄIVÄKODIN HUKKALUKU, JA SYÖTTÄKÄÄ SE KENTTÄÄN.", 5f));

        replayDialog.SetActive(false);
        menu.SetActive(false);
        ruokaCanvas.SetActive(true);
        yield return StartCoroutine(WaitForTextInput());
        WriteToFile();
        ruokaCanvas.SetActive(false);
        menu.SetActive(true);
        //ReadFromFile ();
        replayDialog.SetActive(true);

        table.transform.GetChild(0).gameObject.SetActive(true);
        table.transform.GetChild(1).gameObject.SetActive(true);
        table.transform.GetChild(2).gameObject.SetActive(true);
        table.transform.GetChild(3).gameObject.SetActive(true);

        StartCoroutine(WalkBear(bearOriginalPos, 2));

        fairySpeechAS.clip = acm.fairyDialoqueClips[44];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("On hienoa, jos osaa arvioida, minkä verran aikoo syödä.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[45];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Ei silti kannata alkaa pihistellä annoskoon kanssa. On myös tärkeää syödä monipuolisesti ja tarpeeksi paljon, jotta itselläkin riittää tarmoa luonnon auttamiseen.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[46];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Te varmasti osaatte opettaa Otsoa, jotta hänkin osaisi ottaa hyvän annoksen ruokaa ja oppisi erottelemaan, mitkä ruuantähteet kannattaa säästää ja mitkä heittää pois.", 1f));
        fairySpeechAS.Stop();

        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kootkaa Otson lautaselle monipuolinen ateria tarjolla olevista aineksista!", 1f));

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);

        targetSprite.SetActive(false);
        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, true, false, false, false, false, false, false, false, false, false);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(2, false);
    }

    void WriteToFile()
    {
        // esim: 1.0kg, päiväys: 1.1.2017 12:30:00
        string foodInput = ruokaCanvas.GetComponentInChildren<InputField>().text + "kg, päivä: " + System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");

        StreamWriter writer = new StreamWriter(Application.persistentDataPath + filePath, true);
        writer.WriteLine(foodInput);
        writer.Close();
    }

    public IEnumerator WalkBear(Vector3 pos, int action)
    {
        pos.y = bear.transform.position.y;

        // positioning for trashbin
        if (action == 1)
        {
            pos.x -= 0.25f;
            pos.z += 0.125f;
        }

        bearAnim.SetBool("walking", true);
        while (Vector3.Distance(bear.transform.position, pos) > 0.01f * transform.localScale.x)
        {
            bear.transform.position += (pos - bear.transform.position).normalized * Time.deltaTime * 0.2f * transform.localScale.x;
            bear.transform.LookAt(pos);
            yield return null;
        }

        bearAnim.SetBool("walking", false);

        if (action == 1)
        {
            pos.x += 0.25f;
            pos.z -= 0.125f;
            bear.transform.LookAt(trackableFairy.transform.position);
        }
        else if (action == 2)
        {
            bear.transform.LookAt(trackableFairy.transform.position);
        }

        yield return new WaitForSeconds(2f);
        if (action == 1)
        {
            bear.transform.LookAt(trackableFairy.transform.position);
        }
    }

    IEnumerator WaitForTextInput()
    {
        yield return new WaitUntil(() => textInput == true);
    }
}
