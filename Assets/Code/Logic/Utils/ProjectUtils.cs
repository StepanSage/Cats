using Extension;
using Player;
using Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static ObjectPoolManager;

public static class ProjectUtils
{
  #region ParseTimeGood
  // ===================================================================================================
  public static string ParseTimeGoodV2(TimeSpan time)
  {
    double tmpVal = 0f;
    if (time.TotalHours >= 24)
    {
      tmpVal = Mathf.Round((float) (Math.Floor((float) time.TotalHours) - Math.Floor((float) time.TotalDays) * 24));
      if (tmpVal <= 0)
        return $"{Mathf.Floor((float) time.TotalDays)}d";
      else
        return $"{Mathf.Floor((float) time.TotalDays)}d {tmpVal}h";
    }
    else if (time.TotalMinutes >= 60)
    {
      tmpVal = Mathf.Round((float) (Math.Floor((float) time.TotalMinutes) - Math.Floor((float) time.TotalHours) * 60));
      if (tmpVal <= 0)
        return $"{Mathf.Floor((float) time.TotalHours)}h";
      else
        return $"{Mathf.Floor((float) time.TotalHours)}h {tmpVal}m";
    }
    else if (time.TotalSeconds >= 60)
    {
      tmpVal = Mathf.Round((float) (Math.Floor((float) time.TotalSeconds) - Math.Floor((float) time.TotalMinutes) * 60));
      if (tmpVal <= 0)
        return $"{Mathf.Floor((float) time.TotalMinutes)}m";
      else
        return $"{Mathf.Floor((float) time.TotalMinutes)}m {tmpVal}s";
    }
    else if (time.TotalMilliseconds >= 1000)
    {
      tmpVal = Mathf.Round((float) (Math.Floor((float) time.TotalMilliseconds) - Math.Floor((float) time.TotalSeconds) * 1000));
      if (tmpVal <= 0)
        return $"{Mathf.Floor((float) time.TotalSeconds)}s";
      else
        return $"{Mathf.Floor((float) time.TotalSeconds)}s {tmpVal}ms";
    }
    else
    {
      return $"{Mathf.Floor((float) time.TotalMilliseconds)}ms";
    }
  }

  // ===================================================================================================
  public static string ParseTimeGood(TimeSpan time, bool showZeroValue = true)
  {
    int paramCount = 0;
    string dateTime = "";
    string secondEmptyParam = "";

    /*Days*/
    double tmpDays = Mathf.Round((float) (Mathf.Floor((float) time.TotalDays)));
    if (tmpDays > 0)
    {
      paramCount++;
      dateTime += $"{tmpDays}d" + (paramCount < 2 ? " " : "");
      secondEmptyParam = $"0h";
    }

    /*Hours*/
    double tmpHours = Mathf.Round((float) (Math.Floor((float) time.TotalHours) - Math.Floor((float) time.TotalDays) * 24));
    if (tmpHours > 0)
    {
      paramCount++;
      dateTime += $"{tmpHours}h" + (paramCount < 2 ? " " : "");

      if (paramCount == 2)
        return dateTime;

      secondEmptyParam = $"0m";
    }

    /*Minutes*/
    double tmpMinutes = Mathf.Round((float) (Math.Floor(time.TotalMinutes) - Math.Floor(time.TotalHours) * 60));
    if (tmpMinutes > 0)
    {
      paramCount++;
      dateTime += $"{tmpMinutes}m" + (paramCount < 2 ? " " : "");

      if (paramCount == 2)
        return dateTime;

      secondEmptyParam = $"0c";
    }

    /*Seconds*/
    double tmpSeconds = Mathf.Round((float) (Math.Floor((float) time.TotalSeconds) - Math.Floor((float) time.TotalMinutes) * 60));
    if (tmpSeconds > 0)
    {
      paramCount++;
      dateTime += $"{tmpSeconds}s";

      if (paramCount == 2)
        return dateTime;
    }

    return (paramCount == 1 ? (showZeroValue ? dateTime += $"{secondEmptyParam}" : dateTime) : $"0s");
  }

  // ===================================================================================================
  public static string ParseTimeGood(long time, bool showZeroValue = true)
  {
    TimeSpan tm = TimeSpan.FromSeconds(time);
    return ParseTimeGood(tm, showZeroValue);
  }
  #endregion ParseTimeGood

