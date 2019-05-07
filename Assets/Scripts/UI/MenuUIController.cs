using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MenuUIController : MonoBehaviour
    {
        public ScreenFader ScreenFader;
        public MenuButtonController[] LevelButtons;

        private AsyncOperation GameLoad;

        public void StartLevel()
        {
            StartCoroutine(ScreenFader.FadeScene());
        }

        private void Start()
        {
            var list = GameData.Instance.GetScoreOfLevels();
            for (int i = 0; i < LevelButtons.Length; i++)
            {
                if (i<list.Count)
                {
                    LevelButtons[i].SetScoreButton(i+1, list[i]);
                    var i1 = i+1;
                    LevelButtons[i].Button.onClick.AddListener((() => ChangeLevel(i1)));
                }
                else
                    LevelButtons[i].LockButton();
            }

            StartCoroutine(ScreenFader.SceneAppearance());
            GameLoad = SceneManager.LoadSceneAsync("Game");
            GameLoad.allowSceneActivation = false;
        }
        

        private void ChangeLevel(int num)
        {
            GameData.Instance.CurrentLevel = num;
            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            yield return StartCoroutine(ScreenFader.FadeScene());
//            SceneManager.LoadScene("Game");
            GameLoad.allowSceneActivation = true;
        }
    }
}
