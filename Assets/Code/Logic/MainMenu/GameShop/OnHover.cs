using Code.Logic.GameShop;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Code.Logic.GameShop 
{ 
    public class OnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<bool, Vector3, string> IsHoveringCallBack ;
        private Slot _slot;

        private void Start()
        {
            _slot = GetComponent<Slot>();
            _slot.SellCallBack += Deactivate;
        }

        private void Deactivate()
        {
            IsHoveringCallBack?.Invoke(false, Vector3.zero, null);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
       
            IsHoveringCallBack?.Invoke(true, transform.position, _slot.GetDecription());   
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHoveringCallBack?.Invoke(false, Vector3.zero, null);
        }

   
    }
}

