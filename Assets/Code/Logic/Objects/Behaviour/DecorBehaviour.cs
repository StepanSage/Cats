using Extension;
using GameLogic;
using Static;
using System.Collections.Generic;
using static SlotUtils;

public class DecorBehaviour : BuildingBehaviour
{
  public List<BuildingSlot> Slots { get; private set; } = new List<BuildingSlot>();

  // ===================================================================================================
  public DecorBehaviour(View view, BuildData buildData) : base(view, buildData) { }

  // ===================================================================================================
  public void Init()
  {
    if (!Global.Instance.Game.IsStateBattle)
      return;

    DebugX.LogForBehaviour($"DecorBehaviour : Init : Start");

    var decor = BuildData.Produce.GetRandomElement();

    View.ProduceView.SetComplete(0, new Consume (decor,1));
  }

  // ===================================================================================================
  protected override void OnMouseUp() { }
}