using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Logic.GameShop
{
    public class ConfirmPurchase : AbstractWindow
    {
        public event Action<int> SellCallBack;
        [SerializeField] private Button _buttonYes;
        
       
        public bool isButtonPressed = false;

        private ButtonSellSlot[] _buttonSellSlots;
        [SerializeField] private int IDSlot;

        public void Start()
        {
            _buttonYes.onClick.AddListener(Confirm); 
            Hide();
        }
        public void OnEnable()
        {
            _buttonSellSlots = FindObjectsOfType<ButtonSellSlot>();
            foreach (var item in _buttonSellSlots)
            {
                item.ConfirmCallBack += GetID;

            }
        }

        private void OnDisable()
        {
            foreach (var item in _buttonSellSlots)
            {
                item.ConfirmCallBack -= GetID;

            }
        }

        private void GetID(int id)
        {
            IDSlot = id;
        }

        private void Confirm()
        {
            SellCallBack?.Invoke(IDSlot);
            Hide();
        }


        
       



    }
}