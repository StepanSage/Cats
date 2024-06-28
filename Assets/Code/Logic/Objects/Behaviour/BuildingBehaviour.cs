using GameLogic;
using Static;

public abstract class BuildingBehaviour
{
  public enum EBehaviourState
  {
    Idle,
    IsProgress,
    Complete
  }

  public EBehaviourState State => _state;
  public View View;
  public BuildData BuildData;

  public bool IsInProgress => _state.Equals(EBehaviourState.IsProgress);
  public bool IsComplete => _state.Equals(EBehaviourState.Complete);
  public bool IsIdle => _state.Equals(EBehaviourState.Idle);

  protected EBehaviourState _state;

  // ===================================================================================================
  protected BuildingBehaviour(View view, BuildData buildData)
  {
    View = view;
    BuildData = buildData;

    View.OnObjectMouseDown = null;
    View.OnObjectMouseUp = null;
    View.OnObjectStart = null;
    View.OnObjectUpdate = null;
    View.OnObjectDestroy = null;
    View.OnObjectEnable = null;
    View.OnObjectDisable = null;
    View.OnObjectDrawGizmos = null;

    if (!Global.Instance.Game.IsStateBattle)
      return;

    View.OnObjectMouseDown += OnMouseDown;
    View.OnObjectMouseUp += OnMouseUp;
    View.OnObjectStart += Start;
    View.OnObjectUpdate += Update;
    View.OnObjectDestroy += Destroy;
    View.OnObjectEnable += OnEnable;
    View.OnObjectDisable += OnDisable;
    View.OnObjectDrawGizmos += OnDrawGizmos;
  }

  // ===================================================================================================
  ~BuildingBehaviour()
  {
    if (View.OnObjectMouseDown != null) View.OnObjectMouseDown -= OnMouseDown;
    if (View.OnObjectMouseUp != null) View.OnObjectMouseUp -= OnMouseUp;
    if (View.OnObjectStart != null) View.OnObjectStart -= Start;
    if (View.OnObjectUpdate != null) View.OnObjectUpdate -= Update;
    if (View.OnObjectDestroy != null) View.OnObjectDestroy -= Destroy;
    if (View.OnObjectEnable != null) View.OnObjectEnable -= OnEnable;
    if (View.OnObjectDisable != null) View.OnObjectDisable -= OnDisable;
    if (View.OnObjectDrawGizmos != null) View.OnObjectDrawGizmos -= OnDrawGizmos;
  }

  // ===================================================================================================
  public virtual void Activate() { }

  protected virtual void Update() { }
  protected virtual void Start() { }
  protected virtual void Destroy() { }
  protected virtual void OnEnable() { }
  protected virtual void OnDisable() { }
  
  protected virtual void OnMouseDown() { }
  protected virtual void OnMouseMove() { }
  protected virtual void OnDrawGizmos() { }

  // ===================================================================================================
  protected virtual void OnMouseUp()
  {
    if (PlayerUtils.Controller.IsBussy)
      return;

    Global.Instance.Game.Player.GoToTarget(this);
  }

  // ===================================================================================================
  protected virtual void ChangeState(EBehaviourState newBehaviourState)
  {
    _state = newBehaviourState;
  }

  // ===================================================================================================
  protected void SetProduceState()
  {
    ChangeState(EBehaviourState.IsProgress);
  }

  // ===================================================================================================
  protected void SetCompleteState()
  {
    ChangeState(EBehaviourState.Complete);
  }

  // ===================================================================================================
  protected void SetIdleState()
  {
    ChangeState(EBehaviourState.Idle);
  }
}
