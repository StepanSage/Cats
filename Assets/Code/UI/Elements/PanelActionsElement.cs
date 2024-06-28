using UnityEngine;
using Static;
using System;
using UnityEngine.UI;
using Objects;

public class PanelActionsElement : MonoBehaviour
{
  [SerializeField]
  private Image _icon;
  [SerializeField]
  private AnimationButton _animationButton;

  private Action<ProductData> _onClick;
  private ProductData _product;
  private bool _isIngredientEnought = false;

  // ===================================================================================================
  public void Init(ProductData product, Action<ProductData> onClick)
  {
    _product = product;

    if (_product == null)
      return;

    _isIngredientEnought = _product.IsIngredientEnought();
    _onClick = onClick;

    _animationButton.enabled = _isIngredientEnought;

    Redraw();
    gameObject.SetActive(true);
  }

  // ===================================================================================================
  public void Redraw()
  {
    _icon.sprite = Factory.Instance.GetIcon(_product.Id);
  }

  // ===================================================================================================
  public void GetRatingreward_Click()
  {
    if (!_isIngredientEnought)
      return;

    _onClick?.Invoke(_product);
  }
}
