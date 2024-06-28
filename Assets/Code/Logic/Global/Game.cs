using Code.Service;
using Extension;
using Objects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
  public enum EGameState
  {
    Start,
    Lobby,
    PreBattle,
    Battle,
    BattleEnd
  }

  public enum EGameEndReason
  {
    AllOrdersComplete
  }

  public class Game : MonoBehaviour
  {
    public Map Map;
    public GameUI GameUI;
    public GameInput GameInput;
    public CameraController CameraController;

    public Transform InventorySlot;

    [HideInInspector]
    public Battle Battle = new Battle();

    [HideInInspector]
    public PlayerController Player;

    public Timer OneSecondTimer;
    public Timer Timer;

    public List<BuildingBehaviour> BuildingBehaviours = new List<BuildingBehaviour>();

    public bool IsStateLobby => _gameState.Equals(EGameState.Lobby);
    public bool IsStatePreBattle => _gameState.Equals(EGameState.PreBattle);
    public bool IsStateBattle => _gameState.Equals(EGameState.Battle);

    public ShopController ShopController;

    private EGameState _gameState;

    // ===================================================================================================
    private void Start()
    {
      Global.Instance.Game = this;

      OneSecondTimer = new Timer(1, true);
      Timer = new Timer(0, true);
      OneSecondTimer.Start();
      Timer.Start();

      EventManager.Instance?.AddListener<E_ExpUpdate>(OnExpUpdate);
      EventManager.Instance?.AddListener<E_RatingPointUpdate>(OnRatingPointUpdate);
      EventManager.Instance?.AddListener<E_BattleEnd>(OnBattleEnd);

      Invoke(nameof(StartGame), 1f);
    }

    // ===================================================================================================
    private void OnExpUpdate(E_ExpUpdate evt)
    {
      CheckNewLevel();
    }

    // ===================================================================================================
    private void OnRatingPointUpdate(E_RatingPointUpdate evt)
    {
      CheckNewRatingLevel();
    }

    // ===================================================================================================
    public void CheckNewLevel()
    {
      DebugX.LogForBehaviour($"Game : CheckNewLevel");

      var playerData = Global.Instance.PlayerData;

      if (playerData == null)
        return;

      var level = playerData.Level;
      var exp = playerData.Experience;
      var expMax = PlayerUtils.ExperienceMax;

      if (expMax <= exp)
      {
        var levelData = Global.Instance.StaticData.PlayerLevelDatas.FirstOrDefault(x => x.Level.Equals(level));
        var nextLevelData = Global.Instance.StaticData.PlayerLevelDatas.FirstOrDefault(x => x.Level.Equals(level + 1));

        if (nextLevelData != null)
        {
          playerData.Level++;
          playerData.Experience -= expMax;
        }
        else
        {
          playerData.Experience -= expMax;
        }

        var rewards = levelData.Rewards.Clone();
        EventManager.Instance?.TriggerEvent(new E_NewLevelGet(playerData.Level));

        DialogSystem.Instance.Get<DialogLevelUp>()?.Activate(playerData.Level, rewards, CheckNewLevel);
        DataStoreManager.Instance.DataSave();
      }
    }

    // ===================================================================================================
    public void CheckNewRatingLevel()
    {
      var playerData = Global.Instance.PlayerData;

      if (playerData == null)
        return;

      var ratingLevel = playerData.RatingLevel;
      var ratingPoint = playerData.RatingPoint;
      var ratingPointMax = PlayerUtils.RatingPointMax;

      if (ratingPointMax <= ratingPoint)
      {
        var levelData = Global.Instance.StaticData.PlayerRatingLevelDatas.FirstOrDefault(x => x.RatingLevel.Equals(ratingLevel));
        var nextLevelData = Global.Instance.StaticData.PlayerRatingLevelDatas.FirstOrDefault(x => x.RatingLevel.Equals(ratingLevel + 1));

        if (nextLevelData != null)
        {
          playerData.RatingLevel++;
          playerData.RatingPoint -= ratingPointMax;
        }
        else
        {
          playerData.RatingPoint -= ratingPointMax;
        }

        var rewards = levelData.Rewards.Clone();
        EventManager.Instance?.TriggerEvent(new E_RatingNewLevelGet(playerData.RatingLevel));
        DataStoreManager.Instance.DataSave();
      }
    }

    // ===================================================================================================
    public void OnBattleEnd(E_BattleEnd evt)
    {
      StartBattleEndState();
    }

    #region GameStates
    // ===================================================================================================
    private void ChangeGameState(EGameState gameState)
    {
      if (_gameState.Equals(gameState))
        return;

      var oldGameState = _gameState;
      _gameState = gameState;

      //TODO почему то оваливатеся EventManager
      if (EventManager.Instance == null)
      {
        DebugX.LogForBehaviour($"EventManager.Instance == null");
      }

      PrepairNewGameState();

      DebugX.LogForBehaviour($"ChangeGameState {_gameState}");
    }

    // ===================================================================================================
    public void PrepairNewGameState()
    {
      if (_gameState.Equals(EGameState.Battle))
      {
        DebugX.LogForBehaviour($"OnGameStateChange Battle");
        LoadBattle();
      }
      else if (_gameState.Equals(EGameState.Lobby))
      {
        DebugX.LogForBehaviour($"OnGameStateChange LoadLobby");
        LoadLobby();
      }
      else if (_gameState.Equals(EGameState.PreBattle))
      {
        DebugX.LogForBehaviour($"OnGameStateChange LoadPreBattle");
        LoadPreBattle();
      }
      else if (_gameState.Equals(EGameState.BattleEnd))
      {
        DebugX.LogForBehaviour($"OnGameStateChange LoadBattleEnd");
        Invoke(nameof(LoadBattleEnd), 1.5f);
      }
    }

    // ===================================================================================================
    /// <summary>
    /// Включить бой
    /// </summary>
    public void StartBattleState()
    {
      ChangeGameState(EGameState.Battle);
    }

    // ===================================================================================================
    /// <summary>
    /// Включить лобби
    /// </summary>
    public void StartLobbyState()
    {
      ChangeGameState(EGameState.Lobby);
    }

    // ===================================================================================================
    /// <summary>
    /// Включить прераунд
    /// </summary>
    public void StartPreBattleState()
    {
      ChangeGameState(EGameState.PreBattle);
    }

    // ===================================================================================================
    /// <summary>
    /// Включить прераунд
    /// </summary>
    public void StartBattleEndState()
    {
      ChangeGameState(EGameState.BattleEnd);
    }
    #endregion GameStates

    // ===================================================================================================
    public void StartGame()
    {
      Global.Instance.Game.GameUI.LoadingBg.SetActive(false);
      ChangeGameState(EGameState.Lobby);
    }
    // ===================================================================================================
    public void OnGameStateChange(E_GameStateChange evt)
    {
      if (evt.NewState.Equals(EGameState.Battle))
      {
        LoadBattle();
      }
      else if (evt.NewState.Equals(EGameState.Lobby))
      {
        DebugX.LogForBehaviour($"OnGameStateChange LoadLobby");
        LoadLobby();
      }
      else if (evt.NewState.Equals(EGameState.PreBattle))
      {
        LoadPreBattle();
      }
      else if (evt.NewState.Equals(EGameState.BattleEnd))
      {
        LoadBattleEnd();
      }
    }

    #region PreBattle
    // ===================================================================================================
    private void LevelPreBattleClearn()
    {
      DebugX.LogForBehaviour("LoadPreLobby");
      if (Global.Instance.Game.Map.PreBattleMapSlot != null)
      {

      }
    }

    // ===================================================================================================
    private void LoadPreBattle()
    {
      LevelPreBattleClear();
      LevelPreBattleLoad();

      Global.Instance.Game.Map.GameStateChange(_gameState);
      Global.Instance.Game.GameUI.GameStateChange(_gameState);

      SetCameraToPreBattle();
    }

    // ===================================================================================================
    private void LevelPreBattleClear()
    {
      if (Global.Instance.Game.Map.PreBattleLevelView != null)
      {
        Destroy(Global.Instance.Game.Map.PreBattleLevelView.gameObject);
      }
    }

    // ===================================================================================================
    private void LevelPreBattleLoad(string namePreBattle = "PreBattle")
    {
      Global.Instance.Game.Map.PreBattleLevelView = LoadLevel(namePreBattle, Global.Instance.Game.Map.PreBattleMapSlot);
    }

    // ===================================================================================================
    private void SetCameraToPreBattle()
    {
      if (Global.Instance.Game.Map.PreBattleLevelView.CameraSlot == null)
      {
        Debug.LogError($"CameraSlot is NULL!!!");
      }

      Global.Instance.Game.CameraController.SetSlot(Global.Instance.Game.Map.PreBattleLevelView.CameraSlot);
    }
    #endregion PreBattle

    #region BattleEnd
    // ===================================================================================================
    private void LoadBattleEnd()
    {
      DialogSystem.Instance.CloseAll(true);

      DebugX.LogForBehaviour($"BattleEnd");

      Global.Instance.SoundService.OffSound();

      if (Global.Instance.Game.Map.LobbyLevelView == null)
      {
        LevelBattleEndLoad();
      }

      Global.Instance.Game.Map.GameStateChange(_gameState);
      Global.Instance.Game.GameUI.GameStateChange(_gameState);

      LoadPlayerToLevel(Global.Instance.Game.Map.LobbyLevelView);
      SetCameraToLobby();
      //StartLobbyState();

      DialogSystem.Instance.Get<DialogBattleEnd>()?.Activate(DialogBattleEndWinClick, DialogBattleEndLooseClick);
    }

    // ===================================================================================================
    private void DialogBattleEndWinClick(Vector3 pos)
    {
      var rewards = OrderUtils.GetOrdersCompleteRewards().Clone();
      ProjectUtils.AnimatedReward(rewards, pos, Vector3.negativeInfinity, false);
      Global.Instance.Game.StartLobbyState();
    }

    // ===================================================================================================
    private void DialogBattleEndLooseClick()
    {
      Global.Instance.Game.StartLobbyState();
    }

    // ===================================================================================================
    private void LevelBattleEndLoad(string levelName = "Lobby")
    {
      DebugX.LogForBehaviour($"LevelBattleEndLoad");

      if (Global.Instance.Game.Map.LobbyLevelView == null)
      {
        Global.Instance.Game.Map.LobbyLevelView = LoadLevel(levelName, Global.Instance.Game.Map.LobbyMapSlot);
      }
    }

    // ===================================================================================================
    private void SetCameraToBattleEnd()
    {
      if (Global.Instance.Game.Map.LobbyLevelView.CameraSlot == null)
      {
        Debug.LogError($"CameraSlot is NULL!!!");
      }

      Global.Instance.Game.CameraController.SetSlot(Global.Instance.Game.Map.LobbyLevelView.CameraSlot);
    }
    #endregion BattleEnd

    #region Lobby
    // ===================================================================================================
    private void LoadLobby()
    {
      DebugX.LogForBehaviour($"LoadLobby");

      if (Global.Instance.Game.Map.LobbyLevelView == null)
      {
        LevelLobbyLoad();
      }

      Global.Instance.Game.Map.GameStateChange(_gameState);
      Global.Instance.Game.GameUI.GameStateChange(_gameState);

      LoadPlayerToLevel(Global.Instance.Game.Map.LobbyLevelView);
      SetCameraToLobby();
    }

    // ===================================================================================================
    private void LevelLobbyLoad(string levelName = "Lobby")
    {
      DebugX.LogForBehaviour($"LevelLobbyLoad");
      Global.Instance.Game.Map.LobbyLevelView = LoadLevel(levelName, Global.Instance.Game.Map.LobbyMapSlot);
    }

    // ===================================================================================================
    private void SetCameraToLobby()
    {
      if (Global.Instance.Game.Map.LobbyLevelView.CameraSlot == null)
      {
        Debug.LogError($"CameraSlot is NULL!!!");
      }

      Global.Instance.Game.CameraController.SetSlot(Global.Instance.Game.Map.LobbyLevelView.CameraSlot);
    }
    #endregion Lobby

    #region Battle
    // ===================================================================================================
    private void LoadBattle()
    {
      LevelBattleClear();
      LevelBattleLoad();

      Battle.StartCompleteOrders();

      Global.Instance.Game.Map.GameStateChange(_gameState);
      Global.Instance.Game.GameUI.GameStateChange(_gameState);

    }

    // ===================================================================================================
    private void LevelBattleLoad(string levelName = "Battle")
    {
      Global.Instance.Game.Map.BattleLevelView = LoadLevel(levelName, Global.Instance.Game.Map.BattleMapSlot);
      LoadPlayerToLevel(Global.Instance.Game.Map.BattleLevelView);
      SetCameraToBattle();
    }

    // ===================================================================================================
    private void SetCameraToBattle()
    {
      if (Global.Instance.Game.Map.BattleLevelView.CameraSlot == null)
      {
        Debug.LogError($"CameraSlot is NULL!!!");
      }

      Global.Instance.Game.CameraController.SetSlot(Global.Instance.Game.Map.BattleLevelView.CameraSlot);
    }

    // ===================================================================================================
    private void LevelBattleClear()
    {
      if (Global.Instance.Game.Map.BattleLevelView != null)
      {
        Destroy(Global.Instance.Game.Map.BattleLevelView.gameObject);
      }
    }

    #endregion Battle

    #region LoadCommon
    // ===================================================================================================
    /// <summary>
    /// Загрузка уровня
    /// </summary>
    /// <param name="levelName">ИД уровня</param>
    public LevelView LoadLevel(string levelName, Transform slot)
    {
      var level = Factory.Instance.GetLevel(levelName);

      if (level == null)
      {
        Debug.LogWarning($"level is NULL!!!");
        return null;
      }

      level.transform.SetParentWithParam(slot);
      

      var levelView = level.GetComponent<LevelView>();

      if (levelView == null)
      {
        Debug.LogWarning($"LevelView is NULL!!!");
        return null;
      }

      LoadBuildingToLevelSlots(levelView, levelName);

      return levelView;
    }

    // ===================================================================================================
    /// <summary>
    /// Загрузка игрока
    /// </summary>
    /// <param name="levelView">Вьюха уровня</param>
    public void LoadPlayerToLevel(LevelView levelView)
    {
      if (PlayerUtils.Controller == null)
      {
        PlayerUtils.InitPlayer();
      }

      PlayerUtils.Controller.Transform.SetParentWithParam(levelView.PlayerSpawnPoint);
    }

    // ===================================================================================================
    /// <summary>
    /// Заполнение слотов уровня
    /// </summary>
    /// <param name="levelView">Вьюха уровня</param>
    /// <param name="levelName">Ид уровня ля поулчения даннных</param>
    public void LoadBuildingToLevelSlots(LevelView levelView, string levelName)
    {
      var levelData = Global.Instance.StaticData.LevelDatas.FirstOrDefault(l => l.Level.Equals(levelName));

      if (levelData == null)
      {
        Debug.LogError($"levelData {levelName} is NULL!!!");
        return;
      }

      if (levelData.SlotDatas == null)
      {
        Debug.LogError($"levelData.SlotDatas is NULL!!!");
        return;
      }

      foreach (var slot in levelView.Slots)
      {
        var slotData = levelData.SlotDatas.FirstOrDefault(s => s.Index.Equals(slot.Index));

        if (slotData == null)
          continue;

        var buildData = Global.Instance.StaticData.BuildDatas.FirstOrDefault(b => b.Id.Equals(slotData.BuildingId));

        if (buildData == null)
          continue;

        var building = Factory.Instance.GetBuilding(buildData.Prefab);

        if (building == null)
        {
          Debug.LogError($"LevelObject {slotData.BuildingId} is NULL!!!");
          continue;
        }

        building.transform.SetParentWithParam(slot.Transform);

        var buildingView = building.GetComponent<View>();

        if (buildingView == null)
        {
          Debug.LogError($"levelObjectView is NULL!!!");
          continue;
        }

        if (buildData.Type.Equals("table"))
        {
          if (buildData.Id.Equals("table_coocked"))
          {
            var beh = new ProduceAdvancedBehaviour(buildingView, buildData);
            beh.Init();
            BuildingBehaviours.Add(beh);
          }
          else
          {
            var beh = new ProduceBehaviour(buildingView, buildData);
            beh.Init();
            BuildingBehaviours.Add(beh);
          }
        }
        else if (buildData.Type.Equals("cash"))
        {
          var beh = new CashBehaviour(buildingView, buildData);
          BuildingBehaviours.Add(beh);
        }
        else if (buildData.Type.Equals("bin"))
        {
          var beh = new BinBehaviour(buildingView, buildData);
          BuildingBehaviours.Add(beh);
        }
        else if (buildData.Type.Equals("decor"))
        {
          var beh = new DecorBehaviour(buildingView, buildData);
          beh.Init();
          BuildingBehaviours.Add(beh);
        }
        else if (buildData.Type.Equals("floor"))
        {
          var beh = new FloorBehaviour(buildingView, buildData);
          BuildingBehaviours.Add(beh);
        }
      }
    }
    #endregion LoadCommon

    // ===================================================================================================
    void Update()
    {
      OneSecondTimer?.Tick();
      Timer?.Tick();
    }

    // ===================================================================================================
    private void OnDestroy()
    {
      OneSecondTimer?.Stop();
      Timer?.Stop();
      EventManager.Instance?.RemoveListener<E_BattleEnd>(OnBattleEnd);
      EventManager.Instance?.RemoveListener<E_ExpUpdate>(OnExpUpdate);
      EventManager.Instance?.RemoveListener<E_RatingPointUpdate>(OnRatingPointUpdate);
    }
  }
}