  // ===================================================================================================
  public static bool TryGetBuildingById(string id, out BuildingBehaviour buildingBehaviour)
  {
    buildingBehaviour = Global.Instance.Game.BuildingBehaviours.FirstOrDefault(b=>b.BuildData.Id.Equals(id));
    
    
    return buildingBehaviour != null;
  }

  #region Products
  // ===================================================================================================
  public static List<ProductData> GetProductsByLevel(this List<ProductData> products)
  {
    var player = Global.Instance.PlayerData;
    return products.Where(product => (player.Level >= Global.Instance.StaticData.ItemDatas.First(i => i.Id.Equals(product.Id)).MinPlayerLevel)).ToList();
  }

  // ===================================================================================================
  public static ItemData GetItemById(string itemId)
  {
    return Global.Instance.StaticData.ItemDatas.First(i => i.Id.Equals(itemId));
  }

  // ===================================================================================================
  public static ProductData GetRandomProduct(this List<ProductData> products, int weightMax)
  {
    var player = Global.Instance.PlayerData;
    var productList = products.GetProductsByLevel();

    var productListByWeight = productList.Where(product => (product.Weight <= weightMax)).ToList();

    if (!productListByWeight?.Any() ?? true)
      return null;

    return productListByWeight[UnityEngine.Random.Range(0, productListByWeight.Count())];
  }

  // ===================================================================================================
  public static ProductData GetProductDataById(string id)
  {
    return Global.Instance.StaticData.ProductDatas.FirstOrDefault(p => p.Id.Equals(id));
  }

  // ===================================================================================================
  public static List<ProductData> GetProductDatasByIds(List<string> ids)
  {
    return Global.Instance.StaticData.ProductDatas.Where(p => ids.Contains(p.Id)).ToList();
  }

  // ===================================================================================================
  /// <summary>
  /// Проверка, хватает ли в слоте игрока ингрежиентов для оздания продукта
  /// </summary>
  /// <param name="product">ProductData</param>
  /// <returns></returns>
  public static bool IsIngredientEnought(this ProductData product)
  {
    var receipt = product.Receipt.Clone();
    var consumeInSlots = PlayerUtils.BattleSlots.Where(s => s.Consume != null).Select(s => s.Consume).ToList().Clone();

    foreach (var r in receipt)
    {
      var cons = consumeInSlots.FirstOrDefault(c => c.Type.Equals(r.Type) && c.Amount > 0);

      if (cons == null)
        return false;

      cons.Amount--;
    }

    return true;
  }

  // ===================================================================================================
  public static bool EqualsData(this Consume consume, Consume otherConsume)
  {
    DebugX.LogForUtils($"ProjectUtils : AddConsumeToPlayerSlot");

    return consume.Type.Equals(otherConsume.Type) && consume.Amount.Equals(otherConsume.Amount);
  }

  #endregion Products

  #region PlayerSlots

  // ===================================================================================================
  /// <summary>
  /// Получить все отсортированные боевые слоты
  /// </summary>
  /// <param name="slots">слоты</param>
  /// <returns></returns>
  public static List<PlayerSlot> GetBattleSlots(this List<PlayerSlot> slots)
  {
    return PlayerUtils.BattleSlots.Where(s => s.Type.Equals(EPlayerSlotType.Battle)).OrderBy(s => s.Index).ToList();
  }

  // ===================================================================================================
  /// <summary>
  /// Добавление расходников в слот игрока
  /// </summary>
  /// <param name="consume"></param>
  public static void AddConsumeToPlayerSlot(Consume consume, Action callBackEnd = null, Action callBackFail = null)
  {
    var playerData = Global.Instance.PlayerData;
    var slot = GetPlayerSlotWaitConsume();

    if ((playerData == null) || (consume == null) || (slot == null))
    {
      callBackFail?.Invoke();
      return;
    }

    slot.Consume = consume;
    slot.WaitConsume = false;

    EventManager.Instance?.TriggerEvent(new E_PlayerSlotUpdate(slot));
    callBackEnd?.Invoke();
  }

