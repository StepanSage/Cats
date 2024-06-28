using Static;
using System.Collections.Generic;
using System.Linq;
using static SlotUtils;
using System.Reflection;

public class ProduceBehaviour : BuildingBehaviour
{
  public List<BuildingSlot> Slots { get; private set; } = new List<BuildingSlot>();

  // ===================================================================================================
  public ProduceBehaviour(View view, BuildData buildData) : base(view, buildData) { }

  // ===================================================================================================
  public void Init()
  {
    if (!Global.Instance.Game.IsStateBattle)
      return;

    Global.Instance.Game.OneSecondTimer.Update += OneSecondUpdate;
    Slots = new List<BuildingSlot>();

    for (var i = 0; i < BuildData.SlotCountMax; i++)
    {
      var slot = new BuildingSlot(i, this);
      Slots.Add(slot);
    }

    UpdateSlots();

    if (BuildData.AutoStart)
    {
      TryStartProduce(BuildData.Produce[0]);
    }
  }

  // ===================================================================================================
  /// <summary>
  /// Обновдение состояния здания
  /// </summary>
  /// <param name="slot">слот</param>
  public void UpdateBuildingState()
  {
    if (!IsComplete && IsCompleteAnySlots())
    {
      DebugX.LogForUtils($"SlotUtils : UpdateSlotState : Complete : {BuildData.Id}");
      ChangeState(EBehaviourState.Complete);
    }
    else if (!IsInProgress && IsInProgressAnySlots())
    {
      DebugX.LogForUtils($"SlotUtils : UpdateSlotState : InProgress : {BuildData.Id}");
      ChangeState(EBehaviourState.IsProgress);
    }
    else if (!IsIdle && !IsCompleteAnySlots() && !IsInProgressAnySlots())
    {
      DebugX.LogForUtils($"SlotUtils : UpdateSlotState : Empty : {BuildData.Id}");
      ChangeState(EBehaviourState.Idle);
    }
  }

  // ===================================================================================================
  protected override void ChangeState(EBehaviourState newBehaviourState)
  {
    base.ChangeState(newBehaviourState);
    UpdateSlots();
  }

  // ===================================================================================================
  protected void UpdateSlots()
  {
    Slots.ForEach(slot => slot.UpdateSlotState(UpdateBuildingState));
  }

  // ===================================================================================================
  protected void OneSecondUpdate()
  {
    if (!IsInProgress)
      return;

    UpdateSlots();
  }

  // ===================================================================================================
  public bool IsCompleteAnySlots()
  {
    return Slots.Any(s => s.IsComplete);
  }

  // ===================================================================================================
  public bool IsInProgressAnySlots()
  {
    return Slots.Any(s => s.IsInProgress);
  }

  // ===================================================================================================
  public bool IsEmptyAnySlots()
  {
    return Slots.Any(s => s.IsEmpty);
  }

  // ===================================================================================================
  public override void Activate()
  {
    DebugX.LogForBehaviour($"Activate : {BuildData.Id}  IsComplete = {IsComplete} / ");

    if (IsComplete)
    {
      if (!ProjectUtils.CheckPlayerEmptySlot())
      {
        DialogSystem.Instance?.Get<DialogInfo>()?.Activate("Все слоты заняты !!!", "ОК");
        return;
      }

      var slot = Slots.FirstOrDefault(s => s.IsComplete);

      if (slot == null)
      {
        UpdateBuildingState();
        return;
      }

      var start = View.Transform.position.WorldToScreenPoint();
      var end = Global.Instance.Game.Player.Transform.position.WorldToScreenPoint();
      slot.Complete(
        () =>
        {
          Global.Instance.Game.Player.OnGet();

          UpdateBuildingState();

          if (BuildData.AutoStart)
          {
            TryStartProduce(BuildData.Produce[0]);
          }
        }
        );

      return;
    }

    if (!BuildData.AutoStart)
    {
      PanelSystem.Instance.Get<PanelActions>()?.Activate(this);
    }
  }

  // ===================================================================================================
  /// <summary>
  /// Запуск производства
  /// </summary>
  public virtual void TryStartProduce(string productId)
  {
    #region Errors
    if (ProjectUtils.CheckError(!IsIdle && !IsEmptyAnySlots(), "!IsIdle && !IsEmptyAnySlots()", MethodBase.GetCurrentMethod()))
      return;

    var slot = Slots.FirstOrDefault(s => s.IsEmpty);

    if (ProjectUtils.CheckError(slot == null, "slot == null", MethodBase.GetCurrentMethod()))
    {
      UpdateBuildingState();
      return;
    }

    if (ProjectUtils.CheckError(!slot.CanProduce(productId), "!slot.CanProduce(productId)", MethodBase.GetCurrentMethod()))
    {
      DialogSystem.Instance?.Get<DialogInfo>()?.Activate("Не хватает ресурсов для производства !!!", "ОК");
      return;
    }
    #endregion Errors

    slot.Produce(productId);
    UpdateBuildingState();
  }
}
