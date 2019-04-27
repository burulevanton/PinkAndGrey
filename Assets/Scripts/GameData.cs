using System.Collections.Generic;
using UI;
using UnityEngine;

public class GameData:Singleton<GameData>
{
    public int CurrentLevel { get; set; }
    
    public int MaxLevel { get; set; }
    public int CurrentScoreOnLevel { get; set; }

    private void Awake()
    {
        if (PlayerPrefs.HasKey("MaxLevel"))
        {
            MaxLevel = PlayerPrefs.GetInt("MaxLevel");
            CurrentLevel = MaxLevel;
        }
        else
        {
            MaxLevel = 1;
            PlayerPrefs.SetInt("MaxLevel", MaxLevel);
            PlayerPrefs.Save();
        }
    }

    public List<int> GetScoreOfLevels()
    {
        var list = new List<int>();
        for (var i = 0; i < MaxLevel; i++)
        {
            var score = PlayerPrefs.GetInt(string.Format("Level" + 3));
            list.Add(score);
        }

        return list;
    }

    public void SetScoreOfLevel()
    {
        PlayerPrefs.SetInt(string.Format("Level"+CurrentLevel),CurrentScoreOnLevel);
        PlayerPrefs.Save();
    }
}