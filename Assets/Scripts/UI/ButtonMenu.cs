using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ButtonMenu : MonoBehaviour
    {
        private Text _text;

        private void Awake()
        {
            _text = GetComponentInChildren<Text>();
        }
        
        public void ClickText()
        {
            _text.rectTransform.position= new Vector2(_text.rectTransform.position.x, _text.rectTransform.position.y-15);
        }
        
        public void UnClickText()
        {
            _text.rectTransform.position= new Vector2(_text.rectTransform.position.x, _text.rectTransform.position.y+15);
        }
    }
}
