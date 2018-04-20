using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePollution2 : BaseTargetScript
{
    public Animator bearAnim;

    public GameObject car;

    public Animator trackableFairyAnim;

    public AudioSource carAudioSource;

    public GameObject targetSprite;

    public GameObject replayDialog;

    private void OnEnable()
    {
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

        car.SetActive(false);
        bear.SetActive(false);
        localFairy.SetActive(false);

        carAudioSource.Play();
        trackableFairyAnim.Play("AnimIdleTurn");

        fairySpeechAS.clip = acm.fairyDialoqueClips[118];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Ohhoh! Liekö tuo Otson automobiili, jonka näen kaasuttavan metsästä tänne päin?", 1f));
        fairySpeechAS.Stop();
        replayDialog.SetActive(false);

        yield return new WaitForSeconds(1f);
        car.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        carAudioSource.Stop();
        bear.SetActive(true);
        bearAnim.SetTrigger("celebrate");
        yield return new WaitForSeconds(2.5f);
        fairySpeechAS.clip = acm.fairyDialoqueClips[119];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otso! Marjametsältäkö sinä olet tulossa?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        bearAnim.SetTrigger("nod");
        yield return new WaitForSeconds(2f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[120];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Mutta sinullahan on marjamättäitä kotimetsä pullollaan! Eiköhän polkupyörä olisi ollut autoa parempi kulkupeli marjareissulle?", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        bearAnim.SetTrigger("headDown");
        yield return new WaitForSeconds(3f);
        replayDialog.SetActive(true);

        fairySpeechAS.clip = acm.fairyDialoqueClips[121];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Autot ja muut moottoriajoneuvot saastuttavat paljon. Olisikin parempi kulkea lyhyet matkat polkupyörällä tai jalan.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[122];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Pidemmillä matkoilla kannattaa auton sijasta suosia joukkoliikennettä, vaikkapa junaa tai bussia, joiden kyytiin mahtuu monta autokyydillistä väkeä.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[123];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Valitsemalla pyörän tai julkisen kulkuneuvon voimme myös säästää luonnon voimaa:", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[124];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Samalla määrällä energiaa bussi kulkee kaksi kertaa niin pitkälle kuin auto, juna kahdeksan kertaa, ja pyörä milteipä kaksikymmentäkuusi kertaa niin pitkälle!", 1f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[229];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Jotkut teistä tulevat päiväkotiin kauempaa ja toiset lähempää. Millä kulkupelillä te tänään saavuitte? Keskustelkaapa siitä!", 1f));
        fairySpeechAS.Stop();

        bearAnim.SetBool("thinking", true);

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("KESKUSTELKAA: MILLÄ KULKUNEUVOLLA TE SAAVUITTE TÄNÄÄN?", 3f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[230];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Tepä taidattekin, lapset, tietää nykymaailmassa liikennöimisestä vielä enemmän kuin minä! Opetetaan yhdessä Otsolle, mikä olisi paras menopeli erilaisille matkoille!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);

        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, false, false, true, false, false, false, false);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        //gameObject.SetActive(false);
        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(7, false);
    }
}
