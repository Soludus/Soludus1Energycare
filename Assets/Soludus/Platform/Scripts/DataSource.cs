using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSource : MonoBehaviour
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

    public List<Data> datalist;
}
