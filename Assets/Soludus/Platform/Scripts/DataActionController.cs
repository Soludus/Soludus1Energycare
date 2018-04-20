using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
class ActionDatas
{
    public ActionData[] actionDatas;
}

public class DataActionController : MonoBehaviour
{
    public string dataPath = "/actionData.json";

    public List<DataAction> dataActionList;
    public Transform dataActionHolder;

    public void Start()
    {
        LoadData();
    }

    public void RunAction(DataAction dataAction)
    {
        Debug.Log("running action " + dataAction.actionName);
        GetDataActions();

        dataAction.actionRunTime = DateTime.Now;

        SaveData();
    }

    private void GetDataActions()
    {
        dataActionList = new List<DataAction>();
        dataActionHolder.GetComponentsInChildren(dataActionList);
    }

    public void SaveData()
    {
        ActionDatas aData = new ActionDatas();

        aData.actionDatas = new ActionData[dataActionList.Count];

        for (int i = 0; i < aData.actionDatas.Length; i++)
        {
            aData.actionDatas[i].actionName = dataActionList[i].actionName;
            aData.actionDatas[i].actionRunTime = dataActionList[i].actionRunTime.ToString();
            //aData.actionDatas[i].actionValue = dataActionList[i].actionValue;
            //aData.actionDatas[i].actionDurationInSeconds = dataActionList[i].actionEffectDurationInSeconds;

            //Debug.Log("stored action: level name: " + aData.actionDatas[i].actionName + " , time: " + aData.actionDatas[i].actionRunTime);
        }

        File.WriteAllText(Application.persistentDataPath + dataPath, JsonUtility.ToJson(aData));

        //Debug.Log("writing file success!");
    }

    public void LoadData()
    {
        GetDataActions();

        if (File.Exists(Application.persistentDataPath + dataPath))
        {
            var aDClass = JsonUtility.FromJson<ActionDatas>(File.ReadAllText(Application.persistentDataPath + dataPath));

            for (int i = 0; i < aDClass.actionDatas.Length; i++)
            {
                var dataAction = dataActionList.Find((DataAction x) => x.actionName == aDClass.actionDatas[i].actionName);

                if (dataAction != null && !string.IsNullOrEmpty(dataAction.actionName))
                {
                    //Debug.Log(aDClass.actionDatas[i].startValueTime);
                    dataAction.actionRunTime = DateTime.Parse(aDClass.actionDatas[i].actionRunTime);
                    //dataAction.actionValue = aDClass.actionDatas[i].actionValue;
                    //dataAction.actionEffectDurationInSeconds = aDClass.actionDatas[i].actionDurationInSeconds;

                    //Debug.Log("loaded action: level name: " + aDClass.actionDatas[i].actionName + " , time: " + dataAction.actionRunTime + " , value: " + dataAction.actionValue + " , duration in seconds: " + dataAction.actionEffectDurationInSeconds);
                }
                else
                {
                    Debug.Log("data action is null or empty!");
                }
            }
        }
    }

    public void ResetData()
    {
        ActionDatas aData = new ActionDatas();

        aData.actionDatas = new ActionData[0];

        File.WriteAllText(Application.persistentDataPath + dataPath, JsonUtility.ToJson(aData));
    }
}