  // ===================================================================================================
  /// <summary>
  /// Удаление расходников из слота игрока
  /// </summary>
  /// <param name="consume">расходник</param>
  /// <param name="callBackEnd"></param>
  /// <param name="callBackFail"></param>
  public static void RemoveConsumeFromPlayerSlot(List<Consume> consums)
  {
    if ((consums == null) || !consums.Any())
      return;

    foreach (var cons in consums)
    {
      RemoveConsumeFromPlayerSlot(cons);
    }
  }

  // ===================================================================================================
  /// <summary>
  /// Удаление расходника из слота игрока
  /// </summary>
  /// <param name="consume">расходник</param>
  /// <param name="callBackEnd"></param>
  /// <param name="callBackFail"></param>
  public static void RemoveConsumeFromPlayerSlot(Consume consume, Action callBackEnd = null, Action callBackFail = null)
  {
    var playerData = Global.Instance.PlayerData;

    if (playerData == null)
    {
      callBackFail?.Invoke();
      return;
    }

    DebugX.LogForUtils($"PlayerDataEx : RemoveConsumeFromPlayerSlot :{consume}");

    var playerSlot = GetPlayerSlotWithConsume(consume);

    if (playerSlot == null)
    {
      callBackFail?.Invoke();
      return;
    }

    playerSlot.Consume = null;

    EventManager.Instance?.TriggerEvent(new E_PlayerSlotUpdate(playerSlot));

    callBackEnd?.Invoke();
  }

  // ===================================================================================================
  /// <summary>
  /// Получить слот, в котором есть нужный разсходник 
  /// </summary>
  /// <param name="consume">разсходник</param>
  /// <returns>PlayerSlot</returns>
  public static PlayerSlot GetPlayerSlotWithConsume(Consume consume)
  {
    if (PlayerUtils.BattleSlots == null)
      return null;

    var firstEmptySlot = PlayerUtils.BattleSlots.GetBattleSlots().Where(s => (s.Consume != null) && !s.WaitConsume && s.Consume.EqualsData(consume))?.OrderBy(s => s.Index)?.FirstOrDefault();

    return firstEmptySlot;
  }

  // ===================================================================================================
  /// <summary>
  /// Получить слот, ожидающий расходник разсходник 
  /// </summary>
  /// <returns>PlayerSlot</returns>
  public static PlayerSlot GetPlayerSlotWaitConsume()
  {
    if (PlayerUtils.BattleSlots == null)
      return null;

    var firstEmptySlot = PlayerUtils.BattleSlots.GetBattleSlots().Where(s => (s.Consume == null) && s.WaitConsume)?.OrderBy(s => s.Index)?.FirstOrDefault();

    return firstEmptySlot;
  }

  // ===================================================================================================
  /// <summary>
  /// Проверить есть ли расходник в слотах игрока
  /// </summary>
  /// <param name="consume"></param>
  /// <returns></returns>
  public static bool CheckConsumeOnPlayerSlots(Consume consume)
  {
    return GetPlayerSlotWithConsume(consume) != null;
  }

  #region EmptyPlayerSlot
  // ===================================================================================================
  /// <summary>
  /// Получить количетво пустых слотов игрока
  /// </summary>
  /// <returns>int</returns>
  public static int GetEmptyPlayerSlotCount()
  {
    if (PlayerUtils.BattleSlots == null)
      return 0;

    return PlayerUtils.BattleSlots.GetBattleSlots().Count(s => (s.Consume == null) && !s.WaitConsume);
  }

  // ===================================================================================================
  /// <summary>
  /// Проверка, есть ли хотя бы один пппустой слот у игрока
  /// </summary>
  /// <returns>bool</returns>
  public static bool CheckPlayerEmptySlot()
  {
    return GetEmptyPlayerSlotCount() > 0;
  }

  // ===================================================================================================
  /// <summary>
  /// Получить пустой слот игрока
  /// </summary>
  /// <returns>PlayerSlot</returns>
  public static PlayerSlot GetEmptyPlayerSlot()
  {
    if (PlayerUtils.BattleSlots == null)
      return null;

    var firstEmptySlot = PlayerUtils.BattleSlots.GetBattleSlots()
                                         .Where(s => (s.Consume == null) && !s.WaitConsume)?
                                         .OrderBy(s => s.Index)?
                                         .FirstOrDefault();

    return firstEmptySlot;
  }
  #endregion EmptyPlayerSlot

