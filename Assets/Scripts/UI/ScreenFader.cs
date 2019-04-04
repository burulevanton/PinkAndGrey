using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class ScreenFader : MonoBehaviour
    {
        public static bool SceneEnd;
        public float fadeSpeed = 1.5f;
        public string nextScene;
        private Image _image;
        private bool _sceneStarting;

        private void Awake()
        {
            _image = GetComponent<Image>();
            _image.enabled = true;
            _sceneStarting = true;
            SceneEnd = false;
        }

        public IEnumerator StartScene()
        {
            _image.color = Color.Lerp(_image.color, Color.clear, fadeSpeed * Time.deltaTime);

            while (_image.color.a > 0.01f)
            {
                _image.color = Color.Lerp(_image.color, Color.clear, fadeSpeed * Time.deltaTime);
                yield return null;
            }
            _image.color = Color.clear;
            _image.enabled = false;
            _sceneStarting = false;
        }

        public IEnumerator EndScene()
        {
            _image.enabled = true;
            while (_image.color.a <= 0.95f)
            {
                _image.color = Color.Lerp(_image.color, Color.black, fadeSpeed * Time.deltaTime);
                yield return null;
            }
            _image.color = Color.black;
            SceneManager.LoadScene(nextScene);
        }
    }
}
