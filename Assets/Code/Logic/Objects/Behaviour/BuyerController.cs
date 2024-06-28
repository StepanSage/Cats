using DG.Tweening;
using Objects;
using Player;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BuyerController : MonoBehaviour
{
  public Order Order;
  public Transform Transform { get; private set; }
  public Transform SlotBody;
  public Transform SlotPanel;
  public GameObject Body;
  public PlayerAnimation _playerAnimation;

  private float _moveSpeed = 2;
  private float _moveSpeedDelta = 0.4f;

  
  private CashOrderView _cashOrderView;
  private CashBehaviour _owner;

  // ===================================================================================================
  private void Awake()
  {
    Transform = transform;
    _playerAnimation = GetComponent<PlayerAnimation>();

    EventManager.Instance?.AddListener<E_OrderComplete>(OnOrderComplete);
  }

  // ===================================================================================================
  private void OnDestroy()
  {
    EventManager.Instance?.RemoveListener<E_OrderComplete>(OnOrderComplete);
  }

  // ===================================================================================================
  private void OnOrderComplete(E_OrderComplete evt)
  {
    var order = evt.Order;

    if ((evt.Order == null) || !evt.Order.Equals(Order))
      return;

    GoAway();
  }

  // ===================================================================================================
  /// <summary>
  /// Инициализация покупателя
  /// </summary>
  /// <param name="order">Заказ</param>
  public void Init(Order order, CashOrderView cashOrderView, CashBehaviour owner)
  {
    if (order == null)
    {
      BuyerDestroy();
      return;
    }

    Order = order;
    _cashOrderView = cashOrderView;
    _owner = owner;

    Transform.SetParentWithParam(transform);
    Transform.SetPositionAndRotation(_cashOrderView.PointAway.position, _cashOrderView.PointAway.rotation);

    LoadBuyerBody();
    GoToCash();
  }

  // ===================================================================================================
  /// <summary>
  /// Загрузка модели покупателя
  /// </summary>
  public void LoadBuyerBody()
  {
    Body = Factory.Instance.GetDecor("decor_buyer_1");
    Body.transform.SetParentWithParam(SlotBody);
    _playerAnimation.Init();
  }

  // ===================================================================================================
  public void BuyerDestroy()
  {
    _owner.BuyerRemove(this);
    Destroy(gameObject);
  }

  // ===================================================================================================
  public void GoToCash()
  {
    GoToTarget(_cashOrderView.PointCash,
      ()=>
      {
        EventManager.Instance?.TriggerEvent(new E_BuyerStandingAtCash(this));
      });
  }

  // ===================================================================================================
  public void GoAway()
  {
    EventManager.Instance?.TriggerEvent(new E_BuyerGoAway(this));

    GoToTarget(_cashOrderView.PointAway,
      () =>
      {
        BuyerDestroy();
      });
  }

  // ===================================================================================================
  public void GoToTarget(Transform targetPoint, Action callBackEnd = null)
  {
    StartWalk();

    var distance = Vector3.Distance(Transform.position, targetPoint.position);

    DOTween.Kill(Transform);
    Transform.LookAt(targetPoint.position);
    Transform.DOMove(targetPoint.position, distance / _moveSpeed * Random.Range(1 - _moveSpeedDelta / 2, 1 + _moveSpeedDelta / 2))
      .SetEase(Ease.Linear)
      .SetLink(gameObject)
      .OnComplete(() =>
      {
        StopWalk();
        Transform.LookAt(targetPoint);
        callBackEnd?.Invoke();
      });
  }

  #region Animations
  // ===================================================================================================
  private void StartWalk()
  {
    _playerAnimation.SetWalk(true);
  }

  // ===================================================================================================
  private void StopWalk()
  {
    _playerAnimation.SetWalk(false);
  }

  // ===================================================================================================
  public void OnGet()
  {
    _playerAnimation.OnGet();
  }
  #endregion Animations
}
