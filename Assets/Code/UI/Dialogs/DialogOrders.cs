using System.Linq;
using Player;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class DialogOrders : Dialog
{
  [SerializeField]
  private TextMeshProUGUI _completed;
  [SerializeField]
  private TextMeshProUGUI _skiped;

  [Space(10f)]
  [SerializeField]
  private OrderElement _orderPrefab;
  [SerializeField]
  private Transform _ordersContent;

  private List<OrderElement> _orderItems = new List<OrderElement>();

  // ===================================================================================================
  public void Start()
  {
    EventManager.Instance?.AddListener<E_OrdersUpdate>(OnOrdersUpdate);
    EventManager.Instance?.AddListener<E_PlayerSlotUpdate>(OnPlayerSlotUpdate);
  }

  // ===================================================================================================
  public void OnDestroy()
  {
    EventManager.Instance?.RemoveListener<E_OrdersUpdate>(OnOrdersUpdate);
    EventManager.Instance?.RemoveListener<E_PlayerSlotUpdate>(OnPlayerSlotUpdate);
  }

  // ===================================================================================================
  public void OnOrdersUpdate(E_OrdersUpdate evt)
  {
    Redraw();
  }
  // ===================================================================================================
  public void OnPlayerSlotUpdate(E_PlayerSlotUpdate evt)
  {
    Redraw();
  }

  // ===================================================================================================
  public void Activate()
  {
    Redraw();
    Open();
  }

  // ===================================================================================================
  private void Redraw()
  {
    _orderPrefab.gameObject.SetActive(false);

    var orders = OrderUtils.GetOrdersInProgress();

    foreach (var prod in _orderItems)
    {
      prod.gameObject.SetActive(false);
    }

    if (orders == null)
      return;

    int cacheEC = _orderItems.Count;
    int needCount = orders.Count();

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_orderPrefab, _ordersContent);
        _orderItems.Add(elem);
      }
    }

    int ij = 0;
    foreach (var o in orders)
    {
      //_orderItems[ij].Init(o);
      _orderItems[ij].OnBtnCompleteClick += OnBtnCompleteOrderProductClick;
      ij++;
    }

    var completeOrders = OrderUtils.GetOrdersComplete();
    var skipOorders = OrderUtils.GetOrdersSkip(); 
    
    _completed.text = completeOrders.Count().ToString();
    _skiped.text = skipOorders.Count().ToString();

    _ordersContent.UpdateLayout();
  }

  // ===================================================================================================
  public void OnBtnCompleteOrderProductClick(OrderProduct orderProduct, Vector3 position)
  {
    CompleteOrderProduct(orderProduct, position);
  }

  // ===================================================================================================
  public void CompleteOrderProduct(OrderProduct orderProduct, Vector3 position, Action callbackEnd = null, Action callbackFail = null)
  {
    Global.Instance.Game.Battle.OrderSystem.CompleteOrderProduct(orderProduct,
      () =>
      {
        /*
        ProjectUtils.AnimatedRemove(orderProduct.Consume,
          Global.Instance.Game.Player.GetScreenPointPosition(),
          position);
        */
      });
  }
}