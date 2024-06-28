using Static;
using System.Collections.Generic;
using static OrderUtils;

namespace Player
{
  // ===================================================================================================
  public enum EOrderState
  {
    InWaiting,
    InProgress,
    Complete,
    Skip
  }

  // ===================================================================================================
  public class Order
  {
    public int Index;
    public OrderSlot OrderSlot;
    public string Owner;
    public Timer Expire;
    public List<OrderProduct> Products = new List<OrderProduct>();
    public List<Consume> Rewards = new List<Consume>();

    public bool IsInWaiting => _state.Equals(EOrderState.InWaiting);
    public bool IsInProgress => _state.Equals(EOrderState.InProgress);
    public bool IsComplete => _state.Equals(EOrderState.Complete);
    public bool IsSkip => _state.Equals(EOrderState.Skip);

    private EOrderState _state;

    // ===================================================================================================
    private void SetState(EOrderState newState)
    {
      _state = newState;
    }

    // ===================================================================================================
    public void SetStateInWaiting()
    {
      SetState(EOrderState.InWaiting);
    }

    // ===================================================================================================
    public void SetStateInProgress()
    {
      SetState(EOrderState.InProgress);
      EventManager.Instance?.TriggerEvent(new E_OrderStartProgress(this));
    }

    // ===================================================================================================
    public void SetStateComplete()
    {
      SetState(EOrderState.Complete);

      DebugX.LogForView($"Order : SetStateComplete 1");

      EventManager.Instance?.TriggerEvent(new E_OrderComplete(this));
    }

    // ===================================================================================================
    public void SetStateSkip()
    {
      SetState(EOrderState.Skip);
      EventManager.Instance?.TriggerEvent(new E_OrderSkip(this));
    }
  }
}