using Assets.Code.Logic.Craft_System;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Logic.Craft_System
{
    public class OrderHandler : MonoBehaviour
    {
        public event Action<List<GameObject>> OrderCallBack;

        [SerializeField] private int _countAvailableOrder;

        private readonly IListOrder _listOrder = new ListOrder();

        private List<GameObject> _allOrder;
        private List<GameObject> AvailableOrder;
        private int _currentIndexOrder = 0;

        private void OnEnable() { }

        private void OnDisable() { }
        
        private void Start()
        {
            ApplyOrder();
        }

        private void ApplyOrder()
        {
            _listOrder.SetOrderList(_allOrder);    
            GenerateOrders();
            OrderCallBack?.Invoke(AvailableOrder);
        }

        private void GenerateOrders()
        {
            _allOrder = _listOrder.GetOrderList();
            foreach (GameObject Order in _allOrder)
            {
                if (_currentIndexOrder > _countAvailableOrder)
                {
                    _currentIndexOrder++;
                    AvailableOrder.Add(Order);
                    return;
                }
                else
                {
                    _currentIndexOrder = 0;
                }
            }
        }

        private void CompleteOrder(int index)
        {
            // кинуть ивент о завершонном заказк, для обновлеиия списка заказова
            _listOrder.CrearnElematOrderList(index);
            GenerateOrders();
        }

        
    }
}