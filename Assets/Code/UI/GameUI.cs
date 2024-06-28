using GameLogic;
using UnityEngine;
using static ObjectPoolManager;

public class GameUI : MonoBehaviour
{
  public LobbyMainUI LobbyMainUI;
  public PreBattleMainUI PreBattleMainUI;
  public BattleMainUI BattleMainUI;
  public GameObject LoadingBg;

  [Space(10)]
  public Transform DefaultRewardslot;

  [Space(10)]
  public Vector2 RefferenceResolution = new Vector2(800, 600);

  [Space(10)]
  [Header("GameObjectPoolUI")]
  [SerializeField]
  private Transform _gameObjectPoolUI;

  // ===================================================================================================
  public void Start()
  {
    UIUtils.SetGetter("default", DefaultRewardslot);
    AddPoll(EObjectPoolType.UI, _gameObjectPoolUI, "dropped_ui_item");
    //AddPoll(EObjectPoolType.Debug, _gameObjectPoolUI, "debug_building_indicator");
    LoadingBg.SetActive(true);
  }

  // ===================================================================================================
  public void GameStateChange(EGameState newState)
  {
    LobbyMainUI.gameObject.SetActive(false);
    BattleMainUI.gameObject.SetActive(false);
    PreBattleMainUI.gameObject.SetActive(false);

    if (newState.Equals(EGameState.Lobby))
    {
      LobbyMainUI.gameObject.SetActive(true);
      LobbyMainUI.Init();
    }
    else if (newState.Equals(EGameState.Battle))
    {
      BattleMainUI.gameObject.SetActive(true);
      BattleMainUI.Init();
    }
    else if(newState.Equals(EGameState.PreBattle))
    {
      PreBattleMainUI.gameObject.SetActive(true);
      PreBattleMainUI.Init();
    }
  }
}

