using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Player;
using System;
using UnityEngine.UI;

public class OrderElement : MonoBehaviour
{
  public Order Order => _order;
  public Action<OrderProduct, Vector3> OnBtnCompleteClick;

  [SerializeField]
  private Image _mask;
  [SerializeField]
  private OrderProductElement _orderProductPrefab;
  [SerializeField]
  private Transform _orderProductsContent;
  [SerializeField]
  private Transform _content;

  private List<OrderProductElement> _orderProductItems = new List<OrderProductElement>();

  private Order _order;
  private BuyerController _buyer;

  private Transform _transform;

  // ===================================================================================================
  public void Init(Order order)
  {
    if (order == null)
      return;

    _order = order;
    _transform = transform;

    UpdateTimer();
    Redraw();

    gameObject.SetActive(_buyer != null);
  }

  // ===================================================================================================
  public void Clear()
  {
    _order = null;
    _buyer = null;
    gameObject.SetActive(false);
  }

  // ===================================================================================================
  public void Redraw()
  {
    List<OrderProduct> poroducts = _order.Products;

    int cacheEC = _orderProductItems.Count;
    int needCount = poroducts.Count();

    foreach (var prod in _orderProductItems)
    {
      prod.gameObject.SetActive(false);
    }

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_orderProductPrefab, _orderProductsContent);
        _orderProductItems.Add(elem);
      }
    }

    int ij = 0;

    foreach (var prod in poroducts)
    {
      _orderProductItems[ij].Init(prod);
      _orderProductItems[ij].OnBtnCompleteClick += OnConsumeComplete;
      ij++;
    }

    _orderProductsContent.UpdateLayout();
    _content.UpdateLayout();
    _transform.UpdateLayout();
  }

  // ===================================================================================================
  public void OnConsumeComplete(OrderProduct orderProduct)
  {
    OnBtnCompleteClick?.Invoke(orderProduct, transform.position);
  }

  // ===================================================================================================
  public void UpdateTimer()
  {
    if (_order == null)
      return;
    
    _mask.fillAmount = _order.Expire.ProgressInvert();
  }

  // ===================================================================================================
  public void SetBuyer(BuyerController buyer)
  {
    _buyer = buyer;
    Init(_order);
  }

  // ===================================================================================================
  public void OnDisable()
  {
    try
    {
      Global.Instance.Game.OneSecondTimer.Update -= UpdateTimer;
    }
    catch { }
  }

  // ===================================================================================================
  public void OnEnable ()
  {
    Global.Instance.Game.OneSecondTimer.Update += UpdateTimer;
    UpdateTimer();
  }

  // ===================================================================================================
  private void FixedUpdate()
  {
    if (_buyer == null)
      return;
    
    var pos = _buyer.SlotPanel.position.WorldToScreenPoint();
    _transform.position = pos;
  }
}
