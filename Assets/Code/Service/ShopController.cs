using DG.Tweening;
using UnityEngine;

public class ShopController : MonoBehaviour
{
  public Transform Transform;
  public Transform Slot;

  private GameObject _object;
  private Transform _objectTransform;

  private float _rotationSpeed = 1;

  // ===================================================================================================
  private void Awake()
  {
    Transform = transform;
  }

  // ===================================================================================================
  /// <summary>
  /// Проветрка состояния слота здания
  /// </summary>
  /// <param name="slot">слот</param>
  public void RotateObject(float y)
  {
    _objectTransform.Rotate(new Vector3(_objectTransform.rotation.x, _objectTransform.rotation.y + y * _rotationSpeed, _objectTransform.rotation.z), Space.Self);
  }

  // ===================================================================================================
  public void Clear()
  {
    if (_object != null)
    {
      Destroy(_object);
    }
  }

  // ===================================================================================================
  public void SetGameObject(GameObject obj)
  {
    _object = obj;
    _objectTransform = _object.transform;
  }
}
