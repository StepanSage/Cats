using Objects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.UI
{
    public class PauseView : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        private Sprite _pause;
        private Sprite _play;
          
        public void init()
        {
            _pause = Factory.Instance.GetIcon("Pause");
            _play = Factory.Instance.GetIcon("Play");
        }

        public void SetStatePause(bool state)
        {
            if(state)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }

        private void Pause()
        {
            _icon.sprite = _pause;
        }

        private void Play()
        {
            _icon.sprite = _play;
        }
    }
}