  // ===================================================================================================
  /// <summary>
  /// Очистить все слоты игрока
  /// </summary>
  /// <param name="callbackEnd"></param>
  /// <param name="callbackFail"></param>
  public static void ClearAllPlayerSlots(Action<List<Consume>> callbackEnd = null, Action callbackFail = null)
  {
    var cons = PlayerUtils.BattleSlots.GetBattleSlots().Select(c => c.Consume).ToList();

    PlayerUtils.BattleSlots.GetBattleSlots().ForEach(c => c.Consume = null);

    EventManager.Instance?.TriggerEvent(new E_PlayerSlotUpdate(null));
    callbackEnd?.Invoke(cons);
  }
  #endregion PlayerSlots

  #region PlayerStorage
  // ===================================================================================================
  /// <summary>
  /// Проверка количества ресурса в инвентаре
  /// </summary>
  /// <param name="slot">слот</param>
  public static bool IsEnought(this Consume needComsume)
  {
    if (needComsume == null)
      return false;

    var consumeInStorage = Global.Instance.PlayerData.Storage.FirstOrDefault(c => c.Type.Equals(needComsume.Type));

    if (consumeInStorage == null)
      return false;

    return consumeInStorage.Amount >= needComsume.Amount;
  }

  // ===================================================================================================
  /// <summary>
  /// Проверка количества ресурсов в инвентаре
  /// </summary>
  /// <param name="slot">слот</param>
  public static bool IsEnought(this List<Consume> needComsumes)
  {
    if (needComsumes == null || !needComsumes.Any())
      return false;

    foreach (var consume in needComsumes)
    {
      if (!consume.IsEnought())
        return false;
    }

    return true;
  }

  // ===================================================================================================
  /// <summary>
  /// Добавление расходников в небоевой инвентарь игрока
  /// </summary>
  /// <param name="consume">расходник</param>
  public static void AddConsumeToStorage(Consume consume)
  {
    var playerData = Global.Instance.PlayerData;

    if ((playerData == null) || (consume == null))
      return;

    if (playerData.Storage == null)
      return;

    var cons = playerData.Storage.FirstOrDefault(c => c.Type.Equals(consume.Type));

    if (cons == null)
    {
      playerData.Storage.Add(new Consume(consume.Type, consume.Amount));
    }
    else
    {
      cons.Amount += consume.Amount;
    }
  }

  // ===================================================================================================
  public static void RemoveConsumeFromStorage(List<Consume> consums)
  {
    if ((consums == null) || !consums.Any())
      return;

    foreach (var cons in consums)
    {
      RemoveConsumeFromStorage(cons);
    }
  }

  // ===================================================================================================
  /// <summary>
  /// Удаление расходника из инвентаря
  /// </summary>
  /// <param name="consume">расходник</param>
  public static void RemoveConsumeFromStorage(Consume consume)
  {
    var playerData = Global.Instance.PlayerData;

    #region Errors
    if (CheckError(playerData == null, "playerData == null", MethodBase.GetCurrentMethod()))
      return;

    if (CheckError(consume == null, "consume == null", MethodBase.GetCurrentMethod()))
      return;

    if (CheckError(playerData.Storage == null, "playerData.Storage == null", MethodBase.GetCurrentMethod()))
      return;

    var cons = GetConsume(consume.Type);

    if (CheckError(cons == null, $"cons == null  {consume.Type}", MethodBase.GetCurrentMethod()))
      return;

    if (CheckError(consume.Amount > cons.Amount, "consume.Amount > cons.Amount", MethodBase.GetCurrentMethod()))
      return;

    #endregion Errors

    cons.Amount -= consume.Amount;

    if (consume.Type.Equals("gold"))
    {
      EventManager.Instance?.TriggerEvent(new E_GoldUpdate(consume));
    }
    else if (consume.Type.Equals("crystal"))
    {
      EventManager.Instance?.TriggerEvent(new E_CrystalUpdate(consume));
    }

    EventManager.Instance?.TriggerEvent(new E_ConsumeUpdate(consume));
  }
  #endregion PlayerStorage

