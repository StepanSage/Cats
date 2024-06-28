using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Logic.GameShop
{
    public class ButtonSortItems : MonoBehaviour
    {
        public event Action<TypeItem> OnClick;

        [SerializeField] private TypeItem _typeSort;
        

        private Button _button;

        private void Start()
        {
            _button= GetComponent<Button>();
            _button.onClick.AddListener(SortCallBack);
        }

        public void SortCallBack()
        {
            OnClick?.Invoke(_typeSort);
        }

        public void IsAction(Sprite  _Select)
        {
            GetComponent<Image>().sprite = _Select;
        }

        public TypeItem GetTypeItem()
        {
            return _typeSort;
        }

    }
}