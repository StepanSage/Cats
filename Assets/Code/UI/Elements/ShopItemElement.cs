using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Objects;
using Shop;

public class ShopItemElement : MonoBehaviour
{
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private TextMeshProUGUI _name;
  [SerializeField]
  private Image _priceIcon;
  [SerializeField]
  private TextMeshProUGUI _riceText;
  [SerializeField]
  private GameObject _selected;
  [SerializeField]
  private GameObject _activated;
  [SerializeField]
  private GameObject _buyed;
  [SerializeField]
  private GameObject _price;

  private ShopItem _shopItem;
  private Action<ShopItem> _onBtnSelectClick;

  // ===================================================================================================
  public void Init(ShopItem shopItem, Action<ShopItem> onBtnSelectClick = null)
  {
    if (shopItem == null)
      return;

    _onBtnSelectClick = onBtnSelectClick;

    _shopItem = shopItem;

    DebugX.LogForUI($"ShopItemElement : Init : ");

    Redraw();
    gameObject.SetActive(true);
  }

  // ===================================================================================================
  public void Redraw()
  {
    _name.text = _shopItem.ItemData.Id;
    _icon.sprite = Factory.Instance.GetIcon(_shopItem.ItemData.Id);

    _price.SetActive(!_shopItem.IsBuyed);
    _activated.SetActive(_shopItem.IsActive);
    _buyed.SetActive(_shopItem.IsBuyed && !_shopItem.IsActive);

    if (!_shopItem.IsBuyed)
    {
      var consume = _shopItem.ItemData.Prices[0];
      _priceIcon.sprite = Factory.Instance.GetIcon(consume.Type);
      _riceText.text = consume.Amount.ToString();
    }

    _selected.SetActive(_shopItem.IsSelected);
  }

  // ===================================================================================================
  public void OnBtnSelect_Click()
  {
    _onBtnSelectClick?.Invoke(_shopItem);
  }
}
