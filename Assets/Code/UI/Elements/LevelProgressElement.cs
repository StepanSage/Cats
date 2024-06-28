using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Static;

public class LevelProgressElement : MonoBehaviour
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

  private PlayerLevelData _playerLevelData;

  // ===================================================================================================
  public void Init(PlayerLevelData playerLevelData)
  {
    _playerLevelData = playerLevelData;
    Redraw();
    gameObject.SetActive(true);
  }

  // ===================================================================================================
  public void Redraw()
  {
    DebugX.LogForUI($"Level = {_playerLevelData.Level} /// Experience = {_playerLevelData.ExpForLevel}");
    
    _level.text = _playerLevelData.Level.ToString();

    var isActiveLevel = PlayerUtils.Data.Level.Equals(_playerLevelData.Level);
    var isOldLevel = PlayerUtils.Data.Level > _playerLevelData.Level;

    _oldLevel.SetActive(false);
    _activeLevel.SetActive(false);
    _newLevel.SetActive(false);

    if (isActiveLevel)
    {
      _activeLevel.SetActive(true);

      var exp = PlayerUtils.Data.Experience;
      var expMax = PlayerUtils.ExperienceMax;

      _progress.fillAmount = exp * 1f / expMax;
      _count.text = $"{exp}/{expMax}";
    }
    else if(isOldLevel)
    {
      _oldLevel.SetActive(true);
      _progress.fillAmount = 1f;
      _count.text = "";
    }
    else
    {
      _newLevel.SetActive(true);
      _progress.fillAmount = 0f;
      _count.text = "";
    }

    var rewards = _playerLevelData.Rewards;

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
}
