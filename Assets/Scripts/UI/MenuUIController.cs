using UnityEngine;

namespace UI
{
    public class MenuUIController : MonoBehaviour
    {
        public ScreenFader ScreenFader;
        public void StartLevel()
        {
            StartCoroutine(ScreenFader.EndScene());
        }

        private void Start()
        {
            StartCoroutine(ScreenFader.StartScene());
        }
    }
}
