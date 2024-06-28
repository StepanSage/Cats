using Shop;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViewModelElement : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerMoveHandler
{
  private bool _isDown = false;
  
  // ===================================================================================================
  public void OnPointerDown(PointerEventData eventData)
  {
    _isDown = true;
  }

  // ===================================================================================================
  public void OnPointerMove(PointerEventData eventData)
  {
    if (!_isDown)
      return;

    ShopUtils.RotateObject(-eventData.delta.x);
  }

  // ===================================================================================================
  void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
  {
    _isDown = false;
  }
}
