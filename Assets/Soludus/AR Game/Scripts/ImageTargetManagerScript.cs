using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTargetManagerScript : MonoBehaviour {

    public List<MonoBehaviour> imageTargets;
    public List<MonoBehaviour> dialogScripts;
    public List<MonoBehaviour> targetScripts;

    public void ActivateTarget(int index, bool isDialogActive)
    {
        imageTargets[index].enabled = !isDialogActive;
        dialogScripts[index].enabled = isDialogActive;
        targetScripts[index].enabled = !isDialogActive;

        targetScripts[index].gameObject.SetActive(isDialogActive);
        SetLayer(targetScripts[index].transform, isDialogActive ? LayerMask.NameToLayer("localObjects") : LayerMask.NameToLayer("Default"));
    }

    public void DisableAllTargets()
    {
        for (int i = 0; i < imageTargets.Count; i++)
        {
            imageTargets[i].enabled = false;
        }
        for (int i = 0; i < dialogScripts.Count; i++)
        {
            dialogScripts[i].gameObject.SetActive(false);
        }
    }

    private void SetLayer(Transform t, int layer)
    {
        t.gameObject.layer = layer;
        for (int i = 0; i < t.childCount; i++)
        {
            SetLayer(t.GetChild(i), layer);
        }
    }
}
