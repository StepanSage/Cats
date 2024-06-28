using System;
using System.Collections.Generic;

public abstract class AdsBase
{
  public enum AdsRewardType
  {
    REWARD_BATTLE_END
  }

  public Dictionary<AdsRewardType, Action> Rewards = new Dictionary<AdsRewardType, Action>();

  // ===================================================================================================
  public virtual void Init() { }

  public virtual void Rewarded(int type) { }

  // ===================================================================================================
  public void AddRewardBattleEnd(Action callback)
  {
    if (Rewards.ContainsKey(AdsRewardType.REWARD_BATTLE_END))
      return;

    Rewards.Add(AdsRewardType.REWARD_BATTLE_END, callback);
  }

}
