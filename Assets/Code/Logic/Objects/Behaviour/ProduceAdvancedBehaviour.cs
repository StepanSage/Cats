using Static;

public class ProduceAdvancedBehaviour : ProduceBehaviour
{
  // ===================================================================================================
  public ProduceAdvancedBehaviour(View view, BuildData buildData) : base(view, buildData) { }


  // ===================================================================================================
  protected override void ChangeState(EBehaviourState newBehaviourState)
  {
    base.ChangeState(newBehaviourState);
    
    if (newBehaviourState.Equals(EBehaviourState.Complete))
    {
      Global.Instance.Game.Player.OnStopUse();
    }
  }

  // ===================================================================================================
  /// <summary>
  /// Запуск производства
  /// </summary>
  public override void TryStartProduce(string productId)
  {
    base.TryStartProduce(productId);

    Global.Instance.Game.Player.OnUse();
  }
}