  #region PlayerParam
  // ===================================================================================================
  /// <summary>
  /// Добавление параметра игроку
  /// </summary>
  /// <param name="consume">расходник</param>
  public static void AddPlayerParam(Consume consume)
  {
    var playerData = Global.Instance.PlayerData;

    if ((playerData == null) || (consume == null))
      return;

    if (consume.Type.Equals("exp"))
    {
      playerData.Experience += consume.Amount;
      EventManager.Instance?.TriggerEvent(new E_ExpUpdate(consume));
    }

    if (consume.Type.Equals("rating_point"))
    {
      playerData.RatingPoint += consume.Amount;
      EventManager.Instance?.TriggerEvent(new E_RatingPointUpdate(consume));
    }
  }

  // ===================================================================================================
  /// <summary>
  /// Удаление параметра игроку
  /// </summary>
  /// <param name="consume">расходник</param>
  public static void RemovePlayerParam(Consume consume)
  {
    var playerData = Global.Instance.PlayerData;

    #region Errors
    if (CheckError(playerData == null, "playerData == null", MethodBase.GetCurrentMethod()))
      return;
    if (CheckError(consume == null, "consume == null", MethodBase.GetCurrentMethod()))
      return;
    #endregion Errors


    if (consume.Type.Equals("exp"))
    {
      playerData.Experience -= consume.Amount;
      EventManager.Instance?.TriggerEvent(new E_ExpUpdate(consume));
    }
  }
  #endregion PlayerParam

  #region Consumes
  // ===================================================================================================
  public static bool IsConsumeExist(string type)
  {
    var consume = Global.Instance.PlayerData.Storage?.FirstOrDefault(c => c.Type.Equals(type));
    return consume != null;
  }

  // ===================================================================================================
  public static long GetConsumeValue(string type)
  {
    var consume = Global.Instance.PlayerData.Storage.FirstOrDefault(c => c.Type.Equals(type));

    if (consume == null)
      return 0;

    return consume.Amount;
  }

  // ===================================================================================================
  public static Consume GetConsume(string type)
  {
    var consume = Global.Instance.PlayerData.Storage.FirstOrDefault(c => c.Type.Equals(type));
    return consume;
  }

  #region AddConsumes
  // ===================================================================================================
  public static void AddConsume(List<Consume> consums)
  {
    if (consums == null)
      return;

    foreach (var cons in consums)
    {
      AddConsume(cons);
    }
  }

  // ===================================================================================================
  public static void AddConsume(Consume consume)
  {
    DebugX.LogForUtils($"ProjectUtils : AddConsume : {consume.Type} {consume.Amount}");

    if (consume == null)
      return;

    var prod = GetItemById(consume.Type);

    if (prod == null)
      return;

    if (prod.StorageType.Equals(EItemDataStorageType.Storage))
    {
      AddConsumeToStorage(consume);
    }
    else if (prod.StorageType.Equals(EItemDataStorageType.Slot))
    {
      AddConsumeToPlayerSlot(consume);
    }
    else if (prod.StorageType.Equals(EItemDataStorageType.Param))
    {
      AddPlayerParam(consume);
    }
    else
    {
      return;
    }

    if (consume.Type.Equals("gold"))
    {
      EventManager.Instance?.TriggerEvent(new E_GoldUpdate(consume));
    }
    else if (consume.Type.Equals("crystal"))
    {
      EventManager.Instance?.TriggerEvent(new E_CrystalUpdate(consume));
    }

    EventManager.Instance?.TriggerEvent(new E_ConsumeUpdate(consume));
    DataStoreManager.Instance.DataSave();
  }
  #endregion AddConsumes

  #region AnimatedConsumes
  // ===================================================================================================
  public static void AnimatedReward(Consume consume, Vector3 start, Vector3 end, bool addNow)
  {
    AnimatedItem(consume, start, end, addNow);
  }

  // ===================================================================================================
  public static void AnimatedReward(List<Consume> consums, Vector3 start, Vector3 end, bool addNow)
  {
    foreach (var c in consums)
    {
      AnimatedItem(c, start, end, addNow);
    }
  }

