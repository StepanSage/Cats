using Player;
using System;

public class OrderSystem
{
  // ===================================================================================================
  public void Start()
  {
    OrderUtils.CreateOrders();
    Global.Instance.Game.OneSecondTimer.Update += OrdersUpdateState;

    if (!EventManager.Instance.HasListener<E_OrdersUpdate>(OnOrdersUpdate))
    {
      EventManager.Instance?.AddListener<E_OrdersUpdate>(OnOrdersUpdate);
    }
  }

  // ===================================================================================================
  public void Stop()
  {
    Global.Instance.Game.OneSecondTimer.Update -= OrdersUpdateState;

    if (EventManager.Instance != null)
    {
      EventManager.Instance?.RemoveListener<E_OrdersUpdate>(OnOrdersUpdate);
    }
  }

  // ===================================================================================================
  private void OnOrdersUpdate(E_OrdersUpdate evt)
  {
    TryAddNewActiveOrder();
  }

  // ===================================================================================================
  public void OrdersUpdateState()
  {
    SkipExpireOrders();
  }

  // ===================================================================================================
  public void TryAddNewActiveOrder()
  {
    OrderUtils.TryAddNewActiveOrder(
      () =>
      {
        EventManager.Instance?.TriggerEvent(new E_OrdersUpdate());
      });
  }

  // ===================================================================================================
  public void SkipExpireOrders()
  {
    OrderUtils.SkipExpireOrders(
      () =>
      {
        OrderUtils.TryAddNewActiveOrder();
        EventManager.Instance?.TriggerEvent(new E_OrdersUpdate());
      });
  }

  // ===================================================================================================
  public void CompleteOrderProduct(OrderProduct orderProduct, Action callbackEnd = null, Action callbackFail = null)
  {
    orderProduct.Complete(
      () =>
      {
        orderProduct.Order.Complete();
        EventManager.Instance?.TriggerEvent(new E_OrdersUpdate());
        callbackEnd?.Invoke();
      });
  }
}