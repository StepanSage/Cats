using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class DialogLevelProgress : Dialog
{
  [Space(10f)]
  [SerializeField]
  private LevelProgressElement _levelPrefab;
  [SerializeField]
  private Transform _levelsContent;

  private List<LevelProgressElement> _levelsItems = new List<LevelProgressElement>();

  // ===================================================================================================
  public void Activate()
  {
    Redraw();
    Open();
  }

  // ===================================================================================================
  private void Redraw()
  {
    var levels = Global.Instance.StaticData.PlayerLevelDatas;

    foreach (var prod in _levelsItems)
    {
      prod.gameObject.SetActive(false);
    }

    int cacheEC = _levelsItems.Count;
    int needCount = levels.Count();

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_levelPrefab, _levelsContent);
        _levelsItems.Add(elem);
      }
    }

    int ij = 0;
    foreach (var level in levels)
    {
      _levelsItems[ij].Init(level);
      ij++;
    }
  }
}