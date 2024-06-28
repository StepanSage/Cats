using DG.Tweening;
using Player;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public Transform Transform;
  public Transform SlotBody;
  public GameObject Body;
  public bool IsBussy => _playerAnimation.IsBussy;

  private float _moveSpeed = 4;
  private BuildingBehaviour _objectToMove;
  public PlayerAnimation _playerAnimation;
  private BuildingBehaviour _objectActivate;
  private OrderProduct _targetProduct;

  // ===================================================================================================
  private void Awake()
  {
    Transform = transform;
    _playerAnimation = GetComponent<PlayerAnimation>();
  }

  // ===================================================================================================
  public void Init()
  {
    _playerAnimation.Init();
  }

  // ===================================================================================================
  public void BodyClear()
  {
    if (Body != null)
    {
      Destroy(Body);
    }
  }

  // ===================================================================================================
  public void SetObjectActivate(BuildingBehaviour buildingBehaviour)
  {
    _objectActivate = buildingBehaviour;
    EventManager.Instance?.TriggerEvent(new E_PlayerSetActivateBuilding(buildingBehaviour));
  }

  // ===================================================================================================
  public void GoToTarget(BuildingBehaviour obj)
  {
    _targetProduct = null;
    GoToTargetStart(obj);
  }

  // ===================================================================================================
  public void GoToCashWithOrder(OrderProduct product)
  {
    _targetProduct = product;
    ProjectUtils.TryGetBuildingById("cash", out BuildingBehaviour beh);
    GoToTargetStart(beh);
  }

  // ===================================================================================================
  public void GoToTargetStart(BuildingBehaviour obj)
  {
    if ((_objectToMove == null) || !_objectToMove.Equals(obj) && Global.Instance.Game.OneSecondTimer.IsActive())
    {
      _objectToMove = obj;
      SetObjectActivate(null);

      StartWalk();

      var targetPoint = _objectToMove.View.GetStayPoint();
      var distance = Vector3.Distance(Transform.position, targetPoint.position);

      DOTween.Kill(Transform);
      Transform.LookAt(targetPoint.position);
      Transform.DOMove(targetPoint.position, distance / _moveSpeed)
        .SetEase(Ease.Linear)
        .SetLink(gameObject)
        .OnComplete(() =>
        {
          StopWalk();
          Transform.LookAt(_objectToMove.View.Transform);
          SetObjectActivate(_objectToMove);

          if (_objectActivate is CashBehaviour beh)
          {
            beh.Activate(_targetProduct);
          }
        });
    }
    else
    {
      if (_objectActivate is CashBehaviour beh)
      {
        beh.Activate(_targetProduct);
      }
      else
      {
        _objectActivate?.Activate();
      }
    }
  }

  #region Animations
  // ===================================================================================================
  private void StartWalk()
  {
    _playerAnimation.OnWalk();//.SetWalk(true);
  }

  // ===================================================================================================
  private void StopWalk()
  {
    _playerAnimation.OnIdle();//.SetWalk(false);
  }

  // ===================================================================================================
  public void OnCalc()
  {
    if (IsBussy)
      return;

    _playerAnimation.OnCalc();
  }

  // ===================================================================================================
  public void OnGet()
  {
    if (IsBussy)
      return;

    _playerAnimation.OnGet();
  }

  // ===================================================================================================
  public void OnUse()
  {
    if (IsBussy)
      return;

    _playerAnimation.OnUse();
  }

  // ===================================================================================================
  public void OnStopUse()
  {
    _playerAnimation.OnStopUse();
  }
  #endregion Animations
}
