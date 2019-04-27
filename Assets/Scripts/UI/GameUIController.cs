using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Internal.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameUIController:MonoBehaviour
    {
        public ScreenFader ScreenFader;
        public GameObject GameMenu;
        public GameObject DeathMenu;
        public void RestartScene()
        {
            DeathMenu.SetActive(false);
            StartCoroutine(GameController.Instance.StartLevel());
        }

        public void BackToMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public IEnumerator StartScene()
        {
            yield return StartCoroutine(ScreenFader.SceneAppearance());
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
            GameMenu.SetActive(true);
            GameController.Instance.Pause();
        }

        public void UnpauseGame()
        {
            GameMenu.SetActive(false);
            GameController.Instance.UnPause();
        }

        public void DeathMenuOpen()
        {
            DeathMenu.SetActive(true);
        }
    }
}