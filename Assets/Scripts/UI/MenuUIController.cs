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

        public GameObject _tutorialHint;
        private AsyncOperation GameLoad;

        public void StartLevel()
        {
            StartCoroutine(ScreenFader.FadeScene());
        }

        private void Start()
        {
            if (GameData.Instance.TutorialStage == 0)
            {
                SceneManager.LoadScene("Tutorial");
            }
            if (GameData.Instance.TutorialStage == 1)
            {
                _tutorialHint.SetActive(true);
                LevelButtons[0].Button.onClick.AddListener(TutorialPass);
            }
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
            if (GameData.Instance.TutorialPass)
            {
                StartCoroutine(ScreenFader.SceneAppearance());
                GameLoad = SceneManager.LoadSceneAsync("Game");
                GameLoad.allowSceneActivation = false;
            }
        }

        private void TutorialPass()
        {
            GameData.Instance.PassTutorial(2);
            _tutorialHint.SetActive(false);
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
            //yield return StartCoroutine(ScreenFader.FadeScene());
//            SceneManager.LoadScene("Game");
            GameLoad.allowSceneActivation = true;
            yield return null;
        }
    }
}
