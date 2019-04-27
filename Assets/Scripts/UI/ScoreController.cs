using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace UI
{
    public class ScoreController : Singleton<ScoreController>
    {
        public Image[] _images;
        [SerializeField] private Sprite _active;
        [SerializeField] private Sprite _inactive;

        public void UpdateScore()
        {
            for (int i = 0; i < GameData.Instance.CurrentScoreOnLevel; i++)
            {
                _images[i].sprite = _active;
            }
        }

        public void ResetScore()
        {
            foreach (var image in _images)
            {
                image.sprite= _inactive;
            }
        }
    }
}
