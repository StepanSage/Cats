using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Static;
using UnityEngine.UI;

public class DialogBattleEnd : Dialog
{
  [SerializeField]
  private GameObject _contentWin;
  [SerializeField]
  private GameObject _contentLoose;
  [SerializeField]
  private Transform _rewardSlot;

  [Space(10f)]
  [SerializeField]
  private Button _btnGetReward;
  [SerializeField]
  private Button _btnGetAdsReward;
  [SerializeField]
  private Button _btnClose;

  [Space(10f)]
  [SerializeField]
  private RewardElement _rewardPrefab;
  [SerializeField]
  private Transform _rewardsContent;
  [SerializeField]
  private Transform _rewardsAdsContent;

  private List<RewardElement> _rewardItems = new List<RewardElement>();
  private List<RewardElement> _rewardAdsItems = new List<RewardElement>();

  private Action<Vector3> _onClickWin = null;
  private Action _onClickLoose = null;

  // ===================================================================================================
  private void Awake()
  {
    _btnGetReward.onClick.AddListener(BtnRewardClick);
    _btnGetAdsReward.onClick.AddListener(BtnAdsRewardClick);
    _btnClose.onClick.AddListener(BtnCloseClick);
  }

  // ===================================================================================================
  private void OnDestroy()
  {
    _btnGetReward.onClick.RemoveAllListeners();
    _btnGetAdsReward.onClick.RemoveAllListeners();
    _btnClose.onClick.RemoveAllListeners();
  }

  // ===================================================================================================
  public void Activate(Action<Vector3> onClickWin, Action onClickLoose)
  {
    _onClickWin = onClickWin;
    _onClickLoose = onClickLoose;

    Redraw();
    Open();
  }

  // ===================================================================================================
  private void Redraw()
  {
    var isWin = Global.Instance.Game.Battle.IsWin;

    Global.Instance.SoundService.PlayOne(isWin? "win" : "loose");

    _contentWin.SetActive(isWin);
    _contentLoose.SetActive(!isWin);

    List<Consume> rewards = new List<Consume>();


    foreach (var prod in _rewardItems)
    {
      prod.gameObject.SetActive(false);
    }

    if (isWin)
    {
      var list = OrderUtils.GetOrdersCompleteRewards();

      if (list != null)
      {
        rewards.AddRange(list);
      }
    }

    var ratingPoint = ProjectUtils.GetRatingPoint(isWin);

    if (ratingPoint != null)
    {
      rewards.Add(ratingPoint);
    }

    int cacheEC = _rewardItems.Count;
    int needCount = rewards.Count();

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_rewardPrefab, _rewardsContent);
        _rewardItems.Add(elem);
      }
    }

    int ij = 0;
    foreach (var o in rewards)
    {
      _rewardItems[ij].Init(o);
      ij++;
    }

    _rewardsContent.UpdateLayout();
  }

  // ===================================================================================================
  public void BtnRewardClick()
  {
    _onClickWin?.Invoke(_rewardSlot.position);
    Close();
  }

  // ===================================================================================================
  public void BtnAdsRewardClick()
  {
    _onClickWin?.Invoke(_rewardSlot.position);
    Close();
  }

  // ===================================================================================================
  public void BtnCloseClick()
  {
    _onClickLoose?.Invoke();
    Close();
  }
}