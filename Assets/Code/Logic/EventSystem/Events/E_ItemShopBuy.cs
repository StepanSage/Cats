using Shop;

public class E_ItemShopBuy : EventBase
{
  public ShopItem ShopItem; 
  
  public E_ItemShopBuy(ShopItem shopItem) 
  {
    DebugX.LogForEvents($"E_ItemShopBuy : SEND  {shopItem.ItemData.Id}");
    ShopItem = shopItem;
  }
}