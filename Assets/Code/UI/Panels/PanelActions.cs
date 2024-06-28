using Static;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PanelActions : Panel
{
  [Space(10f)]
  [SerializeField]
  private TextMeshProUGUI _name;

  [Space(10f)]
  [SerializeField]
  private PanelActionsElement _productPrefab;
  [SerializeField]
  private Transform _productsContent;

  private List<PanelActionsElement> _productsItems = new List<PanelActionsElement>();

  private ProduceBehaviour _behaviour;

  // ===================================================================================================
  public void Activate(ProduceBehaviour behaviour)
  {
    _behaviour = behaviour;

    if (_behaviour == null)
    {
      DebugX.LogForUI("_behaviour == null");
      return;
    }

    if (_behaviour.BuildData == null)
    {
      DebugX.LogForUI("_behaviour.BuildData");
      return;
    }

    Redraw();
    Open();
  }

  // ===================================================================================================
  private void Redraw()
  {
    _name.text = LocalizeManager.Instance.Get(_behaviour.BuildData.Id);
    var products = ProjectUtils.GetProductDatasByIds(_behaviour.BuildData.Produce);

    foreach (var prod in _productsItems)
    {
      prod.gameObject.SetActive(false);
    }

    int cacheEC = _productsItems.Count;
    int needCount = products.Count();

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_productPrefab, _productsContent);
        _productsItems.Add(elem);
      }
    }

    int ij = 0;
    foreach (var prod in products)
    {
      _productsItems[ij].Init(prod, OnClick);
      ij++;
    }
  }

  // ===================================================================================================
  private void OnClick(ProductData product)
  {
    _behaviour.TryStartProduce(product.Id);
    Close();
  }

  // ===================================================================================================
  private void FixedUpdate()
  {
    var pos = _behaviour.View.ProduceView.SlotForPanelAction.position.WorldToScreenPoint();
    transform.position = pos;
  }
}
