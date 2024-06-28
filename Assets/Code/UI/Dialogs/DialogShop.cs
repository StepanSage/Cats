using Objects;
using Player;
using Shop;
using Static;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogShop : Dialog
{
  public enum EShopConsumeType
  {
    Gold,
    Crystal,
    Ads
  }

  [Space(10f)]
  [SerializeField]
  private ShopItemElement _shopItemPrefab;
  [SerializeField]
  private Transform _shopItemContent;

  [Space(10)]
  [Header("Buttons")]
  [SerializeField]
  private GameObject _btnBuy;
  [SerializeField]
  private Image _priceIcon;
  [SerializeField]
  private TextMeshProUGUI _priceText;
  [SerializeField]
  private GameObject _btnShow;

  [SerializeField]
  private List<ShopTabElement> _shopTabs = new List<ShopTabElement>();

  private List<ShopItemElement> _shopItemElements = new List<ShopItemElement>();
  private List<ShopItem> _shopItems = new List<ShopItem>();

  private bool _tabInited = false;
  
  private EShopTab _selectedTab;

  // ===================================================================================================
  public void Activate()
  {
    if (!_tabInited)
    {
      InitTabs();
    }

    OnTab_Click(EShopTab.Suits);
    Open();
  }

  // ===================================================================================================
  private void InitTabs()
  {
    Debug.LogWarning($"InitTabs");

    foreach (var tab in _shopTabs)
    {
      tab.OnTabClick = OnTab_Click;
    }
    _tabInited = true;
  }

  // ===================================================================================================
  private void OnTab_Click(EShopTab state)
  {
    _selectedTab = state;

    Debug.LogWarning($"OnTab_Click {_selectedTab}");

    RedrawTabs();
    Redraw();
  }

  // ===================================================================================================
  private void RedrawTabs()
  {
    Debug.LogWarning($"RedrawTabs");

    foreach (var tab in _shopTabs)
    {
      tab.UpdateState(_selectedTab);
    }
  }

  // ===================================================================================================
  private void Redraw()
  {
    _shopItems = ShopUtils.GetShopItemByShopType(_selectedTab);

    int cacheEC = _shopItemElements.Count;
    int needCount = _shopItems.Count();

    foreach (var item in _shopItemElements)
    {
      item.gameObject.SetActive(false);
    }

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_shopItemPrefab, _shopItemContent);
        _shopItemElements.Add(elem);
      }
    }

    int ij = 0;
    foreach (var o in _shopItems)
    {
      _shopItemElements[ij].Init(o, OnBtnSelectClick);
      ij++;
    }

    _btnBuy.SetActive(!ShopUtils.SelectedShopItem.IsBuyed);
    _btnShow.SetActive(ShopUtils.SelectedShopItem.IsBuyed && !ShopUtils.SelectedShopItem.IsActive);

    if (!ShopUtils.SelectedShopItem.IsBuyed)
    {
      var consume = ShopUtils.SelectedShopItem.ItemData.Prices[0];
      _priceIcon.sprite = Factory.Instance.GetIcon(consume.Type);
      _priceText.text = consume.Amount.ToString();
    }
  }

  // ===================================================================================================
  public void OnBtnSelectClick(ShopItem itemData)
  {
    SelectObject(itemData);
  }

  // ===================================================================================================
  public void SelectObject(ShopItem shopItem)
  {
    ShopUtils.SelectObject(shopItem);
    Redraw();
  }

  // ===================================================================================================
  public void OnBtnApplySelected_Click()
  {
    ShopUtils.ApplySelected();
    Redraw();
  }

  // ===================================================================================================
  public void OnBtnBuy_Click()
  {
    ShopUtils.ItemBuy(Redraw, Redraw);
  }
}