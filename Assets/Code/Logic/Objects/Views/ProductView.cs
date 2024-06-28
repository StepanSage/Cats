using UnityEngine;

public class ProductView : MonoBehaviour
{
  private enum ProductState
  {
    Default,
    Chopped,
    Slice
  }

  [SerializeField]
  private GameObject _default;
  [SerializeField]
  private GameObject _chopped;
  [SerializeField]
  private GameObject _slice;

  // ===================================================================================================
  public void SetDefault()
  {
    SetState(ProductState.Default);
  }

  // ===================================================================================================
  public void SetChopped()
  {
    SetState(ProductState.Default);
  }

  // ===================================================================================================
  public void SetSlice()
  {
    SetState(ProductState.Default);
  }

  // ===================================================================================================
  /// <summary>
  /// Set product state
  /// </summary>
  /// <param name="state">state</param>
  private void SetState(ProductState state)
  {
    _default.SetActive(false);
    _chopped.SetActive(false);
    _slice.SetActive(false);

    if (state.Equals(ProductState.Default))
    {
      _default.SetActive(true);
    }
    else if (state.Equals(ProductState.Chopped))
    {
      _chopped.SetActive(true);
    }
    else if (state.Equals(ProductState.Slice))
    {
      _slice.SetActive(true);
    }
  }
}
