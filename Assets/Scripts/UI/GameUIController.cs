using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameUIController:Singleton<GameUIController>
    {
        public ScreenFader ScreenFader;
        public GameMenu GameMenu;
        public GameMenu DeathMenu;
        public GameObject LevelInfoPanel;
        [SerializeField] private ScoreController[] _scoreControllers;
        [SerializeField] private Text _levelPassedText;
        public GameMenu LevelPassedMenu;
        
        public void RestartScene()
        {
            if (DeathMenu.gameObject.activeSelf)
            {
                DeathMenu.Close((() => StartCoroutine(GameController.Instance.StartLevel())));
            }
            else
            {
                StartCoroutine(GameController.Instance.StartLevel());
            }
        }

        public void BackToMainMenu()
        {
            //todo screenfader
            if (GameMenu.gameObject.activeSelf)
            {
                GameMenu.Close((() => SceneManager.LoadScene("Menu")));
            }
            else if (DeathMenu.gameObject.activeSelf)
            {
                DeathMenu.Close((() => SceneManager.LoadScene("Menu")));
            }
            else if (LevelPassedMenu.gameObject.activeSelf)
            {
                LevelPassedMenu.Close(() => SceneManager.LoadScene("Menu"));
            }
        }

        public IEnumerator StartScene()
        {
            DeathMenu.gameObject.SetActive(false);
            GameMenu.gameObject.SetActive(false);
            UpdateScore();
            yield return StartCoroutine(ScreenFader.SceneAppearance());
            LevelInfoPanel.SetActive(true);
            yield return new WaitUntil(() => LevelInfoPanel.activeSelf == false);
        }

        private void Awake()
        {
            GameController.Instance.GameUiController = this;
        }

        public void Deserialize()
        {
            StartCoroutine(LevelController.Instance.Deserialize());
        }

        public void GameMenuOpen()
        {
            GameMenu.gameObject.SetActive(true);
            GameController.Instance.Pause();
        }

        public void UnpauseGame()
        {
            GameMenu.Close(()=>GameController.Instance.UnPause());
        }

        public void DeathMenuOpen()
        {
            DeathMenu.gameObject.SetActive(true);
        }

        public void UpdateScore()
        {
            foreach (var scoreController in _scoreControllers)
            {
                scoreController.UpdateScore();
            }
        }

        public void LevelPassed()
        {
            GameController.Instance.Pause();
            GameData.Instance.SetScoreOfLevel();
            _levelPassedText.text = "Level " + GameData.Instance.CurrentLevel;
            LevelPassedMenu.gameObject.SetActive(true);
        }

        public void GoToNextLevel()
        {
            LevelPassedMenu.Close(()=> StartCoroutine(GameController.Instance.LevelPassed()));
        }
    }
}