  // ===================================================================================================
  public static void AnimatedRemove(Consume consume, Vector3 start, Vector3 end)
  {
    AnimatedItem(consume, start, end, false);
  }

  // ===================================================================================================
  public static void AnimatedRemove(List<Consume> consums, Vector3 start, Vector3 end)
  {
    AnimatedItem(consums, start, end, false);
  }

  // ===================================================================================================
  private static void AnimatedItem(List<Consume> consums, Vector3 start, Vector3 end, bool addNow)
  {
    foreach (var c in consums)
    {
      AnimatedItem(c, start, end, addNow);
    }
  }

  // ===================================================================================================
  private static void AnimatedItem(Consume consume, Vector3 start, Vector3 end, bool addNow)
  {
    int maxItemCount = 10;
    int amountForOne = consume.Amount / maxItemCount;
    int amountRemainder = (amountForOne == 0 ? 0 : consume.Amount % amountForOne);

    if (addNow)
    {
      AddConsume(consume);
    }

    if (amountForOne <= 0)
    {
      AnimatedItemStart(new Consume(consume.Type, consume.Amount), start, end, addNow);
    }
    else
    {
      for (var i = 0; i < maxItemCount; i++)
      {
        AnimatedItemStart(new Consume(consume.Type, amountForOne), start, end, addNow);
      }

      if (amountRemainder > 0)
      {
        AnimatedItemStart(new Consume(consume.Type, amountRemainder), start, end, addNow);
      }
    }
  }

  // ===================================================================================================
  private static void AnimatedItemStart(Consume consume, Vector3 start, Vector3 end, bool addNow = true)
  {
    Global.Instance.PlayerData.WaitableStorage.Add(consume); // Записываем во временный инвентарь сразу

    //Создаем летящий елемент
    var ob = Spawn(EObjectPoolType.UI);
    ob.transform.position = start;
    ob.GetComponent<DroppedItem>()?.Init(consume, start, end, !addNow);
  }

  // ===================================================================================================
  public static void AnimatedItemAddEnd(Consume consume)
  {
    DebugX.LogForUtils($"ProjectUtils : AnimatedItemAddEnd : {consume.Type} {consume.Amount}");

    AddConsume(consume);
    Global.Instance.PlayerData.WaitableStorage.Remove(consume); // Удаляем из временного хранилища
  }
  #endregion AnimatedConsumes

  #endregion Consumes

  //Отношение текущего разрешения канваса к рефференсному
  #region Resolution 
  // ===================================================================================================
  public static float GetCurToRefResolutionRatioHeight()
  {
    return (((float) Screen.height) / Global.Instance.Game.GameUI.RefferenceResolution.y);
  }

  // ===================================================================================================
  public static float GetCurToRefResolutionRatioWidth()
  {
    return (((float) Screen.width) / Global.Instance.Game.GameUI.RefferenceResolution.x);
  }

  // ===================================================================================================
  public static float GetValueWithRatioHeight(float value)
  {
    return value * GetCurToRefResolutionRatioHeight();
  }

  // ===================================================================================================
  public static Vector3 GetValueWithRatioHeight(Vector3 value)
  {
    return value * GetCurToRefResolutionRatioHeight();
  }

  // ===================================================================================================
  public static float GetValueWithRatioWidth(float value)
  {
    return value * GetCurToRefResolutionRatioWidth();
  }

  // ===================================================================================================
  public static Vector3 GetValueWithRatioWidth(Vector3 value)
  {
    return value * GetCurToRefResolutionRatioWidth();
  }
  #endregion Resolution

  #region Consts
  // ===================================================================================================
  public static int GetConstInt(EConstType type)
  {
    var constdata = Global.Instance.StaticData.ConstDatas.FirstOrDefault(c => c.Id.Equals(type));

    if (constdata == null)
      return 0;

    return (int) constdata.IntValue;
  }

  // ===================================================================================================
  public static long GetConstLong(EConstType type)
  {
    var constdata = Global.Instance.StaticData.ConstDatas.FirstOrDefault(c => c.Id.Equals(type));

    if (constdata == null)
      return 0;

    return constdata.IntValue;
  }

