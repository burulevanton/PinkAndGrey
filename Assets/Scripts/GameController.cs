using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enum;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
  
  //public static GameController instance;
  public PlayerController PlayerController;
  public GameUIController GameUiController;

  public bool IsPaused { get; private set; } = false;

  private void Start()
  {
    StartCoroutine(StartLevel());
  }

  public IEnumerator StartLevel()
  {
    Pause();
    //yield return StartCoroutine(GameUiController.ScreenFader.FadeScene());
    yield return StartCoroutine(LevelController.Instance.Deserialize());
    GameData.Instance.CurrentScoreOnLevel = 0;
    yield return StartCoroutine(GameUiController.StartScene());
    UnPause();
  }

  public void PlayerDeath()
  {
    GameUiController.DeathMenuOpen();
  }

  public IEnumerator LevelPassed()
  {
    GameData.Instance.SetScoreOfLevel();
    if (GameData.Instance.CurrentLevel == GameData.Instance.MaxAmountOfLevels)
    {
      yield return StartCoroutine(GameUiController.ScreenFader.FadeScene());
      SceneManager.LoadScene("Menu");
    }
    GameData.Instance.CurrentLevel++;
    GameData.Instance.UpdateMaxLevel();
    StartCoroutine(StartLevel());
  }

  // Update is called once per frame

  public void Pause()
  {
    IsPaused = true;
  }

  public void UnPause()
  {
    IsPaused = false;
  }
}
