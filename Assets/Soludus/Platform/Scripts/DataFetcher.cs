using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DataFetcher : MonoBehaviour
{
    private System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
    private Dictionary<string, string> postHeader = new Dictionary<string, string>();

    internal string error = null;

    private void Start()
    {
        postHeader.Add("content-type", "application/json");
        //CreateIndex ();

        //StartCoroutine(JsonQuery("X1", 10));
    }

    private void ParseData(DataSource source, SimpleJSON.JSONNode node)
    {
        if (!node.IsArray)
        {
            var timestamp = node[source.timestampField];
            var value = node[source.valueField];
            if (value != null)
            {
                if (timestamp != null)
                {
                    DateTime tempTime;
                    if (DateTime.TryParse(timestamp.Value, out tempTime) == false)
                    {
                        Debug.LogError("Invalid time format");
                    }
                    source.datalist.Add(new Data
                    {
                        time = tempTime,
                        value = value.AsFloat * source.valueScale
                    });
                    //Debug.Log(source.datalist[source.datalist.Count - 1].time);
                }
                else
                {
                    source.datalist.Add(new Data
                    {
                        time = default(DateTime),
                        value = value.AsFloat * source.valueScale
                    });
                    //Debug.Log(source.datalist[source.datalist.Count - 1].time);
                }

                return;
            }
        }
        for (int i = 0; i < node.Count; i++)
        {
            ParseData(source, node[i]);
        }
    }

    public IEnumerator JsonQuery(DataSource source)
    {
        yield return null;

        WWW www;

        if (!string.IsNullOrEmpty(source.requestBody))
        {
            www = new WWW(source.url, encoding.GetBytes(source.requestBody), postHeader);
        }
        else
        {
            www = new WWW(source.url);
        }

        yield return www;

        error = www.error;
        if (error != null)
        {
            Debug.LogError(error);
        }
        else
        {
            Debug.Log(www.text);

            var parsedNode = SimpleJSON.JSON.Parse(www.text);
            source.datalist = new List<Data>();
            ParseData(source, parsedNode);

            if (source.datalist.Count > 0)
            {
                if (source.useUTCCorrection && source.datalist[0].time != default(DateTime))
                {
                    var difference = DateTime.UtcNow - DateTime.Now;

                    for (int i = 0; i < source.datalist.Count; i++)
                    {
                        var d = source.datalist[i];
                        d.time += difference;
                        source.datalist[i] = d;
                        //Debug.Log(source.datalist[i].time);
                    }
                }
            }
        }
    }
}
