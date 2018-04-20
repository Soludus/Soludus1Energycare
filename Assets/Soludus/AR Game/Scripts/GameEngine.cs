using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

[Serializable]
public struct PlayerScore
{
    public int score;
    public DateTime updateTimestamp;
}

[Serializable]
public class PlayerData
{
    public string playerName = "";
    public bool firstTimePlaying = true;

    public PlayerScore[] scores = new PlayerScore[12];

    public void Save(string filePath)
    {
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {
            writer.Write(playerName);
            writer.Write(firstTimePlaying);

            writer.Write(scores.Length);
            for (int i = 0; i < scores.Length; ++i)
            {
                writer.Write(scores[i].score);
                writer.Write(scores[i].updateTimestamp.ToBinary());
            }
        }
    }

    public void Load(string filePath)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
        {
            playerName = reader.ReadString();
            firstTimePlaying = reader.ReadBoolean();

            int scoreCount = reader.ReadInt32();

            for (int i = 0; i < scoreCount; ++i)
            {
                scores[i].score = reader.ReadInt32();
                scores[i].updateTimestamp = DateTime.FromBinary(reader.ReadInt64());
            }
        }
    }
}

public class GameEngine : MonoBehaviour
{
    public PlayerData playerData = new PlayerData();

    public float CoolDownInSeconds = 0;

    public DataActionController dAC = null;

    private const string playerDataFileName = "/playerData.dat";

    private void SavePlayerData(string filePath)
    {
        try
        {
            playerData.Save(filePath);
        }
        catch (Exception e)
        {
            playerData = new PlayerData();
            playerData.Save(filePath);
            Debug.LogException(e);
        }
    }

    private void LoadPlayerData(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                playerData.Load(filePath);
            }
            else
            {
                playerData.Save(filePath);
            }
        }
        catch (Exception e)
        {
            playerData = new PlayerData();
            playerData.Save(filePath);
            Debug.LogException(e);
        }
    }

    public void SaveGame()
    {
        SavePlayerData(Application.persistentDataPath + playerDataFileName);
    }

    public void LoadGame()
    {
        LoadPlayerData(Application.persistentDataPath + playerDataFileName);

        if (dAC != null)
        {
            dAC.LoadData();
        }
    }

    public void ResetGame()
    {
        playerData = new PlayerData();
        playerData.Save(Application.persistentDataPath + playerDataFileName);
    }

    public void IncrementScore(int index)
    {
        ++playerData.scores[index].score;
        playerData.scores[index].updateTimestamp = DateTime.Now;
        SaveGame();
    }

    public PlayerScore GetScore(int index)
    {
        return playerData.scores[index];
    }

    public void FirstTimePlayed()
    {
        playerData.firstTimePlaying = false;
        SaveGame();
    }
}
