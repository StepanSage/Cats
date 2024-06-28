using Objects;
using Player;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OrderProductElement : MonoBehaviour
{
  public Action<OrderProduct> OnBtnCompleteClick;

  [SerializeField]
  private Image _icon = null;
  [SerializeField]
  private GameObject _sign = null;
  [SerializeField]
  private GameObject _light = null;
  [SerializeField]
  private Button _button = null;

  private OrderProduct _product;
  
  private bool _isEnought = false;

  // ===================================================================================================
  private void OnEnable()
  {
    _button.onClick.AddListener(ButtonCompleteClick);
  }

  // ===================================================================================================
  private void OnDisable()
  {
    _button.onClick.RemoveListener(ButtonCompleteClick);
  }

  // ===================================================================================================
  public void Init(OrderProduct product)
  {
    if ((product == null) || (product.Consume == null))
      return;
   
    _product = product;
    _isEnought = ProjectUtils.CheckConsumeOnPlayerSlots(product.Consume);
    
    Redraw();

    gameObject.SetActive(true);
  }

  // ===================================================================================================
  public void Redraw()
  {
    _icon.sprite = Factory.Instance.GetIcon(_product.Consume.Type);

    _sign.SetActive(_product.Complete);
    _light.SetActive(!_product.Complete && _isEnought);
  }

  // ===================================================================================================
  public void ButtonCompleteClick()
  {
    if (!_isEnought)
      return;

    OnBtnCompleteClick?.Invoke(_product);
  }

  // ===================================================================================================
  public void Start()
  {
    EventManager.Instance?.AddListener<E_PlayerSlotUpdate>(OnPlayerSlotUpdate);
    EventManager.Instance?.AddListener<E_OrderProductComplete>(OnOrderProductComplete);
  }

  // ===================================================================================================
  public void OnDestroy()
  {
    EventManager.Instance?.RemoveListener<E_PlayerSlotUpdate>(OnPlayerSlotUpdate);
    EventManager.Instance?.RemoveListener<E_OrderProductComplete>(OnOrderProductComplete);
  }

  // ===================================================================================================
  public void OnPlayerSlotUpdate(E_PlayerSlotUpdate evt)
  {
    Init(_product);
  }

  // ===================================================================================================
  public void OnOrderProductComplete(E_OrderProductComplete evt)
  {
    if (!_product.Equals(evt.OrderProduct))
      return;

    //gameObject.SetActive(false);
    Init(_product);
  }
}
