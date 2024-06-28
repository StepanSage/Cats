using Objects;
using Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CashOrderView : MonoBehaviour
{
  public Transform PointCash => _pointCash;
  public Transform PointAway => _pointAway;
  

  [Serializable]
  class ContentProduct
  {
    public Transform Transform;
    public Transform SlotProduct;
    public GameObject Product;
    public GameObject SlotPlate;
  }

  [Serializable]
  class ContentSlot
  {
    public List<Transform> Slots;
  }

  [SerializeField]
  private List<ContentSlot> _contentSlots;
  [SerializeField]
  private List<ContentProduct> _contentProducts;

  [SerializeField]
  private GameObject _active;
  [SerializeField]
  private GameObject _idle;
  [SerializeField]
  private Transform _pointCash;
  [SerializeField]
  private Transform _pointAway;

  // ===================================================================================================
  public void OnOrderComplete()
  {
    ClearSlotAndProducts();
  }

  // ===================================================================================================
  private void ClearSlotAndProducts()
  {
    _contentSlots.ForEach(s =>
    {
      s.Slots.ForEach(ss =>
      {
        ss.gameObject.SetActive(false);
      });
    });

    _contentProducts.ForEach(p =>
    {
      if (p.Product != null)
      {
        Destroy(p.Product);
      }

      p.Transform.gameObject.SetActive(false);
      p.SlotPlate.SetActive(false);
    });
  }

  // ===================================================================================================
  public void OrderProductComplete(OrderProduct orderProduct)
  {
    var contentProduct = _contentProducts[orderProduct.Index];
    var contentSlot = _contentSlots[orderProduct.Order.Products.Count()-1];
    var product = Factory.Instance.GetProduct(orderProduct.Consume.Type);

    if (product == null)
      return;

    product.transform.SetParentWithParam(contentProduct.SlotProduct, true);
    product.GetComponent<ProductView>()?.SetDefault();
    contentProduct.Product = product;
    contentProduct.Transform.SetParentWithParam(contentSlot.Slots[orderProduct.Index], false);
  }

  // ===================================================================================================
  public void OrderStartProgress(Order order)
  {
    ClearSlotAndProducts();

    var contentSlot = _contentSlots[order.Products.Count() - 1];
    int prodContentIndex = 0;

    foreach (var slot in contentSlot.Slots)
    {
      slot.gameObject.SetActive(true);
      var cp = _contentProducts[prodContentIndex];
      cp.Transform.gameObject.SetActive(true);
      cp.SlotPlate.SetActive(true);
      cp.Transform.SetParentWithParam(slot);
      prodContentIndex++;
    }
  }

  // ===================================================================================================
  private void Start()
  {
    if (_active != null)
    {
      _active.SetActive(false);
    }

    if (_idle != null)
    {
      _idle.SetActive(true);
    }
  }
}
