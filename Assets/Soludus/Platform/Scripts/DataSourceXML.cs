using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

public class DataSourceXML : MonoBehaviour
{
    public string dataPath = "/data_source_config.xml";
    public GameObject defaultSources;
    public GameObject dataActions;

    private void OnEnable()
    {
        GetDataSourcesFromXML();
    }

    public void GetDataSourcesFromXML()
    {
        DataSourceSerializableList dataSourcesFromXML;

        XmlSerializer x = new XmlSerializer(typeof(DataSourceSerializableList));

        string path = Application.persistentDataPath + dataPath;

        if (!File.Exists(path))
        {
            Debug.LogWarning(path + " not found, creating default...");
            var defSources = defaultSources.GetComponentsInChildren<DataSource>();

            for (int i = 0; i < defSources.Length; i++)
            {
                Instantiate(defSources[i], transform);
            }

            SaveDataSourcesToXML();
            return;
        }

        using (TextReader reader = new StreamReader(path))
        {
            dataSourcesFromXML = (DataSourceSerializableList)x.Deserialize(reader);
        }

        Dictionary<string, DataSource> dataSourceDict = new Dictionary<string, DataSource>();

        for (int i = 0; i < dataSourcesFromXML.dataSourcesList.Length; i++)
        {
            DataSource ds = new GameObject().AddComponent<DataSource>();
            dataSourcesFromXML.dataSourcesList[i].CopyToDataSource(ds);
            ds.gameObject.name = ds.dataName;
            ds.transform.SetParent(transform);
            dataSourceDict[ds.dataName] = ds;
        }

        var dvl = dataSourcesFromXML.actionValuesList;
        for (int i = 0; i < dvl.Length; i++)
        {
            var da = dataActions.transform.Find(dvl[i].actionName).GetComponent<DataAction>();
            dvl[i].CopyToDataAction(da);
            if (!dataSourceDict.TryGetValue(dvl[i].dataSourceName, out da.dataSource))
            {
                Debug.LogWarning("No matching datasource name found!");
            }
        }

        Debug.Log(path + " loaded.");
    }

    public void SaveDataSourcesToXML()
    {
        DataSource[] dataSources = GetComponentsInChildren<DataSource>();
        DataSourceSerializable[] dataSourceSerializables = new DataSourceSerializable[dataSources.Length];

        for (int i = 0; i < dataSources.Length; i++)
        {
            dataSourceSerializables[i] = new DataSourceSerializable();
            dataSourceSerializables[i].CopyFromDataSource(dataSources[i]);
        }

        DataAction[] actions = dataActions.GetComponentsInChildren<DataAction>();
        ActionValueSerializable[] actionValuesSerializable = new ActionValueSerializable[actions.Length];

        for (int i = 0; i < actions.Length; i++)
        {
            actionValuesSerializable[i] = new ActionValueSerializable();
            actionValuesSerializable[i].CopyFromDataAction(actions[i]);
        }

        DataSourceSerializableList dsList = new DataSourceSerializableList();
        dsList.dataSourcesList = dataSourceSerializables;
        dsList.actionValuesList = actionValuesSerializable;

        XmlSerializer x = new XmlSerializer(typeof(DataSourceSerializableList));

        string path = Application.persistentDataPath + dataPath;

        using (TextWriter writer = new StreamWriter(path))
        {
            x.Serialize(writer, dsList);
        }

        //Debug.Log(path);
    }
}

[XmlRoot("DataConfiguration", Namespace = "list")]
[System.Serializable]
public class DataSourceSerializableList
{
    public DataSourceSerializable[] dataSourcesList;
    public ActionValueSerializable[] actionValuesList;
}

[XmlRoot("DataSource")]
[System.Serializable]
public class DataSourceSerializable
{
    public string dataName;

    public string timestampField = "timestamp";

    public bool useUTCCorrection = false;

    public string valueUnitName;

    public string valueField = "value";

    public float valueScale = 1;

    [Header("JSON")]
    public string url;
    [Multiline(16)]
    public string requestBody;

    public void CopyToDataSource(DataSource _dataSource)
    {
        _dataSource.dataName = dataName;
        _dataSource.timestampField = timestampField;
        _dataSource.useUTCCorrection = useUTCCorrection;
        _dataSource.valueUnitName = valueUnitName;
        _dataSource.valueField = valueField;
        _dataSource.valueScale = valueScale;
        _dataSource.url = url;
        _dataSource.requestBody = XmlUnescape(requestBody);
    }

    public void CopyFromDataSource(DataSource _dataSource)
    {
        dataName = _dataSource.dataName;
        timestampField = _dataSource.timestampField;
        useUTCCorrection = _dataSource.useUTCCorrection;
        valueUnitName = _dataSource.valueUnitName;
        valueField = _dataSource.valueField;
        valueScale = _dataSource.valueScale;
        url = _dataSource.url;
        requestBody = XmlEscape(_dataSource.requestBody);
    }

    public static string XmlEscape(string unescaped)
    {
        XmlDocument doc = new XmlDocument();
        XmlNode node = doc.CreateElement("root");
        node.InnerText = unescaped;
        return node.InnerXml;
    }

    public static string XmlUnescape(string escaped)
    {
        XmlDocument doc = new XmlDocument();
        XmlNode node = doc.CreateElement("root");
        node.InnerXml = escaped;
        return node.InnerText;
    }
}

[XmlRoot("Action")]
[System.Serializable]
public class ActionValueSerializable
{
    public string actionName;
    public string dataSourceName;
    public float actionValue;
    public float actionDurationInSeconds;

    public void CopyToDataAction(DataAction da)
    {
        da.actionValue = actionValue;
        da.actionEffectDurationInSeconds = actionDurationInSeconds;
    }

    public void CopyFromDataAction(DataAction da)
    {
        actionValue = da.actionValue;
        actionDurationInSeconds = da.actionEffectDurationInSeconds;
        actionName = da.actionName;
        dataSourceName = da.dataSource != null ? da.dataSource.dataName : null;
    }
}
