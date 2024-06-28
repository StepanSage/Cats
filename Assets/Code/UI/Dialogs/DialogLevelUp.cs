using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Static;
using System;

public class DialogLevelUp : Dialog
{
  [SerializeField]
  private TextMeshProUGUI _level;
  [SerializeField]
  private Transform _rewardSlot;

  [Space(10f)]
  [SerializeField]
  private RewardElement _rewardPrefab;
  [SerializeField]
  private Transform _rewardsContent;

  private List<RewardElement> _rewardItems = new List<RewardElement>();
  private List<Consume> _rewards;

  private Action _onClickWin = null;
  private int _newLevel;

  // ===================================================================================================
  public void Activate(int newLevel, List<Consume> rewards, Action onClickWin)
  {
    _rewards = rewards;
    _newLevel = newLevel;
    Redraw();
    Open();
  }

  // ===================================================================================================
  private void Redraw()
  {
    _level.text = _newLevel.ToString();


    foreach (var prod in _rewardItems)
    {
      prod.gameObject.SetActive(false);
    }

    int cacheEC = _rewardItems.Count;
    int needCount = _rewards.Count();

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_rewardPrefab, _rewardsContent);
        _rewardItems.Add(elem);
      }
    }

    int ij = 0;
    foreach (var o in _rewards)
    {
      _rewardItems[ij].Init(o);
      ij++;
    }
  }

  // ===================================================================================================
  public void BtnGetReward_Click()
  {
    ProjectUtils.AnimatedReward(_rewards, _rewardSlot.position, Vector3.negativeInfinity, false);
    _onClickWin?.Invoke();
    Close();
  }
}