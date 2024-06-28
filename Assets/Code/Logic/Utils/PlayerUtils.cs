using Extension;
using Objects;
using Player;
using Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class PlayerUtils
{
  public static PlayerData Data => Global.Instance.PlayerData;
  public static PlayerController Controller => Global.Instance.Game.Player;

  public static List<Order> Orders = new List<Order>(); // Список заказов в бою
  public static List<PlayerSlot> BattleSlots = new List<PlayerSlot>(); // Список активных заказов в бою

  public static int ExperienceMax => GetExperienceMax();
  public static int RatingPointMax => GetRatingPointMax();

  // ===================================================================================================
  public static void Init()
  {
    Data.Storage.Add(new Consume("decor_cat_1", 1));

    Data.Slots.Add(new PlayerSlot(1, EPlayerSlotType.Body, new Consume("decor_cat_1", 1)));
    Data.Slots.Add(new PlayerSlot(1, EPlayerSlotType.Head));
    DataStoreManager.Instance.DataSave();
  }

  // ===================================================================================================
  public static void InitBattleSlots()
  {
    BattleSlots = new List<PlayerSlot>();

    var playerLevelData = Global.Instance.StaticData.PlayerLevelDatas.FirstOrDefault(x => x.Level.Equals(Data.Level));

    int slotCount = 3;

    if (playerLevelData != null)
    {
      slotCount = playerLevelData.BattleSlotCount;
    }
    else
    {
      DebugX.LogForUtils($"PlayerUtils : InitBattleSlots : playerLevelData is null!!!");
    }

    for (var i = 0; i < slotCount; i++)
    {
      BattleSlots.Add(new PlayerSlot(i, EPlayerSlotType.Battle));
    }
  }

  // ===================================================================================================
  /// <summary>
  /// Загрузка визуала игрока
  /// </summary>
  /// <param name="slot">слот</param>
  public static void LoadPlayerBody()
  {
    Controller.BodyClear();
    Controller.Body = Factory.Instance.GetDecor(GetBody());
    Controller.Body.transform.SetParentWithParam(Controller.SlotBody);
    Controller.Init();
  }

  // ===================================================================================================
  /// <summary>
  /// Инициализация игрока
  /// </summary>
  public static void InitPlayer()
  {
    var player = Factory.Instance.GetBuilding("player");

    if (player == null)
    {
      DebugX.LogError($"player is NULL!!!");
      return;
    }

    var controller = player.GetComponent<PlayerController>();

    Global.Instance.Game.Player = controller;

    LoadPlayerBody();

    EventManager.Instance?.TriggerEvent(new E_PlayerInit(controller));
  }

  // ===================================================================================================
  public static void SetBody(string body)
  {
    ChangeSlot(EPlayerSlotType.Body, new Consume(body, 1));
    LoadPlayerBody();
  }

  // ===================================================================================================
  public static void ChangeSlot(EPlayerSlotType type, Consume consume)
  {
    var slot = Data.Slots.First(s => s.Type.Equals(type));
    slot.Consume = consume;
    DataStoreManager.Instance.DataSave();
  }

  // ===================================================================================================
  public static string GetBody()
  {
    return Data.Slots.First(s => s.Type.Equals(EPlayerSlotType.Body)).Consume.Type;
  }

  // ===================================================================================================
  public static void SetHead(string head)
  {
    var slot = Data.Slots.First(s => s.Type.Equals(EPlayerSlotType.Head));
    slot.Consume = new Consume(head, 1);
  }

  // ===================================================================================================
  public static string GetHead()
  {
    return Data.Slots.First(s => s.Type.Equals(EPlayerSlotType.Head)).Consume.Type;
  }

  // ===================================================================================================
  public static void SetBattleSlot(int index, Consume consume)
  {
    var slot = Data.Slots.First(s => s.Type.Equals(EPlayerSlotType.Battle) && s.Index.Equals(index));

    slot.Consume = consume;
  }

  // ===================================================================================================
  private static int GetExperienceMax()
  {
    return Global.Instance.StaticData.PlayerLevelDatas.FirstOrDefault(l => l.Level.Equals(Data.Level)).ExpForLevel;
  }

  // ===================================================================================================
  private static int GetRatingPointMax()
  {
    return Global.Instance.StaticData.PlayerRatingLevelDatas.FirstOrDefault(l => l.RatingLevel.Equals(Data.RatingLevel)).Point;
  }

  // ===================================================================================================
  public static void GetRatingReward(Action<List<Consume>> calbackEnd = null, Action calbackFail = null)
  {
    #region Errors
    if (ProjectUtils.CheckError(Data.RatingLevel <= Data.RatingLevelGetReward, "PlayerData.RatingLevel <= PlayerData.RatingLevelGetReward", MethodBase.GetCurrentMethod(), calbackFail))
      return;

    var rewardLevel = Data.RatingLevel + 1;

    var ratingData = Global.Instance.StaticData.PlayerRatingLevelDatas.FirstOrDefault(l => l.RatingLevel.Equals(rewardLevel));

    if (ProjectUtils.CheckError(ratingData == null, "ratingData == null", MethodBase.GetCurrentMethod(), calbackFail))
      return;
    #endregion Errors

    Data.RatingLevelGetReward++;

    calbackEnd?.Invoke(ratingData.Rewards.Clone());
  }
}