using GameLogic;
using UnityEngine;

public class Map : MonoBehaviour
{
  [Header("Lobby")]
  [SerializeField]
  private GameObject _lobby;

  public Transform LobbyMapSlot;
  [HideInInspector]
  public LevelView LobbyLevelView;

  [Space (10)]
  [Header("Battle")]
  [SerializeField]
  private GameObject _battle;

  public Transform BattleMapSlot;
  [HideInInspector]
  public LevelView BattleLevelView;

  [Space(10)]
  [Header("PreBattle")]
  [SerializeField]
  private GameObject _preBattle;
  public Transform PreBattleMapSlot;
  [HideInInspector]
  public LevelView PreBattleLevelView;

  // ===================================================================================================
  public void GameStateChange(EGameState newState)
  {
    DebugX.LogForBattle($"GameStateChange {newState}");
    
    _lobby.SetActive(false);
    _battle.SetActive(false);
    _preBattle.SetActive(false);

    if (newState.Equals(EGameState.Lobby))
    {
      _lobby.SetActive(true);
    }
    else if (newState.Equals(EGameState.Battle))
    {
      _battle.SetActive(true);
    }
    else if(newState.Equals(EGameState.BattleEnd))
    {
      _lobby.SetActive(true);
    }
    else if (newState.Equals(EGameState.PreBattle))
    {
      _preBattle.SetActive(true);
    }
  }
}
