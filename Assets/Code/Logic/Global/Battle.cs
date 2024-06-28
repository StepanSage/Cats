using UnityEngine;

namespace GameLogic
{
  // ===================================================================================================
  public enum EBattleRule
  {
    CompleteOrders,
    CompleteOrdersWithTime,
    CollectGoldAmount,
    CollectGoldAmountWithTime
  }

  // ===================================================================================================
  public enum EBattleState
  {
    Start,
    InProgress,
    Win,
    Loose
  }

  // ===================================================================================================
  public class Battle
  {
    public OrderSystem OrderSystem;

    public EBattleState BattleState { get; private set; }
    public EBattleRule BattleRule { get; private set; }
    public int Amount { get; private set; }
    public Timer Expire { get; private set; }
    public bool RewardWithAds { get; private set; }

    public bool IsStart => BattleState.Equals(EBattleState.Start);
    public bool IsInProgress => BattleState.Equals(EBattleState.InProgress);
    public bool IsWin => BattleState.Equals(EBattleState.Win);
    public bool IsLoose => BattleState.Equals(EBattleState.Loose);

    public bool IsBattleRuleCompleteOrders => BattleRule.Equals(EBattleRule.CompleteOrders);
    public bool IsBattleRuleCompleteOrdersWithTime => BattleRule.Equals(EBattleRule.CompleteOrdersWithTime);
    public bool IsBattleRuleCollectGoldAmount => BattleRule.Equals(EBattleRule.CollectGoldAmount);
    public bool IsBattleRuleCollectGoldAmountWithTime => BattleRule.Equals(EBattleRule.CollectGoldAmountWithTime);

    // ===================================================================================================
    private void Init(EBattleRule battleRule, int amountModifier, int durationModifier)
    {
      if (!EventManager.Instance.HasListener<E_OrdersUpdate>(OnOrdersUpdate))
      {
        EventManager.Instance?.AddListener<E_OrdersUpdate>(OnOrdersUpdate);
      }

      if (OrderSystem == null)
      {
        OrderSystem = new OrderSystem();
      }

      BattleRule = battleRule;

      OrderSystem.Start();

      var isOrder = BattleRule.Equals(EBattleRule.CompleteOrders) || BattleRule.Equals(EBattleRule.CompleteOrdersWithTime);
      
      Amount = Mathf.RoundToInt(amountModifier * (isOrder ? OrderUtils.GetAllOrdersCount() : OrderUtils.GetAllOrdersRewardGold()));

      var withTimer = BattleRule.Equals(EBattleRule.CollectGoldAmountWithTime) || BattleRule.Equals(EBattleRule.CompleteOrdersWithTime);
      
      if (withTimer)
      {
        var duration = Mathf.RoundToInt(durationModifier * OrderUtils.GetAllOrdersDuration());
        Expire = new Timer(duration);
        Expire.Start();
        Global.Instance.Game.OneSecondTimer.Update += OnSecondUpdate;
      }

      PlayerUtils.InitBattleSlots();

      BattleState = EBattleState.InProgress;
    }

    // ===================================================================================================
    ~Battle()
    {
      EventManager.Instance?.RemoveListener<E_OrdersUpdate>(OnOrdersUpdate);
      Global.Instance.Game.OneSecondTimer.Update -= OnSecondUpdate;
    }

    // ===================================================================================================
    private void OnSecondUpdate()
    {
      if (!IsInProgress)
        return;

      if (Expire.IsExpired())
      {
        CheckBattleState();
      }
    }

    // ===================================================================================================
    private void OnOrdersUpdate(E_OrdersUpdate evt)
    {
      if (!IsInProgress)
        return;

      CheckBattleState();
    }

    // ===================================================================================================
    private void CheckBattleState()
    {
      if (!IsInProgress)
        return;

      if (IsBattleRuleCompleteOrders || IsBattleRuleCompleteOrdersWithTime)
      {
        DebugX.LogForBattle($"CheckBattleState : IsBattleRuleCompleteOrders || IsBattleRuleCompleteOrdersWithTime");
        DebugX.LogForBattle($"{Amount}<={OrderUtils.GetOrdersCompleteCount()}  /// ");
        DebugX.LogForBattle($"{Amount} > {OrderUtils.GetAllOrdersCount() - OrderUtils.GetOrdersSkipCount()}");

        if (Amount <= OrderUtils.GetOrdersCompleteCount())
        {
          //Если выполнили достаточно коилество заказов, то побеждаем
          SetBattleState(EBattleState.Win);
        }
        else if (Amount > (OrderUtils.GetAllOrdersCount() - OrderUtils.GetOrdersSkipCount()))
        {
          //Разница между всеми заказама и пропущенными меньше, чем необходиомоет количетсво выполненных заказов, то проигрываем
          SetBattleState(EBattleState.Loose);
        }
      }

      if (IsBattleRuleCollectGoldAmount || IsBattleRuleCollectGoldAmountWithTime)
      {
        DebugX.LogForBattle($"CheckBattleState : IsBattleRuleCollectGoldAmount || IsBattleRuleCollectGoldAmountWithTime");

        if (Amount <= OrderUtils.GetOrdersCompleteRewardGold())
        {
          //Если выполнили достаточно коилество золота, то побеждаем
          SetBattleState(EBattleState.Win);
        }
        else if (Amount > (OrderUtils.GetAllOrdersRewardGold() - OrderUtils.GetOrdersSkipRewardGold()))
        {
          //Разница золота между всеми заказами и пропущенными меньше, чем необходиомоет количетсвозолота выполненных заказов, то проигрываем
          SetBattleState(EBattleState.Loose);
        }
      }

      if (IsBattleRuleCompleteOrdersWithTime || IsBattleRuleCollectGoldAmountWithTime)
      {
        DebugX.LogForBattle($"CheckBattleState : IsBattleRuleCompleteOrdersWithTime || IsBattleRuleCollectGoldAmountWithTime");

        if (Expire.IsExpired())
        {
          //Если время прошло, но проигрываем
          SetBattleState(EBattleState.Loose);
        }
      }
    }

    // ===================================================================================================
    private void SetBattleState(EBattleState newState)
    {
      BattleState = newState;

      DebugX.LogForBattle($"SetBattleState : {BattleState}");

      if (IsWin || IsLoose)
      {
        EventManager.Instance?.TriggerEvent(new E_BattleEnd());
      }
    }

    // ===================================================================================================
    public void StartCompleteOrders()
    {
      Init(EBattleRule.CompleteOrders, 1, 1);
    }

    // ===================================================================================================
    public void StartCompleteOrdersWithTime()
    {
      Init(EBattleRule.CompleteOrdersWithTime, 1, 1);
    }

    // ===================================================================================================
    public void StartCollectGoldAmount()
    {
      Init(EBattleRule.CollectGoldAmount, 1, 1);
    }

    // ===================================================================================================
    public void StartCollectGoldAmountWithTime()
    {
      Init(EBattleRule.CollectGoldAmountWithTime, 1, 1);
    }
  }
}
