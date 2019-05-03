using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Internal.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameUIController:MonoBehaviour
    {
        public ScreenFader ScreenFader;
        public GameMenu GameMenu;
        public GameMenu DeathMenu;
        public GameObject LevelInfoPanel;
        
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
        }

        public IEnumerator StartScene()
        {
            ScoreController.Instance.ResetScore();
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
    }
}