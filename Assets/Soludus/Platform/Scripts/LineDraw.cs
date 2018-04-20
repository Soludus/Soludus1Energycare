using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDraw : MonoBehaviour
{
    public DataFetcher df;
    public DataActionController dAC;

    public int verticalValueCount = 5;
    public int horizontalTimeCount = 5;

    [Header("Lines")]
    public LineRenderer lr;
    public LineRenderer lr2;
    public GameObject verticalBorderLine;
    public GameObject horizontalBorderLine;

    [Header("Text")]
    public Text valueNameText;
    public Text earliestTimeText;
    public Text newestTimeText;
    public Text lowestValueText;
    public Text highestValueText;
    public Text errorText;
    public GameObject loadingGO;
    public GameObject valueText;
    public GameObject timeText;

    private List<GameObject> verticalGOList = new List<GameObject>();
    private List<GameObject> horizontalGoList = new List<GameObject>();

    public void DrawLines(DataSource source)
    {
        StartCoroutine(DrawRealDataLine(source));
    }

    private IEnumerator DrawRealDataLine(DataSource source)
    {
        // show loading text
        loadingGO.SetActive(true);
        errorText.text = "";

        for (int i = 0; i < verticalGOList.Count; i++)
        {
            Destroy(verticalGOList[i]);
            verticalGOList[i] = null;
        }
        verticalGOList.Clear();

        for (int i = 0; i < horizontalGoList.Count; i++)
        {
            Destroy(horizontalGoList[i]);
            horizontalGoList[i] = null;
        }
        horizontalGoList.Clear();

        // fetch new datas
        yield return StartCoroutine(df.JsonQuery(source));

        if (!string.IsNullOrEmpty(df.error) || source.datalist.Count < 2)
        {
            errorText.text = df.error;
            loadingGO.SetActive(false);
            lowestValueText.text = "";
            highestValueText.text = "";
            lr.positionCount = 0;
            lr2.positionCount = 0;
            earliestTimeText.text = "";
            newestTimeText.text = "";
            valueNameText.text = "";
            timeText.SetActive(false);
            horizontalBorderLine.SetActive(false);
            verticalBorderLine.SetActive(false);
            Debug.LogError("Can't draw line with less than 2 data samples.");
            yield break;
        }

        horizontalBorderLine.SetActive(true);
        verticalBorderLine.SetActive(true);
        timeText.SetActive(true);

        // set count of line positions by datalists data count
        lr.positionCount = source.datalist.Count;

        float valueDistance = 0;
        bool useValueDistance = false;

        DateTime firstPosTime = new DateTime();
        //DateTime lastPosTime = new DateTime();
        double diffInSeconds = 0;

        // if the first and the last datas don't have time, constant distance is used
        if (!DataListDateTimeCheck(source))
        {
            Debug.LogWarning("timestamp not found on all data samples. Using value distance.");
            //Debug.Log(source.datalist.Count);
            valueDistance = 24f / (source.datalist.Count - 1);
            //Debug.Log(valueDistance);
            useValueDistance = true;

            earliestTimeText.text = "";
            newestTimeText.text = "";
            diffInSeconds = (source.datalist.Count) * 60;
        }
        else
        {
            // get first and last time
            firstPosTime = source.datalist[0].time;
            //lastPosTime = source.datalist[source.datalist.Count - 1].time;

            // calculate difference in first and last time for relative point distance
            diffInSeconds = (source.datalist[0].time - source.datalist[source.datalist.Count - 1].time).TotalSeconds;
            //Debug.LogWarning("diff in seconds: " + diffInSeconds);
        }

        // close loading text
        loadingGO.SetActive(false);

        // get the highest value to be set in line renderer's y value and count relative height with it, so that the values always fit to predefined area
        // also check if there is values under 0
        float highestValue = 0;
        float lowestValue = 0;
        for (int i = 0; i < source.datalist.Count; i++)
        {
            if (source.datalist[i].value > highestValue)
            {
                highestValue = source.datalist[i].value;
            }

            if (source.datalist[i].value < lowestValue)
            {
                lowestValue = source.datalist[i].value;
            }
        }

        float relativeHeight = highestValue != 0 ? 12f / (highestValue - lowestValue) : 0;

        // set values to line renderer in reverse order, so that the newest value is on the right side
        for (int i = lr.positionCount - 1; i >= 0; i--)
        {
            float pointPos = 0;

            if (useValueDistance == false)
            {
                // Calculate relative distance to newest time, so the distance of the point sets correctly between newest and oldest time
                double timeDifference = (firstPosTime - source.datalist[i].time).TotalSeconds;
                float relativeDistance = 1 - (float)(timeDifference / diffInSeconds);
                //Debug.Log("points " + i + " relative distance to newest time: " + (1 - relativeDistance));
                pointPos = -12 + 24 * relativeDistance;
            }
            else
            {
                pointPos = -12 + (24 - (valueDistance * i));
            }

            lr.SetPosition(i, new Vector3(pointPos, (source.datalist[i].value - lowestValue) * relativeHeight, -1f));
        }

        // display lowest and highest value
        lowestValueText.text = Math.Round(lowestValue, 2).ToString();
        highestValueText.text = Math.Round(highestValue, 2).ToString();

        if (verticalValueCount > 0)
        {
            float verticalDistance = 12f / (verticalValueCount + 1);

            for (int i = 0; i < verticalValueCount; i++)
            {
                var go = Instantiate(valueText, lr.transform.position, Quaternion.identity, lr.transform);
                go.transform.localPosition = new Vector3(-14, verticalDistance * (i + 1), 0f);
                float shownValue = (highestValue + Mathf.Abs(lowestValue)) * (verticalDistance * (i + 1) / 12f);
                shownValue = lowestValue + shownValue;
                go.GetComponent<Text>().text = Math.Round(shownValue, 2).ToString();
                verticalGOList.Add(go);
            }
        }
        if (horizontalTimeCount > 0)
        {
            float horizontalDistance = 24f / (horizontalTimeCount);
            float diffInMinutes = (float)diffInSeconds / horizontalTimeCount / 60;
            //Debug.Log(horizontalDistance);

            for (int i = 0; i <= horizontalTimeCount; i++)
            {
                ;
                float roundedMinutes = Mathf.Round(diffInMinutes * i);

                var go = Instantiate(valueText, lr.transform.position, Quaternion.identity, lr.transform);
                go.transform.localPosition = new Vector3(-12 + roundedMinutes * horizontalDistance / diffInMinutes, -2f, 0f);
                string shownTime = roundedMinutes.ToString();
                go.GetComponent<Text>().text = shownTime;
                go.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                horizontalGoList.Add(go);
            }
        }

        // draw another line with action values
        if (useValueDistance == false)
        {
            DrawLineWithTimes(relativeHeight, source, diffInSeconds);
        }
        else
        {
            DrawReducedDataLine(relativeHeight, source);
        }

        valueNameText.text = source.valueUnitName;
        if (source.datalist[0].time != DateTime.MinValue)
        {
            earliestTimeText.text = source.datalist[source.datalist.Count - 1].time.ToString();
            newestTimeText.text = source.datalist[0].time.ToString();
        }
    }

    public void DrawReducedDataLine(float relativeHeight, DataSource source)
    {
        lr2.positionCount = lr.positionCount;

        float actionValuesInUse = 0;

        dAC.LoadData();

        for (int i = 0; i < dAC.dataActionList.Count; i++)
        {
            if (System.DateTime.Compare(dAC.dataActionList[i].actionRunTime.AddSeconds(dAC.dataActionList[i].actionEffectDurationInSeconds), DateTime.Now) > 0)
            {
                //Debug.Log("Found data action with valid time!");
                if (dAC.dataActionList[i].dataSource != null && dAC.dataActionList[i].dataSource.dataName == source.dataName)
                {
                    actionValuesInUse += dAC.dataActionList[i].actionValue;
                }
            }
        }

        if (actionValuesInUse != 0)
        {
            // set the reduced line with real data values and reduced value
            for (int i = 0; i < lr.positionCount; i++)
            {
                float yPos = lr.GetPosition(i).y + (actionValuesInUse * relativeHeight);
                if (yPos < 0)
                {
                    yPos = 0;
                }
                lr2.SetPosition(i, new Vector3(lr.GetPosition(i).x, yPos, lr.GetPosition(i).z));
            }
        }
        else
        {
            lr2.positionCount = 0;
        }
    }

    public void DrawLineWithTimes(float relativeHeight, DataSource source, double differenceInSeconds)
    {
        lr2.positionCount = 0;

        // line where action's start and end time is checked, so the line has an beginning and ending

        dAC.LoadData();

        List<float> ActionValues = new List<float>();
        List<DateTime> ActionStartTimes = new List<DateTime>();
        List<DateTime> ActionEndTimes = new List<DateTime>();

        // get values, start times and end times to arrays
        for (int i = 0; i < dAC.dataActionList.Count; i++)
        {
            if (dAC.dataActionList[i].dataSource != null && dAC.dataActionList[i].dataSource.dataName == source.dataName)
            {
                ActionValues.Add(dAC.dataActionList[i].actionValue);
                ActionStartTimes.Add(dAC.dataActionList[i].actionRunTime);
                ActionEndTimes.Add(dAC.dataActionList[i].actionRunTime.AddSeconds(dAC.dataActionList[i].actionEffectDurationInSeconds));
            }
        }

        DateTime currentlyOldestTime = DateTime.Now;
        int currentlyOldestIndex = 0;
        bool indexIsForStartTimes = true;
        float currentValueToAdd = 0;
        List<int> startTimeIndexes;
        List<int> endTimeIndexes;

        DateTime firstPosTime = source.datalist[0].time;
        double timeDifference;
        float relativeDistance;
        float pointPos;
        double distanceBetweenPoints;
        double distanceToLastPoint;
        float relativeDistanceBetweenPoints;
        float relativeHeightBetweenValues;

        for (int i = 0; i < source.datalist.Count; i++)
        {
            // check if there is start times before first real data time, and draw the line to start from the right height
            if (i == 0)
            {
                for (int y = 0; y < ActionStartTimes.Count; y++)
                {
                    if (System.DateTime.Compare(ActionStartTimes[y], source.datalist[source.datalist.Count - 1].time) < 0 && System.DateTime.Compare(ActionEndTimes[y], source.datalist[source.datalist.Count - 1].time) > 0)
                    {
                        currentValueToAdd += ActionValues[y];
                    }
                }

                if (currentValueToAdd != 0)
                {
                    lr2.positionCount++;
                    lr2.SetPosition(lr2.positionCount - 1, new Vector3(lr.GetPosition(lr.positionCount - 1).x, lr.GetPosition(lr.positionCount - 1).y + (currentValueToAdd * relativeHeight), lr.GetPosition(lr.positionCount - 1).z));
                }

            }
            // check all action start times and end times between first and last data time, and change height if there is any. If none, draw to next real data time
            else
            {
                startTimeIndexes = new List<int>();
                endTimeIndexes = new List<int>();

                // get each index which is earlier than next real data time and later than last real data time
                for (int sIndex = 0; sIndex < ActionStartTimes.Count; sIndex++)
                {
                    if ((System.DateTime.Compare(ActionStartTimes[sIndex], source.datalist[source.datalist.Count - i - 1].time) < 0) && (System.DateTime.Compare(ActionStartTimes[sIndex], source.datalist[source.datalist.Count - i].time) > 0))
                    {
                        //Debug.Log("TIME FOUND!");
                        startTimeIndexes.Add(sIndex);
                    }
                    if ((System.DateTime.Compare(ActionEndTimes[sIndex], source.datalist[source.datalist.Count - i - 1].time) < 0) && (System.DateTime.Compare(ActionEndTimes[sIndex], source.datalist[source.datalist.Count - i].time) > 0))
                    {
                        endTimeIndexes.Add(sIndex);
                    }
                }

                // draw points as long as they last
                while ((startTimeIndexes.Count > 0 || endTimeIndexes.Count > 0))
                {

                    currentlyOldestTime = DateTime.Now;

                    for (int z = 0; z < startTimeIndexes.Count; z++)
                    {
                        if (System.DateTime.Compare(ActionStartTimes[startTimeIndexes[z]], currentlyOldestTime) < 0)
                        {
                            currentlyOldestTime = ActionStartTimes[startTimeIndexes[z]];
                            currentlyOldestIndex = z;
                            indexIsForStartTimes = true;
                        }
                    }
                    for (int k = 0; k < endTimeIndexes.Count; k++)
                    {
                        if (System.DateTime.Compare(ActionEndTimes[endTimeIndexes[k]], currentlyOldestTime) < 0)
                        {
                            currentlyOldestTime = ActionEndTimes[endTimeIndexes[k]];
                            currentlyOldestIndex = k;
                            indexIsForStartTimes = false;
                        }
                    }

                    if (indexIsForStartTimes)
                    {
                        // difference between newest real time and oldest action time
                        timeDifference = (firstPosTime - currentlyOldestTime).TotalSeconds;

                        // relative distance to max difference
                        relativeDistance = 1 - (float)(timeDifference / differenceInSeconds);

                        // position on line
                        pointPos = -12 + 24 * relativeDistance;

                        // distance between next and earlier points
                        distanceBetweenPoints = (source.datalist[source.datalist.Count - i - 1].time - source.datalist[source.datalist.Count - i].time).TotalSeconds;

                        // distance of the action time to earlier point
                        distanceToLastPoint = (currentlyOldestTime - source.datalist[source.datalist.Count - i].time).TotalSeconds;

                        // how big change action time is from earlier point relative to the complete difference of the next and earlier point
                        relativeDistanceBetweenPoints = (float)(distanceToLastPoint / distanceBetweenPoints);

                        // the value change of the real datas is linear, so taking only part of the change is the current height of the line
                        relativeHeightBetweenValues = (source.datalist[source.datalist.Count - i - 1].value - source.datalist[source.datalist.Count - i].value) * relativeDistanceBetweenPoints * relativeHeight;

                        // create new position, from which the line begins increasing or decreasing current action value

                        lr2.positionCount++;
                        lr2.SetPosition(lr2.positionCount - 1, new Vector3(pointPos, lr.GetPosition(lr.positionCount - i).y + relativeHeightBetweenValues + (currentValueToAdd * relativeHeight), -1));

                        if (lr2.GetPosition(lr2.positionCount - 1).y < 0)
                        {
                            lr2.SetPosition(lr2.positionCount - 1, new Vector3(pointPos, 0f, -1));
                        }

                        // add the action change to current action value
                        currentValueToAdd += ActionValues[startTimeIndexes[currentlyOldestIndex]];

                        // create new position, where the new current action value is added (creates the drop in the line)
                        //lr2.positionCount++;
                        //lr2.SetPosition(lr2.positionCount - 1, new Vector3(pointPos, lr.GetPosition(lr.positionCount - i).y + relativeHeightBetweenValues + (currentValueToAdd * relativeHeight), -1));

                        // remove index so it wont be checked again on next loop
                        startTimeIndexes.RemoveAt(currentlyOldestIndex);

                    }
                    else
                    {
                        timeDifference = (firstPosTime - currentlyOldestTime).TotalSeconds;

                        relativeDistance = 1 - (float)(timeDifference / differenceInSeconds);

                        pointPos = -12 + 24 * relativeDistance;

                        distanceBetweenPoints = (source.datalist[source.datalist.Count - i - 1].time - source.datalist[source.datalist.Count - i].time).TotalSeconds;

                        distanceToLastPoint = (currentlyOldestTime - source.datalist[source.datalist.Count - i].time).TotalSeconds;

                        relativeDistanceBetweenPoints = (float)(distanceToLastPoint / distanceBetweenPoints);

                        relativeHeightBetweenValues = (source.datalist[source.datalist.Count - i - 1].value - source.datalist[source.datalist.Count - i].value) * relativeDistanceBetweenPoints * relativeHeight;

                        lr2.positionCount++;
                        lr2.SetPosition(lr2.positionCount - 1, new Vector3(pointPos, lr.GetPosition(lr.positionCount - i).y + relativeHeightBetweenValues + (currentValueToAdd * relativeHeight), -1));

                        if (lr2.GetPosition(lr2.positionCount - 1).y < 0)
                        {
                            lr2.SetPosition(lr2.positionCount - 1, new Vector3(pointPos, 0f, -1));
                        }

                        //Debug.Log(lr2.GetPosition(lr2.positionCount - 1).y);

                        currentValueToAdd -= ActionValues[endTimeIndexes[currentlyOldestIndex]];

                        //lr2.positionCount++;
                        //lr2.SetPosition(lr2.positionCount - 1, new Vector3(pointPos, lr.GetPosition(lr.positionCount - i).y + relativeHeightBetweenValues + (currentValueToAdd * relativeHeight), -1));

                        endTimeIndexes.RemoveAt(currentlyOldestIndex);
                    }
                }
            }

            // draw to next point
            lr2.positionCount++;
            lr2.SetPosition(lr2.positionCount - 1, new Vector3(lr.GetPosition(lr.positionCount - 1 - i).x, lr.GetPosition(lr.positionCount - 1 - i).y + (currentValueToAdd * relativeHeight), lr.GetPosition(lr.positionCount - 1 - i).z));
            if (lr2.GetPosition(lr2.positionCount - 1).y < 0)
            {
                lr2.SetPosition(lr2.positionCount - 1, new Vector3(lr.GetPosition(lr.positionCount - 1 - i).x, 0, lr.GetPosition(lr.positionCount - 1 - i).z));
            }
        }
    }

    private bool DataListDateTimeCheck(DataSource source)
    {
        for (int i = 0; i < source.datalist.Count; i++)
        {
            if (source.datalist[i].time == default(DateTime))
            {
                return false;
            }
        }

        return true;
    }

}
