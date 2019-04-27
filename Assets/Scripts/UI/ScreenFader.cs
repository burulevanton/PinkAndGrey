using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class ScreenFader : MonoBehaviour
    {
        public float fadeSpeed = 1.5f;
        [SerializeField] private Image _image;


        public IEnumerator SceneAppearance()
        {
            _image.color = Color.Lerp(_image.color, Color.clear, fadeSpeed * Time.deltaTime);

            while (_image.color.a > 0.01f)
            {
                _image.color = Color.Lerp(_image.color, Color.clear, fadeSpeed * Time.deltaTime);
                yield return null;
            }
            _image.color = Color.clear;
            _image.enabled = false;
        }
        public IEnumerator FadeScene()
        {
            _image.enabled = true;
            while (_image.color.a <= 0.95f)
            {
                _image.color = Color.Lerp(_image.color, Color.black, fadeSpeed * Time.deltaTime);
                yield return null;
            }
            _image.color = Color.black;
        }
    }
}
