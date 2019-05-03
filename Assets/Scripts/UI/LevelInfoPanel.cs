using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelInfoPanel : MonoBehaviour
    {
        private Text _text;

        void Awake()
        {
            _text = GetComponentInChildren<Text>();
        }

        private void OnEnable()
        {
            _text.text = "Level " + GameData.Instance.CurrentLevel;
        }

        private void OnAnimationEnd()
        {
            gameObject.SetActive(false);
        }
    }
}
