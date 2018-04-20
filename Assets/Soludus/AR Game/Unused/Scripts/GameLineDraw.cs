using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLineDraw : MonoBehaviour {

    public GameEngine GE;
    public LineRenderer lr;
    public LineRenderer lr2;
    public float energyValue;
    public int i;
    public bool showLine;

    public float energyDropValue;
    public float energyDropFromLowest;
    public int pointsBeforeReset;

    public void DrawInstantDropLine(float relativeHeight)
    {
        //bool searchStartPoint = true;
        bool startPointFound = true;
        //i = 7;
        int lineMaxLength = pointsBeforeReset;

        //while (searchStartPoint == true)
        //{
        //    if (System.DateTime.Compare(GE.GetScore(2).updateTimestamp.AddSeconds(300 * i), System.DateTime.Now) > 0)
        //    {
        //        searchStartPoint = false;
        //        startPointFound = true;
        //    }

        //    if (i < lr.positionCount)
        //    {
        //        i++;
        //    }
        //    else
        //    {
        //        searchStartPoint = false;
        //        startPointFound = false;
        //    }
        //}

        if (startPointFound == true)
        {
            int line2Index = 0;
            int index = i - 1;
            lr2.positionCount++;
            lr2.SetPosition(line2Index, lr.GetPosition(index));

            if (energyDropValue < 0)
            {
                energyDropValue = 0;
            }

            int checkForLowest = lineMaxLength;
            float lowestValue = Mathf.Infinity;

            for (int y = index; y > -1; y--)
            {
                if (checkForLowest <= 0)
                {
                    break;
                }

                checkForLowest -= 1;

                if ( lowestValue > lr.GetPosition(y).y)
                {
                    lowestValue = lr.GetPosition(y).y;
                }
            }

            energyDropFromLowest *= relativeHeight;
            lowestValue -= energyDropFromLowest;

            if (lowestValue < 0)
            {
                lowestValue = 0;
            }

            for(int y = index; y > -1; y--)
            {
                lineMaxLength -= 1;
                line2Index++;
                lr2.positionCount++;

                //float yPos = lr.GetPosition(y).y - energyDropAmount;
                //lr2.SetPosition(line2Index, new Vector3(lr.GetPosition(y).x, energyDropValue, lr.GetPosition(y).z));

                lr2.SetPosition(line2Index, new Vector3(lr.GetPosition(y).x, lowestValue, lr.GetPosition(y).z));

                if (lineMaxLength <= 0)
                {
                    lr2.positionCount++;
                    line2Index++;
                    lr2.SetPosition(line2Index, lr.GetPosition(y));
                    break;
                }
            }
        }
    }

    public void DrawReducedEnergyLine(float relativeHeight)
    {
        lr2.positionCount = lr.positionCount;
        GE.LoadGame();

        Debug.Log("relative height: " + relativeHeight);

        for (int i = 0; i < lr.positionCount; i++)
        {
            float yPos = lr.GetPosition(i).y - (energyValue * relativeHeight);
            if (yPos < 0)
            {
                yPos = 0;
            }
            lr2.SetPosition(i, new Vector3(lr.GetPosition(i).x, yPos, lr.GetPosition(i).z));
        }

        if (System.DateTime.Compare(System.DateTime.Now.AddSeconds(-GE.CoolDownInSeconds), GE.GetScore(1).updateTimestamp) < 0)
        {
            for (int i = 0; i < lr.positionCount; i++)
            {
                float yPos = lr.GetPosition(i).y - (energyValue * relativeHeight);
                if (yPos < 0)
                {
                    yPos = 0;
                }
                lr2.SetPosition(i, new Vector3(lr.GetPosition(i).x, yPos, lr.GetPosition(i).z));
            }
        }
    }

    public void ChangeEnergyValue(float nextValue)
    {
        energyValue = nextValue;
    }

    public void IncreaseOrReduceEnergyValue(float amount)
    {
        energyValue += amount;
    }

    public void ChangeEnergyDropAmount(float nextValue)
    {
        energyDropValue = nextValue;
    }

    public void IncreaseOrReduceEnergyDropAmount(float amount)
    {
        energyDropValue += amount;
    }
}
