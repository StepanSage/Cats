using DG.Tweening;
using Objects;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EmptyGraphic))]
[RequireComponent(typeof(CanvasRenderer))]
public class AnimationButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
  [SerializeField]
  private float _minScale = 0.85f;
  [SerializeField]
  private float _hoverScale = 1.1f;
  [SerializeField]
  private float _scaleTime = 0.1f;

  private Transform _content;
  private bool _pushed;
  private bool _active = true;

  private static Canvas _mainCanvas = null;

  // ===================================================================================================
  public void Awake()
  {
    _content = transform.GetChild(0).transform;

    if (_mainCanvas == null)
    {
      _mainCanvas = GetComponentInParent<Canvas>();
    }

    var emptyGraphic = gameObject.GetComponent<EmptyGraphic>();
  }

  // ===================================================================================================
  public void OnPointerDown(PointerEventData data)
  {
    if (!_active || _pushed)
      return;

    _content?.DOScale(_minScale, _scaleTime);
    _pushed = true;
  }

  // ===================================================================================================
  public void OnPointerUp(PointerEventData data)
  {
    if (!_active)
      return;

    _content?.DOScale(1, _scaleTime);
    _pushed = false;
  }

  // ===================================================================================================
  public void OnPointerExit(PointerEventData eventData)
  {
    if (!_active)
      return;

    _content?.DOScale(1, _scaleTime);
  }

  // ===================================================================================================
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!_active)
      return;

    _content?.DOScale((_pushed ? _minScale : _hoverScale), _scaleTime);
  }

  // ===================================================================================================
  public void OnEnable()
  {
    _pushed = false;
    _content.localScale = Vector3.one;
  }

  // ===================================================================================================
  public void OnDestroy()
  {
    _content.DOKill();
  }
}
