using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Static;
using System;

public class RatingLevelProgressElement : MonoBehaviour
{
  [Space(10)]
  [Header("Level")]
  [SerializeField]
  private TextMeshProUGUI _level;
  [SerializeField]
  private GameObject _oldLevel;
  [SerializeField]
  private GameObject _activeLevel;
  [SerializeField]
  private GameObject _newLevel;
  [SerializeField]
  private AnimationButton _animationButton;

  [Space(10)]
  [Header("Progress")]
  [SerializeField]
  private TextMeshProUGUI _count;
  [SerializeField]
  private Image _progress;

  [Space(10f)]
  [SerializeField]
  private RewardElement _rewardPrefab;
  [SerializeField]
  private Transform _rewardContent;

  private List<RewardElement> _rewardItems = new List<RewardElement>();

  private PlayerRatingLevelData _playerRatingLevelData;
  private bool _canGetReward = false;
  private Action<Transform> _getRewardClick;

  // ===================================================================================================
  public void Init(PlayerRatingLevelData playerLevelData, Action<Transform> getRewardClick)
  {
    _playerRatingLevelData = playerLevelData;
    _getRewardClick = getRewardClick;
    Redraw();
    gameObject.SetActive(true);
  }

  // ===================================================================================================
  public void Redraw()
  {
    _level.text = _playerRatingLevelData.RatingLevel.ToString();

    _canGetReward = (PlayerUtils.Data.RatingLevel >= _playerRatingLevelData.RatingLevel) &&
                    (PlayerUtils.Data.RatingLevelGetReward < _playerRatingLevelData.RatingLevel);

    var alreadyGetReward = (PlayerUtils.Data.RatingLevel >= _playerRatingLevelData.RatingLevel) &&
                           (PlayerUtils.Data.RatingLevelGetReward >= _playerRatingLevelData.RatingLevel);

    var nextLevel = (PlayerUtils.Data.RatingLevel < _playerRatingLevelData.RatingLevel);

    var isActiveLevel = PlayerUtils.Data.RatingLevel.Equals(_playerRatingLevelData.RatingLevel);
    var isOldLevel = PlayerUtils.Data.RatingLevel > _playerRatingLevelData.RatingLevel;

    DebugX.LogForUI($"Redraw {PlayerUtils.Data.RatingLevel} = {_playerRatingLevelData.RatingLevel}");

    _oldLevel.SetActive(alreadyGetReward);
    _newLevel.SetActive(nextLevel);
    
    _animationButton.enabled = _canGetReward;
    _activeLevel.SetActive(_canGetReward);

    if (isActiveLevel)
    {
      var ratingPoint = PlayerUtils.Data.RatingPoint;
      var ratingPointMax = PlayerUtils.RatingPointMax;

      _progress.fillAmount = ratingPoint * 1f / ratingPointMax;
      _count.text = $"{ratingPoint}/{ratingPointMax}";
    }
    else if(isOldLevel)
    {
      _progress.fillAmount = 1f;
      _count.text = "";
    }
    else
    {
      _progress.fillAmount = 0f;
      _count.text = "";
    }

    var rewards = _playerRatingLevelData.Rewards;

    int cacheEC = _rewardItems.Count;
    int needCount = rewards.Count();

    foreach (var prod in _rewardItems)
    {
      prod.gameObject.SetActive(false);
    }

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_rewardPrefab, _rewardContent);
        _rewardItems.Add(elem);
      }
    }

    int ij = 0;

    foreach (var prod in rewards)
    {
      _rewardItems[ij].Init(prod);
      ij++;
    }
  }

  // ===================================================================================================
  public void GetRatingreward_Click()
  {
    if (!_canGetReward)
      return;    
    
    _getRewardClick?.Invoke(transform);
  }
}
