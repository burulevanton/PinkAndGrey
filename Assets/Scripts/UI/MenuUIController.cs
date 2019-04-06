using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MenuUIController : MonoBehaviour
    {
        public ScreenFader ScreenFader;
        public Button[] LevelButtons;

        public void StartLevel()
        {
            StartCoroutine(ScreenFader.FadeScene());
        }

        private void Start()
        {
            for (int i = 0; i < LevelButtons.Length; i++)
            {
                var i1 = i+1;
                LevelButtons[i].onClick.AddListener((() => ChangeLevel(i1)));
            }

            StartCoroutine(ScreenFader.SceneAppearance());
        }

        private void ChangeLevel(int num)
        {
            GameData.Instance.CurrentLevel = num;
            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            yield return StartCoroutine(ScreenFader.FadeScene());
            SceneManager.LoadScene("Game");
        }
    }
}
