using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class GameUIController:MonoBehaviour
    {
        public ScreenFader ScreenFader;
        public void RestartScene()
        {
            GameData.Instance.CurrentLevel++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public IEnumerator StartScene()
        {
            yield return StartCoroutine(ScreenFader.SceneAppearance());
        }

        private void Awake()
        {
            GameController.Instance.GameUiController = this;
        }
    }
}