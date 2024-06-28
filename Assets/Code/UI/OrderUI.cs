using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player;

public class OrderUI : MonoBehaviour
{
  [Space(10f)]
  [SerializeField]
  private OrderElement _orderPrefab;
  [SerializeField]
  private Transform _ordersContent;

  private List<OrderElement> _orderItems = new List<OrderElement>();

  // ===================================================================================================
  public void Start()
  {
    _orderPrefab.gameObject.SetActive(false);

    EventManager.Instance?.AddListener<E_OrderStartProgress>(OnOrderStartProgress);
    EventManager.Instance?.AddListener<E_BuyerStandingAtCash>(OnBuyerStandingAtCash);
    EventManager.Instance?.AddListener<E_BuyerGoAway>(OnBuyerGoAway);
  }

  // ===================================================================================================
  public void OnDestroy()
  {
    EventManager.Instance?.RemoveListener<E_OrderStartProgress>(OnOrderStartProgress);
    EventManager.Instance?.RemoveListener<E_BuyerStandingAtCash>(OnBuyerStandingAtCash);
    EventManager.Instance?.RemoveListener<E_BuyerGoAway>(OnBuyerGoAway);
  }

  // ===================================================================================================
  public void OnOrderStartProgress(E_OrderStartProgress evt)
  {
    var el = _orderItems.FirstOrDefault(e => (e.Order == null) || e.Order.IsComplete);

    if (el == null)
      return;

    el.Init(evt.Order);
  }

  // ===================================================================================================
  public void OnOrderComplete(E_OrderComplete evt)
  {
    var el = _orderItems.FirstOrDefault(e => (e.Order != null) && e.Order.Equals(evt.Order));

    if (el == null)
      return;

    el.Clear();
  }

  // ===================================================================================================
  public void InitOrderItems()
  {
    if (_orderItems.Any())
      return;

    if (!Global.Instance.Game.IsStateBattle)
      return;

    int cacheEC = _orderItems.Count;
    int needCount = 3;

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_orderPrefab, _ordersContent);
        _orderItems.Add(elem);
      }
    }

    foreach (var orderItem in _orderItems)
    {
      orderItem.Clear();
      orderItem.OnBtnCompleteClick += OnBtnCompleteOrderProductClick;
    }
  }

  // ===================================================================================================
  public void Init()
  {
    InitOrderItems();
    InitOrders();
  }

  // ===================================================================================================
  public void InitOrders()
  {
    var orders = PlayerUtils.Orders.Where(o => o.IsInProgress);

    foreach (var o in orders)
    {
      var el = _orderItems.FirstOrDefault(e => (e.Order == null) || e.Order.IsComplete);

      if (el == null)
        return;

      el.Init(o);
    }
  }

  // ===================================================================================================
  public void OnBtnCompleteOrderProductClick(OrderProduct orderProduct, Vector3 position)
  {
    Global.Instance.Game.Player.GoToCashWithOrder(orderProduct);
  }

  // ===================================================================================================
  public void OnBuyerStandingAtCash(E_BuyerStandingAtCash evt)
  {
    var el = _orderItems.FirstOrDefault(e => (e.Order != null) && e.Order.Equals(evt.BuyerController.Order));

    if (el == null)
      return;

    el.SetBuyer(evt.BuyerController);
  }

  // ===================================================================================================
  public void OnBuyerGoAway(E_BuyerGoAway evt)
  {
    var el = _orderItems.FirstOrDefault(e => (e.Order != null) && e.Order.Equals(evt.BuyerController.Order));

    if (el == null)
      return;

    el.Clear();
  }
}
