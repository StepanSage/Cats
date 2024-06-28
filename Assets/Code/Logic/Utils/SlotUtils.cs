using Static;
using System.Linq;
using UnityEngine;
using System;

public static class SlotUtils
{

  #region BuildingSlot
  // ===================================================================================================
  public enum EProduceSlotState
  {
    Start,
    Empty,
    InProgress,
    Complete
  }

  // ===================================================================================================
  public class BuildingSlot
  {
    public int Index;
    public Timer CompleteTimer;
    public Consume NeedConsume;
    public Consume FinishConsume;
    public EProduceSlotState State;
    public BuildingBehaviour Building;

    public bool IsComplete => State.Equals(EProduceSlotState.Complete);
    public bool IsInProgress => State.Equals(EProduceSlotState.InProgress);
    public bool IsEmpty => State.Equals(EProduceSlotState.Empty);
    public bool IsStart => State.Equals(EProduceSlotState.Start);

    public BuildingSlot(int index, BuildingBehaviour building)
    {
      Index = index;
      CompleteTimer = new Timer();
      State = EProduceSlotState.Start;
      Building = building;
    }
  }

  // ===================================================================================================
  public static void Clear(this BuildingSlot slot)
  {
    slot.CompleteTimer.Stop();
    slot.FinishConsume = null;
    slot.FinishConsume = null;
    slot.State = EProduceSlotState.Start;
  }

  // ===================================================================================================
  /// <summary>
  /// ѕроветрка состо€ни€ слота здани€
  /// </summary>
  /// <param name="slot">слот</param>
  public static void UpdateSlotState(this BuildingSlot slot, Action calbackNeedUpdate = null)
  {
    DebugX.LogForUtils($"SlotUtils : UpdateSlotState");

    if (!slot.IsComplete && slot.CompleteTimer.IsActive() && slot.CompleteTimer.IsExpired())
    {
      DebugX.LogForUtils($"SlotUtils : UpdateSlotState : Complete");
      slot.ChangeSlotState(EProduceSlotState.Complete);
      calbackNeedUpdate?.Invoke();
    }
    else if (!slot.IsInProgress && slot.CompleteTimer.IsActive() && !slot.CompleteTimer.IsExpired())
    {
      DebugX.LogForUtils($"SlotUtils : UpdateSlotState : InProgress");
      ChangeSlotState(slot, EProduceSlotState.InProgress);
      calbackNeedUpdate?.Invoke();
    }
    else if (!slot.IsEmpty && !slot.CompleteTimer.IsActive())
    {
      DebugX.LogForUtils($"SlotUtils : UpdateSlotState : Empty");
      ChangeSlotState(slot, EProduceSlotState.Empty);
      calbackNeedUpdate?.Invoke();
    }
  }

  // ===================================================================================================
  /// <summary>
  /// »зменение состо€ни€ слота здани€
  /// </summary>
  /// <param name="slot">слот</param>
  /// <param name="newState">состо€ние</param>
  public static void ChangeSlotState(this BuildingSlot slot, EProduceSlotState newState)
  {
    if (!slot.State.Equals(newState) && newState.Equals(EProduceSlotState.InProgress))
    {
      slot.Building.View?.ProduceView.SetInProgress(slot.Index, slot.NeedConsume);
    }

    if (!slot.State.Equals(newState) && newState.Equals(EProduceSlotState.Complete))
    {
      slot.Building.View?.ProduceView.SetComplete(slot.Index, slot.FinishConsume);
    }

    if (!slot.State.Equals(newState) && newState.Equals(EProduceSlotState.Empty))
    {
      slot.Building.View?.ProduceView.SetEmpty(slot.Index);
    }

    slot.State = newState;
    DebugX.LogForUtils($"SlotUtils : ChangeSlotState : {slot.State}");
  }

  // ===================================================================================================
  /// <summary>
  /// ѕроверка возможности запустить производство
  /// </summary>
  /// <param name="slot">слот</param>
  /// <param name="productId">расходник</param>
  /// <returns></returns>
  public static bool CanProduce(this BuildingSlot slot, string productId)
  {
    var palyerData = Global.Instance.PlayerData;

    if (palyerData == null) // Ќет пллера
    {
      DebugX.LogForUtils($"SlotUtils : CanProduce : palyerData == null : FALSE");
      return false;
    }

    if (!slot.IsEmpty) // слот не пустой
    {
      DebugX.LogForUtils($"SlotUtils : CanProduce : !slot.State.Equals(EProduceSlotState.Empty) = {slot.State} : FALSE");
      return false;
    }

    var product = ProjectUtils.GetProductDataById(productId);

    if (product == null) // не найден продукт
    {
      DebugX.LogForUtils($"SlotUtils : CanProduce : product == null : FALSE");
      return false;
    }

    if (product.Receipt.Any())
    {
      foreach (var cons in product.Receipt)
      {
        if (!ProjectUtils.CheckConsumeOnPlayerSlots(cons)) // не хватает продукта
        {
          DebugX.LogForUtils($"SlotUtils : CanProduce : Consume not enought : FALSE");
          return false;
        }
      }
    }

    return true;
  }

  // ===================================================================================================
  /// <summary>
  /// «апуск производства
  /// </summary>
  /// <param name="slot">слот</param>
  /// <param name="callBackEnd"></param>
  /// <param name="callBackFail"></param>
  public static void Produce(this BuildingSlot slot, string productId, Action callBackEnd = null, Action callBackFail = null)
  {
    if ((slot.Building.BuildData.Produce != null) &&
         slot.Building.BuildData.Produce.Any() &&
        !slot.CanProduce(productId))
    {
      DebugX.LogForUtils($"SlotUtils : TryProduce : FALSE", error: true);
      callBackFail?.Invoke();
      return;
    }

    var product = ProjectUtils.GetProductDataById(productId);

    if (product == null)
    {
      DebugX.LogForUtils($"SlotUtils : TryProduce : product == null : FALSE", error: true);
      callBackFail?.Invoke();
      return;
    }

    if (product.Receipt.Any())
    {
      foreach (var cons in product.Receipt)
      {
        ProjectUtils.RemoveConsumeFromPlayerSlot(cons);
      }
    }

    slot.NeedConsume = product.Receipt.Any() ? product.Receipt[0] : null;
    slot.FinishConsume = new Consume(product.Id, 1);
    slot.CompleteTimer.Init(product.ProductionTime);
    slot.CompleteTimer.Start();

    slot.UpdateSlotState();
    DebugX.LogForUtils($"SlotUtils : TryProduce : TRUE");
    callBackEnd?.Invoke();
  }

  // ===================================================================================================
  public static void Complete(this BuildingSlot slot, Action callBackEnd = null, Action callBackFail = null)
  {
    if (!slot.State.Equals(EProduceSlotState.Complete))
    {
      DebugX.LogForUtils($"SlotUtils : Complete : !slot.State.Equals(EProduceSlotState.Complete) : FALSE", error: true);
      callBackFail?.Invoke();
      return;
    }

    var playerSlot = ProjectUtils.GetEmptyPlayerSlot();

    if (playerSlot == null)
    {
      DebugX.LogForUtils($"SlotUtils : Complete : playerSlot == null : FALSE", error: true);
      callBackFail?.Invoke();
      return;
    }

    playerSlot.WaitConsume = true;

    ProjectUtils.AddConsume(slot.FinishConsume);

    slot.Clear();
    slot.UpdateSlotState();

    callBackEnd?.Invoke();
  }

  #endregion BuildingSlot
}