using YG;

public class AdsYandex : AdsBase
{
  // ===================================================================================================
  public override void Init()
  {
    YandexGame.RewardVideoEvent += Rewarded;
  }

  // ===================================================================================================
  public override void Rewarded(int type)
  {
    try
    {
      var adsRewardType = (AdsRewardType) type;

      if (Rewards.TryGetValue(adsRewardType, out var adsReward))
      {
        adsReward.Invoke();
      }

    }
    catch { }
  }
}
