using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNature3 : BaseTargetScript
{
    public GameObject bearSpeechBubble;
    public Animator bearAnim;

    public Animator trackableFairyAnim;

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

        localFairy.SetActive(false);

        trackableFairyAnim.SetTrigger("celebrate");
        bear.SetActive(true);
        bearAnim.SetBool("sitting", true);
        bearAnim.Play("sittinganim");

        fairySpeechAS.clip = acm.fairyDialoqueClips[91];
        fairySpeechAS.Play();
        replayDialog.SetActive(true);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = fairySpeechAS;
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Onpas Otso kerrankin rauhallinen. Hän taitaa kuunnella metsän ääniä.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[92];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Luonnossa tapahtuukin vaikka mitä sellaista jännittävää, minkä tarkkakorvainen kuuntelija voi havaita. Eläimet, kasvit ja tuuli voivat maalata oikein äänimaiseman.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[93];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Kokeilkaapa tekin! Olkaa hetki aivan hiljaa, sulkekaa silmänne ja koettakaa kuunnella, millainen äänimaisema ympärillänne aukeaa! Keskustelkaa sen jälkeen yhdessä, millaisia ääniä kuulitte!", 1f));
        fairySpeechAS.Stop();

        //yield return StartCoroutine(ShowFairySpeechWaitForInput("KUUNNELKAA SILMÄT KIINNI LUONNON ÄÄNIÄ. KESKUSTELKAA KUULEMASTANNE.", 5f));

        fairySpeechAS.clip = acm.fairyDialoqueClips[94];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Hienoja havaintoja! Olettepa te tarkkaavaisia!", 1f));

        bearAnim.SetBool("sitting", false);

        fairySpeechAS.clip = acm.fairyDialoqueClips[95];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Otsokin haluaisi kokea kuulemanne äänimaiseman. Tehän voisitte piirtää sen Otsolle! Käyttäkää eri värejä kuvaamaan erilaisia ääniä.", 1f));
        fairySpeechAS.clip = acm.fairyDialoqueClips[96];
        fairySpeechAS.Play();
        yield return StartCoroutine(ShowFairySpeechWaitForInput("Piirtäkää sormella ja vaihtakaa väriä, kun haluatte. Kun kuva on valmis, painakaa merkkiä alakulmassa!", 1f));
        fairySpeechAS.Stop();

        replayDialog.SetActive(false);
        replayDialog.GetComponent<ReplayDialogue>().currentDialogueAS = null;

        menu.SetActive(false);
        targetSprite.SetActive(true);
        yield return StartCoroutine(ShowFairySpeech("Etsikää tämä rasti!", 5f));
        menu.SetActive(true);
        targetSprite.SetActive(false);

        // show image of the target

        //GameObject.Find("ARTargets").GetComponent<ImageTargetManagerScript>().ActivateImageTargets(false, false, false, false, false, true, false, false, false, false, false, false);

        localFairy.SetActive(true);
        //acm.currentBearAudioSource = null;
        //acm.currentFairyAudioSource = null;
        //gameObject.SetActive(false);
        FindObjectOfType<ImageTargetManagerScript>().ActivateTarget(5, false);
    }
}
