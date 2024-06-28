using Player;
using Static;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogInventory : Dialog
{

  public enum EShopTab
  {
    All,
    Suits,
    Decors
  }

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
  [SerializeField]
  private List<ShopTabElement> _shopTabs = new List<ShopTabElement>();

  private List<ShopItemElement> _shopItems = new List<ShopItemElement>();

  private bool _tabInited = false;

  // ===================================================================================================
  public void Activate()
  {
    //if (!_tabInited)
    //{
    //  InitTabs();
    //}

    //OnTab_Click(EShopTab.All);
    Open();
  }
  /*
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
      Debug.LogWarning($"OnTab_Click {state}");

      RedrawTabs(state);
      Redraw(state);
    }

    // ===================================================================================================
    private void RedrawTabs(EShopTab state)
    {
      Debug.LogWarning($"RedrawTabs");

      foreach (var tab in _shopTabs)
      {
        tab.UpdateState(state);
      }
    }

    // ===================================================================================================
    private void Redraw(EShopTab tab)
    {
      Debug.LogWarning($"Redraw {tab}");

      List<ShopData> shops = new List<ShopData>();

      if (tab.Equals(EShopTab.All))
      {
        shops = Global.Instance.StaticData.ShopDatas;
      }
      else if (tab.Equals(EShopTab.Suits))
      {
        shops = Global.Instance.StaticData.ShopDatas.Where(s => s.Id.Equals("suits")).ToList();
      }
      else if (tab.Equals(EShopTab.Decors))
      {
        shops = Global.Instance.StaticData.ShopDatas.Where(s => s.Id.Equals("decors")).ToList();
      }

      Dictionary<ItemData, ShopItemData> itemDatas = new Dictionary<ItemData, ShopItemData>();


      foreach (var shop in shops.OrderBy(s=>s.SortOrder))
      {
        foreach (var shopItem in shop.ShopItems.OrderBy(s => s.SortOrder))
        {
          var item = Global.Instance.StaticData.ItemDatas.FirstOrDefault(i => i.Id.Equals(shopItem.ItemId));

          if (item == null)
            continue;

          itemDatas.Add(item, shopItem);
        }

      }

      int cacheEC = _shopItems.Count;
      int needCount = itemDatas.Count();

      foreach (var item in _shopItems)
      {
        item.gameObject.SetActive(false);
      }

      if (cacheEC < needCount)
      {
        for (int i = 0; i < (needCount - cacheEC); i++)
        {
          var elem = Instantiate(_shopItemPrefab, _shopItemContent);
          _shopItems.Add(elem);
        }
      }

      int ij = 0;
      foreach (var o in itemDatas)
      {
        _shopItems[ij].Init(o.Key, o.Value);
        _shopItems[ij].OnBtnBuyClick += OnBtnBuyClick;
        ij++;
      }
    }

    // ===================================================================================================
    public void OnBtnBuyClick(ItemData itemData)
    {
      ShopUtils.ItemBuy(itemData);
    }
    */
}