using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GameLogic;

public class PlayerSlotsUI : MonoBehaviour
{
  [Space(10f)]
  [SerializeField]
  private PlayerSlotElement _slotPrefab;
  [SerializeField]
  private Transform _slotsContent;

  private List<PlayerSlotElement> _playerSlotItems = new List<PlayerSlotElement>();

  // ===================================================================================================
  public void Start()
  {
    EventManager.Instance?.AddListener<E_PlayerSlotUpdate>(OnPlayerSlotUpdate);
  }

  // ===================================================================================================
  public void OnDestroy()
  {
    EventManager.Instance?.RemoveListener<E_PlayerSlotUpdate>(OnPlayerSlotUpdate);
  }

  // ===================================================================================================
  public void OnPlayerSlotUpdate(E_PlayerSlotUpdate evt)
  {
    DebugX.LogForEvents($"E_PlayerSlotUpdate : PlayerSlotsUI : OnPlayerSlotUpdate");
    Redraw();
  }

  // ===================================================================================================
  public void Init()
  {
    Redraw();
  }

  // ===================================================================================================
  public void Redraw()
  {
    DebugX.LogForUI("PlayerSlotsUI Redraw Start");

    foreach (var playerSlot in _playerSlotItems)
    {
      playerSlot.gameObject.SetActive(false);
    }

    if (!Global.Instance.Game.IsStateBattle)
      return;

    var slots = PlayerUtils.BattleSlots.GetBattleSlots();

    if (slots == null)
      return;

    DebugX.LogForUI($"PlayerSlotsUI slots {JsonConvert.SerializeObject(slots)}");


    int cacheEC = _playerSlotItems.Count;
    int needCount = slots.Count();

    if (cacheEC < needCount)
    {
      for (int i = 0; i < (needCount - cacheEC); i++)
      {
        var elem = Instantiate(_slotPrefab, _slotsContent);
        _playerSlotItems.Add(elem);
      }
    }

    int ij = 0;
    foreach (var o in slots)
    {
      _playerSlotItems[ij].Init(o);
      ij++;
    }

    DebugX.LogForUI("PlayerSlotsUI Redraw End");
  }
}
