using Static;
using System.Linq;
using System;
using System.Reflection;
using Objects;
using System.Collections.Generic;
using System.Threading.Tasks.Sources;

namespace Shop
{
  public enum EShopTab
  {
    All,
    Suits,
    Decors
  }

  public class ShopItem
  {
    public ItemData ItemData;
    public int ShopOrder;
    public int ItemOrder;
    public bool IsActive;
    public bool IsBuyed;
    public bool IsSelected;

    public ShopItem(ItemData itemData, int shopOrder = 0, int itemOrder = 0, bool isBuyed = false, bool isActive = false)
    {
      ItemData = itemData;
      IsActive = isActive;
      IsBuyed = isBuyed;
      ShopOrder = shopOrder;
      ItemOrder = itemOrder;
    }
  }

  public static class ShopUtils
  {
    public static ShopController ShopController => Global.Instance.Game.ShopController;
    public static ShopItem SelectedShopItem => _selectedShopItem;

    private static List<ShopItem> _shopItems;

    private static ShopItem _selectedShopItem;

    // ===================================================================================================
    /// <summary>
    /// Выделить объект
    /// </summary>
    public static void SelectObject (ShopItem shopItem)
    {
      _selectedShopItem = shopItem;
      _shopItems.ForEach(item => { item.IsSelected = item.Equals(_selectedShopItem); });
      LoadShopObject(shopItem.ItemData.Id);
    }

    // ===================================================================================================
    /// <summary>
    /// Сделать выбранный объект активным
    /// </summary>
    public static void ApplySelected()
    {
      var activeShopItem = GetActiveShopItem();
      activeShopItem.IsActive = false;
      _selectedShopItem.IsActive = true;
      PlayerUtils.SetBody(_selectedShopItem.ItemData.Id);
      PlayerUtils.LoadPlayerBody();
    }

    // ===================================================================================================
    /// <summary>
    /// Проветрка состояния слота здания
    /// </summary>
    /// <param name="slot">слот</param>
    public static void RotateObject(float delta)
    {
      ShopController.RotateObject(delta);
    }

    // ===================================================================================================
    /// <summary>
    /// Проветрка состояния слота здания
    /// </summary>
    /// <param name="slot">слот</param>
    public static void LoadShopObject(string id)
    {
      var obj = Factory.Instance.GetDecor(id);

      if (obj == null)
      {
        DebugX.LogError($"Can't find OBJECT {id}!!!");
        return;
      }

      ShopController.Clear();
      ShopController.SetGameObject(obj);
      obj.transform.SetParentWithParam(ShopController.Slot);
    }

    // ===================================================================================================
    /// <summary>
    /// Проветрка состояния слота здания
    /// </summary>
    /// <param name="slot">слот</param>
    public static void ItemBuy(Action calbackEnd = null, Action calbackFail = null)
    {
      #region Errors
      if (ProjectUtils.CheckError(_selectedShopItem == null, "itemData = null", MethodBase.GetCurrentMethod(), calbackFail))
        return;

      var prices = _selectedShopItem.ItemData.Prices;

      if (ProjectUtils.CheckError(prices == null || !prices.Any(), "prices = null", MethodBase.GetCurrentMethod(), calbackFail))
        return;

      if (ProjectUtils.CheckError(ProjectUtils.IsConsumeExist(_selectedShopItem.ItemData.Id), "IsConsumeExist", MethodBase.GetCurrentMethod(), calbackFail))
        return;

      #endregion Errors

      if (!ProjectUtils.IsEnought(prices))
      {
        DialogSystem.Instance?.Get<DialogInfo>()?.Activate("Не достаточно ресурсов!", "ОК");
        return;
      }

      ProjectUtils.AddConsume(new Consume(_selectedShopItem.ItemData.Id, 1));
      ProjectUtils.RemoveConsumeFromStorage(_selectedShopItem.ItemData.Prices);

      _selectedShopItem.IsBuyed = true;

      EventManager.Instance?.TriggerEvent(new E_ItemShopBuy(_selectedShopItem));
      calbackEnd?.Invoke();
    }

    // ===================================================================================================
    /// <summary>
    /// Инициализация магазина
    /// </summary>
    /// <param name="slot">слот</param>
    public static void InitShopItems()
    {
      _shopItems = new List<ShopItem>();

      foreach (var consume in PlayerUtils.Data.Storage)
      {
        var itemData = ProjectUtils.GetItemById(consume.Type);

        if (itemData == null)
          continue;

        if (itemData.Type.Equals(EItemDataType.Resource))
          continue;

        _shopItems.Add(new ShopItem(itemData, isBuyed: true));
      }

      foreach (var shop in Global.Instance.StaticData.ShopDatas.OrderBy(s => s.SortOrder))
      {
        foreach (var si in shop.ShopItems.OrderBy(s => s.SortOrder))
        {
          var itemData = Global.Instance.StaticData.ItemDatas.FirstOrDefault(i => i.Id.Equals(si.ItemId));

          if (itemData == null)
            continue;

          if (_shopItems.Any(s => s.ItemData.Equals(itemData)))
            continue;

          _shopItems.Add(new ShopItem(itemData, shopOrder: shop.SortOrder, itemOrder: si.SortOrder));
        }
      }

      var item = ProjectUtils.GetItemById(PlayerUtils.GetBody());
      var shopItem = _shopItems.FirstOrDefault(i => i.ItemData.Equals(item));

      if (shopItem == null)
        return;

      shopItem.IsActive = true;

      if (_selectedShopItem == null)
      {
        _selectedShopItem = GetActiveShopItem();
        SelectObject(_selectedShopItem);
      }
    }

    // ===================================================================================================
    /// <summary>
    /// Поулчить активный предмет магазина
    /// </summary>
    public static ShopItem GetActiveShopItem()
    {
      return _shopItems.FirstOrDefault(i => i.IsActive);
    }

    // ===================================================================================================
    /// <summary>
    /// Поулчить все предметы магазина по типу
    /// </summary>
    /// <param name="slot">слот</param>
    public static List<ShopItem> GetShopItemByShopType(EShopTab type)
    {
      if (_shopItems == null)
      {
        InitShopItems();
      }

      if (type.Equals(EShopTab.All))
      {
        return _shopItems.OrderByDescending(s => s.IsActive)
                  .ThenByDescending(s => s.IsBuyed)
                  .ThenBy(s => s.ShopOrder)
                  .ThenBy(s => s.ItemOrder).ToList();
      }
      else if (type.Equals(EShopTab.Suits))
      {
        return _shopItems.Where(s => s.ItemData.Type.Equals(EItemDataType.Suit))
                  .OrderBy(s => s.IsActive)
                  .ThenBy(s => s.IsBuyed)
                  .ThenBy(s => s.ShopOrder)
                  .ThenBy(s => s.ItemOrder).ToList();
      }
      else if (type.Equals(EShopTab.Decors))
      {
        return _shopItems.Where(s => s.ItemData.Type.Equals(EItemDataType.BuildingDecor))
                  .OrderBy(s => s.IsActive)
                  .ThenBy(s => s.IsBuyed)
                  .ThenBy(s => s.ShopOrder)
                  .ThenBy(s => s.ItemOrder).ToList();
      }
      else
      {
        return _shopItems.OrderBy(s => s.IsActive)
                    .ThenBy(s => s.IsBuyed)
                    .ThenBy(s => s.ShopOrder)
                    .ThenBy(s => s.ItemOrder).ToList();
      }
    }
  }
}