using UnityEngine;

public class CameraController : MonoBehaviour
{
  public Camera MainCamera;

  private Transform _target;
  private Transform _transform;
  private bool _inited = false;
  private Transform _transformSlot;

  // ===================================================================================================
  private void Awake()
  {
    _transform = gameObject.transform;
  }

  // ===================================================================================================
  public void Start()
  {
    Global.Instance.Game.CameraController = this;
    EventManager.Instance?.AddListener<E_PlayerInit>(OnPlayerInit);
  }

  // ===================================================================================================
  public void OnDestroy()
  {
    EventManager.Instance?.RemoveListener<E_PlayerInit>(OnPlayerInit);
  }

  // ===================================================================================================
  public void OnPlayerInit(E_PlayerInit evt)
  {
    _target = evt.PlayerController.Transform;
    _inited = true;
  }

  // ===================================================================================================
  public void SetSlot(Transform transform)
  {
    _transformSlot = transform;
    _transform.SetParentWithParam(transform);
  }

  // ===================================================================================================
  private void FixedUpdate()
  {
    if (!_inited)
      return;
    
    if (_transform.position.x.Equals(_target.position.x))
      return;

    float x = Mathf.Clamp(_target.position.x - _transformSlot.position.x, -2, 2);
    _transform.localPosition = new Vector3(x, 0, 0);
  }
}
