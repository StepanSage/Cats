using Objects;
using Player;
using Static;
using System.Collections.Generic;
using UnityEngine;

public class CashBehaviour : BuildingBehaviour
{
  private List<BuyerController> _buyers = new List<BuyerController>();

  // ===================================================================================================
  public CashBehaviour(View view, BuildData buildData) : base(view, buildData)
  {
    EventManager.Instance?.AddListener<E_OrderComplete>(OnOrderComplete);
    EventManager.Instance?.AddListener<E_OrderProductComplete>(OnOrderProductComplete);
    EventManager.Instance?.AddListener<E_OrderStartProgress>(OnOrderStartProgress);
  }

  // ===================================================================================================
  ~CashBehaviour()
  {
    EventManager.Instance?.RemoveListener<E_OrderComplete>(OnOrderComplete);
    EventManager.Instance?.RemoveListener<E_OrderProductComplete>(OnOrderProductComplete);
    EventManager.Instance?.RemoveListener<E_OrderStartProgress>(OnOrderStartProgress);
  }

  // ===================================================================================================
  private void OnOrderComplete(E_OrderComplete evt)
  {
    if (evt.Order == null)
      return;

    var order = evt.Order;
    var cashOrderView = View.CashView.GetCashOrderViewByIndex(order.OrderSlot.Id);
    cashOrderView.OnOrderComplete();

    if (order.OrderSlot != null)
    {
      order.OrderSlot.IsBussy = false;
      order.OrderSlot = null;
    }
  }

  // ===================================================================================================
  private void OnOrderProductComplete(E_OrderProductComplete evt)
  {
    var orderProduct = evt.OrderProduct;
    var cashOrderView = View.CashView.GetCashOrderViewByIndex(orderProduct.Order.OrderSlot.Id);

    cashOrderView.OrderProductComplete(orderProduct);
  }

  // ===================================================================================================
  private void OnOrderStartProgress(E_OrderStartProgress evt)
  {
    var order = evt.Order;
    var cashOrderView = View.CashView.GetCashOrderViewByIndex(order.OrderSlot.Id);

    cashOrderView.OrderStartProgress(order);

    CreateNewBuyer(order, cashOrderView);
  }

  // ===================================================================================================
  public void Activate(OrderProduct product)
  {
    Activate();

    if (product != null)
    {
      Global.Instance.Game.Battle.OrderSystem.CompleteOrderProduct(product);
    }
  }

  // ===================================================================================================
  public override void Activate()
  {
    Global.Instance.Game.Player.OnCalc();
  }

  // ===================================================================================================
  public void CreateNewBuyer(Order order, CashOrderView cashOrderView)
  {
    var buyer = Factory.Instance.GetBuilding("buyer");

    if (buyer == null)
    {
      Debug.LogError($"LevelObject \"buyer\" is NULL!!!");
      return;
    }

    var controller = buyer.GetComponent<BuyerController>();

    controller.Init(order, cashOrderView, this);
  }

  // ===================================================================================================
  public void BuyerRemove(BuyerController buyer)
  {
    _buyers.Remove(buyer);
  }
}
