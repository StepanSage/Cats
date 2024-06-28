using System;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Logic.GameShop 
{
    public class ButtonSellSlot : MonoBehaviour
    {
        public event Action<int> BuyCallBack;
        public event Action<int> ConfirmCallBack;
        
        private Button _buy;
        private int _iDButton;
        private ConfirmPurchase _confirmPurchase;

    
        public void Init(int ID)
        {
            _buy = GetComponent<Button>();
            _iDButton= ID;
            _confirmPurchase = FindObjectOfType<ConfirmPurchase>();
            _buy.onClick.AddListener(ConfirmSelection);
            _confirmPurchase.SellCallBack += Buy;
           
        } 

        public void ConfirmSelection()
        {
            _confirmPurchase.Show();
            ConfirmCallBack?.Invoke(_iDButton);
        }

        public void Buy(int id)
        {    
             BuyCallBack?.Invoke(id);
        }
    }
}

