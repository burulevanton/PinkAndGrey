using System.Collections.Generic;
using UI;
using UnityEngine;

public class GameData:Singleton<GameData>
{
    public int CurrentLevel { get; set; }
    
    public int MaxLevel { get; set; }

    public int MaxAmountOfLevels { get; } = 3;
    public int CurrentScoreOnLevel { get; set; }

    public int TutorialStage { get; private set; } = 0;

    public bool TutorialPass => TutorialStage == 2;

    public bool LogoPassed { get; set; } = false;

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

        TutorialStage = PlayerPrefs.GetInt("TutorialStage", 0);
    }

    public List<int> GetScoreOfLevels()
    {
        var list = new List<int>();
        for (var i = 0; i < MaxLevel; i++)
        {
            var score = PlayerPrefs.GetInt(string.Format("Level"+(i+1)));
            list.Add(score);
        }

        return list;
    }

    public void SetScoreOfLevel()
    {
        if (PlayerPrefs.HasKey(string.Format("Level"+CurrentLevel)))
        {
            var prevResult = PlayerPrefs.GetInt(string.Format("Level" + CurrentLevel));
            if (prevResult<CurrentScoreOnLevel)
               PlayerPrefs.SetInt(string.Format("Level"+CurrentLevel),CurrentScoreOnLevel);
        }
        else
            PlayerPrefs.SetInt(string.Format("Level"+CurrentLevel),CurrentScoreOnLevel);
        PlayerPrefs.Save();
    }

    public void UpdateMaxLevel()
    {
        if (MaxLevel < CurrentLevel)
        {
            MaxLevel = CurrentLevel;
            PlayerPrefs.SetInt("MaxLevel", MaxLevel);
            PlayerPrefs.Save();
        }
    }

    public void PassTutorial(int stage)
    {
        TutorialStage = stage;
        PlayerPrefs.SetInt("TutorialStage", stage);
        PlayerPrefs.Save();
    }
}