using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataSourceController : MonoBehaviour
{
    public List<DataSource> dataSourceList;
    public Transform datasourceHolder;
    public GameObject sourceList;
    public GameObject dataButton;
    public LineDraw ld;

    private void OnEnable()
    {
        GetDataSources();
        if (dataSourceList.Count > 0)
            ld.DrawLines(dataSourceList[0]);
    }

    private void OnDisable()
    {
        for (int i = sourceList.transform.childCount - 1; i > 0; i--)
        {
            Destroy(sourceList.transform.GetChild(i).gameObject);
        }
    }

    private void GetDataSources()
    {
        dataSourceList = new List<DataSource>();
        datasourceHolder.GetComponentsInChildren(dataSourceList);

        foreach (var i in dataSourceList)
        {
            GameObject createdButton = Instantiate(dataButton, sourceList.transform);
            createdButton.GetComponentInChildren<Text>().text = i.dataName;
            createdButton.GetComponent<Button>().onClick.AddListener(() => ld.DrawLines(i));
            createdButton.SetActive(true);
        }
    }
}
