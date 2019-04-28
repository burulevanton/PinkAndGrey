using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuButtonController : MonoBehaviour
    {
        [SerializeField] private Image _lockImage;
        public Button Button;
        [SerializeField] private Image[] _collectables;
        [SerializeField] private Sprite[] _spritesOfCollectable;

        public void LockButton()
        {
            _lockImage.gameObject.SetActive(true);
            Button.gameObject.SetActive(false);
        }

        public void SetScoreButton(int level, int score)
        {
            _lockImage.gameObject.SetActive(false);
            Button.gameObject.SetActive(true);
            Button.GetComponentInChildren<Text>().text = level.ToString();
            if (_collectables.Count(x => x.sprite == _spritesOfCollectable[1]) > score)
                return;
            for (var i = 0; i < score; i++)
            {
                _collectables[i].sprite = _spritesOfCollectable[1];
            }
        }
    }
}
