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
        private AsyncOperation SceneLoad;

        public LogoController LogoController;
        public GameObject Logo;

        public GameObject LoadingPanel;
        public Slider Slider;

        public void StartLevel()
        {
            StartCoroutine(ScreenFader.FadeScene());
        }

        private IEnumerator LoadLevelAsync(string sceneName)
        {
            SceneLoad = SceneManager.LoadSceneAsync(sceneName);
            LoadingPanel.gameObject.SetActive(true);
            while (!SceneLoad.isDone)
            {
                float progress = Mathf.Clamp01(SceneLoad.progress / 0.9f);
                Slider.value = progress;
                yield return null;
            }
        }

        private void Start()
        {
            StartCoroutine(GameData.Instance.LoadAssets());
            /*SceneLoad = SceneManager.LoadSceneAsync("Game");
            SceneLoad.allowSceneActivation = false;*/
            if (!GameData.Instance.LogoPassed)
            {
                Logo.SetActive(true);
                if (GameData.Instance.TutorialStage == 0)
                {
                    LogoController.action = () =>
                    {
                        StartCoroutine(LoadLevelAsync("Tutorial"));
                        Logo.gameObject.SetActive(false);
                    };
                    return;
                }

                LogoController.action = () => Logo.gameObject.SetActive(false);
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
//            if (GameData.Instance.TutorialPass)
//            {
//                StartCoroutine(ScreenFader.SceneAppearance());
//                GameLoad = SceneManager.LoadSceneAsync("Game");
//                GameLoad.allowSceneActivation = false;
//            }
        }

        private void TutorialPass()
        {
            GameData.Instance.PassTutorial(2);
            _tutorialHint.SetActive(false);
            //StartCoroutine(ScreenFader.SceneAppearance());
//            StartCoroutine(LoadLevelAsync("Game"));
        }
        private void ChangeLevel(int num)
        {
            GameData.Instance.CurrentLevel = num;
//            StartCoroutine(StartGame());
//            SceneLoad.allowSceneActivation = true;
            StartCoroutine(LoadLevelAsync("Game"));
        }

//        private IEnumerator StartGame()
//        {
//            //yield return StartCoroutine(ScreenFader.FadeScene());
////            SceneManager.LoadScene("Game");
//            GameLoad.allowSceneActivation = true;
//            yield return null;
//        }
    }
}
