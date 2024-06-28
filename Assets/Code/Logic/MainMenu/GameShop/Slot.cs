using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Logic.GameShop
{
    public class Slot : MonoBehaviour
    {
        public event Action SellCallBack;

        public bool _isBuy = false;
        public TypeItem TypeSlot { get; private set; }

        private Item _item;
        private string Description => _item.Discription;
        private float _price => _item.Price;
        private ButtonSellSlot _buyItem;
        private int _IDSlot;

        public void Init(Item item, int ID)
        {
            _item = item;
            _IDSlot = ID;
            TypeSlot = _item.TypeItem;

            _buyItem = gameObject.GetComponentInChildren<ButtonSellSlot>();
            _buyItem.Init(_IDSlot);
            FillSlot();
        }

        private void FillSlot()
        {
            GetComponent<Image>().sprite = _item.Sprite;
        }

        public void BuySlot()
        {
            _isBuy = true;
            TypeSlot = TypeItem.Inventory;
            SellCallBack?.Invoke();
          
        }

        public string GetDecription()
        {
            return Description;
        }
    }
}