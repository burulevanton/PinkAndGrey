using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class GameUIController:MonoBehaviour
    {
        public ScreenFader ScreenFader;
        public void RestartScene()
        {
            GameController.Instance.CurrentLevel++;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void StartScene()
        {
            StartCoroutine(ScreenFader.StartScene());
        }
    }
}