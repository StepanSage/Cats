using System.Collections.Generic;
using UnityEngine;

namespace Code.Logic.GameShop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private Item[] _items;
        [SerializeField] private Slot _slotPrefab;
        [SerializeField] private Sprite Sellect;
        [SerializeField] private Sprite Normal;

        private TypeItem _typeSortButton = TypeItem.All;
        private readonly List<Slot> _allSlots = new();
        private readonly List<ButtonSellSlot> _buyItem = new();
        private ButtonSortItems[] _buttonSort;
        private int _currentIDSlot;

        private void Awake()
        {
            CreatSlots();
            _buttonSort = FindObjectsOfType<ButtonSortItems>();
        }

        private void OnEnable()
        {
            foreach (var buttonSort in _buttonSort)
            {
                buttonSort.OnClick += ChangeStateSort;
            }

            foreach (var buyitem in _buyItem)
            {
                buyitem.BuyCallBack += SellSlot;
            }
        }

        private void OnDisable()
        {
            foreach (var buttonSort in _buttonSort)
            {
                buttonSort.OnClick -= ChangeStateSort;
            }
            foreach (var buyitem in _buyItem)
            {
                buyitem.BuyCallBack -= SellSlot;
            }

        }

        public void CreatSlots()
        {
            for (int i = 0; i < _items.Length; i++)
            {
                var prefab = Instantiate(_slotPrefab, transform);
                prefab.Init(_items[i],i);
                _allSlots.Add(prefab);
                _buyItem.Add(prefab.GetComponentInChildren<ButtonSellSlot>());
            }
        }

        public void SellSlot(int idSlot)
        {
            _allSlots[idSlot].BuySlot();
            _currentIDSlot = idSlot;
            ResortSlots(idSlot+1, idSlot);
        }

        public void ChangeStateSort(TypeItem typeActionButtonSort)
        {
            ChangeSpriteSelectedButtonSort(typeActionButtonSort);
            _typeSortButton = typeActionButtonSort;
            ResortSlots(_allSlots.Count);
        }

        private void ResortSlots(int maxValue, int minValue = 0)
        {
            for(int i = minValue; i < maxValue; i++)
            {
                if (_typeSortButton == TypeItem.All && _allSlots[i].TypeSlot != TypeItem.Inventory)
                {
                    _allSlots[i].gameObject.SetActive(true);
                }
                else if (_typeSortButton == TypeItem.Inventory && _allSlots[i].TypeSlot == _typeSortButton)
                {
                    _allSlots[i].gameObject.SetActive(true);
                }
                else if (_typeSortButton == _allSlots[i].TypeSlot)
                {
                    _allSlots[i].gameObject.SetActive(true);
                }
                else
                {
                    _allSlots[i].gameObject.SetActive(false);
                }
            }  
        }

        public void ChangeSpriteSelectedButtonSort(TypeItem sortBurron)
        {
            foreach (var item in _buttonSort)
            {
                if(item.GetTypeItem() == sortBurron)
                {
                    item.IsAction(Sellect);
                }
                else
                {
                    item.IsAction(Normal);
                }
            }
        }
    }
}