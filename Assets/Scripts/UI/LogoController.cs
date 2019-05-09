using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LogoController : MonoBehaviour
    {
        public Button button;
        public Action action;
        

        private void OnAnimationLogoEnd()
        {
            if (action != null)
            {
                button.onClick.AddListener(()=> action());
            }
            GameData.Instance.LogoPassed = true;
        }
    }
}