  // ===================================================================================================
  public static float GetConstFloat(EConstType type)
  {
    var constdata = Global.Instance.StaticData.ConstDatas.FirstOrDefault();

    if (constdata == null)
      return 0f;

    return (float) constdata.FloatValue;
  }

  // ===================================================================================================
  public static double GetConstDouble(EConstType type)
  {
    var constdata = Global.Instance.StaticData.ConstDatas.FirstOrDefault();

    if (constdata == null)
      return 0d;

    return constdata.FloatValue;
  }

  // ===================================================================================================
  public static string GetConstString(EConstType type)
  {
    var constdata = Global.Instance.StaticData.ConstDatas.FirstOrDefault();

    if (constdata == null)
      return "";

    return constdata.StringValue;
  }

  // ===================================================================================================
  public static bool GetConstBool(EConstType type)
  {
    var constdata = Global.Instance.StaticData.ConstDatas.FirstOrDefault();

    if (constdata == null)
      return false;

    return constdata.BoolValue;
  }
  #endregion Consts

  #region TimerEx
  // ===================================================================================================
  /// <summary>
  /// Пройденное время с начала старта таймера
  /// </summary>
  /// <returns>Время в секундах</returns>
  public static long Elapsed(this Timer timer)
  {
    return Mathf.RoundToInt((float) Math.Min(Global.Instance.Time - timer.StartTime, timer.Duration));
  }

  // ===================================================================================================
  /// <summary>
  /// Прогрес таймера от 0 до 1
  /// </summary>
  /// <returns>0f..1f</returns>
  public static float Progress(this Timer timer)
  {
    return timer.Elapsed() / (float) timer.Duration;
  }

  // ===================================================================================================
  public static float ProgressInvert(this Timer timer)
  {
    return timer.Remaining() / (float) timer.Duration;
  }

  // ===================================================================================================
  /// <summary>
  /// Оставшееся время до конца таймера
  /// </summary>
  /// <returns>Время в секундах</returns>
  public static float Remaining(this Timer timer)
  {
    if (timer == null)
      return 0;
    return (timer.Duration - timer.Elapsed());
  }

  // ===================================================================================================
  public static bool IsStarted(this Timer timer)
  {
    if (timer == null)
      return false;
    return (timer.StartTime > 0 && timer.Duration >= 0);
  }

  // ===================================================================================================
  public static bool IsInProgress(this Timer timer)
  {
    if (timer == null)
      return false;
    return (timer.IsStarted() && !IsExpired(timer));
  }

  // ===================================================================================================
  public static bool IsExpired(this Timer timer)
  {
    if (timer == null)
      return false;
    return (timer.IsStarted() && ((timer.StartTime + timer.Duration) <= Global.Instance.Time));
  }

  //=======================================================================================================================
  public static long GetTimeLeft(this Timer timer)
  {
    var sTime = ((timer.StartTime + timer.Duration) - Global.Instance.Time);
    return Mathf.RoundToInt((float) (Mathf.Clamp((float) sTime, 0, timer.Duration)));
  }
  #endregion TimerEx

  // ===================================================================================================
  public static Consume GetRatingPoint(bool isWin)
  {
    var ratingLevel = Global.Instance.StaticData.PlayerRatingLevelDatas.FirstOrDefault(rl => rl.RatingLevel.Equals(PlayerUtils.Data.RatingLevel));

    if (ratingLevel == null)
      return null;

    var point = isWin ? ratingLevel.WinPoint : ratingLevel.LoosePoint;

    return new Consume("rating_point", point);
  }

  // ===================================================================================================
  public static bool CheckError(bool result, string errName, MethodBase metod, Action callback = null)
  {
    if (result)
    {
      DebugX.LogError($"Error on check [{errName}] ");

      try
      {
        callback?.Invoke();
      }
      catch (Exception e) { DebugX.LogError($"Error on check [{errName}] in [{metod.DeclaringType}.{metod.Name}] callback", e); }
    }

    return result;
  }

  // ===================================================================================================
  public static void SetParentWithParam(this Transform transform, Transform owner, bool dropScale = false)
  {
    transform.SetParent(owner);
    transform.localPosition = Vector3.zero;
    transform.localRotation = Quaternion.identity;

    if (dropScale)
    {
      transform.localScale = Vector3.one;
    }
  }
}