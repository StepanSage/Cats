using Objects;
using Static;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProduceView : MonoBehaviour
{
  [Serializable]
  class ProduceViewSlot
  {
    public int Index;
    public Transform Slot;
    public GameObject Product;
  }

  public Transform SlotForPanelAction => _slotForPanelAction;

  [SerializeField]
  private Transform _slotForPanelAction;
  [SerializeField]
  private GameObject _active;
  [SerializeField]
  private GameObject _idle;

  [SerializeField]
  private List<ProduceViewSlot> _slots = new List<ProduceViewSlot>();

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

  // ===================================================================================================
  public void SetInProgress(int slotId, Consume consume = null)
  {
    if (_active != null)
    {
      _active.SetActive(true);
    }

    if (_idle != null)
    {
      _idle.SetActive(false);
    }

    SetProductInSlot(slotId, consume);
  }

  // ===================================================================================================
  public void SetComplete(int slotId, Consume consume = null)
  {
    if (_active != null)
    {
      _active.SetActive(false);
    }

    if (_idle != null)
    {
      _idle.SetActive(true);
    }

    SetProductInSlot(slotId, consume);
  }

  // ===================================================================================================
  public void SetEmpty(int slotId)
  {
    if (_active != null)
    {
      _active.SetActive(false);
    }

    if (_idle != null)
    {
      _idle.SetActive(true);
    }

    CleanProductInSlot(slotId);
  }

  // ===================================================================================================
  private void SetProductInSlot(int slotId, Consume consume = null)
  {
    var slot = _slots.FirstOrDefault(s => s.Index.Equals(slotId));

    if (slot == null)
      return;

    if (slot.Product != null)
    {
      Destroy(slot.Product);
    }

    if (consume == null)
      return;

    InstantiateProduct(consume.Type, slot);
  }

  // ===================================================================================================
  private void CleanProductInSlot(int slotId)
  {
    var slot = _slots.FirstOrDefault(s => s.Index.Equals(slotId));

    if ((slot != null) && (slot.Product != null))
    {
      Destroy(slot.Product);
    }
  }

  // ===================================================================================================
  private void InstantiateProduct(string id, ProduceViewSlot slot)
  {
    var product = Factory.Instance.GetProduct(id);

    slot.Product = product;

    if (product == null)
      return;

    product.transform.SetParentWithParam(slot.Slot, true);
    product.GetComponent<ProductView>()?.SetDefault();
  }
}
