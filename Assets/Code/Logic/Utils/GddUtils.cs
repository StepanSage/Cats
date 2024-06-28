using ExcelDataReader;
using Newtonsoft.Json;
using Static;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEngine;

public static class GddUtils
{
  
  
  private readonly static string _pathXLSFolderGD = $"{Application.dataPath}/GDD_Calculator.xlsm";
  private readonly static string _pathXLSFolderGDLocalization = $"{Application.dataPath}/Localization.xlsm";
  private readonly static string _filePathStaticGD = $"{Application.dataPath}/static_base.json";
  private readonly static string _filePathStaticGDLocalization = $"{Application.dataPath}/localization.json";

  private readonly static string _pathXLSFolderNoGD = $@"{Application.dataPath}\..\- Project Data -\Docs\GDD_Calculator.xlsm";
  private readonly static string _pathXLSFolderNoGDLocalization = $@"{Application.dataPath}\..\- Project Data -\Docs\Localization.xlsm";
  private readonly static string _filePathStaticNoGD = $@"{Application.dataPath}\Resources\static_base.json";
  private readonly static string _filePathStaticNoGDLocalization = $@"{Application.dataPath}\Resources\localization.json";

  private static string _filePathStatic = "";
  private static string _filePathLocalization = "";

  private static StaticData Static = new StaticData();
  private static DataTableCollection TableCollection;

  // ===================================================================================================
  public static void ImportStatic()
  {
    var pathXLSFolder = "";
    var filePathStatic = "";

    if (File.Exists(_pathXLSFolderGD))
    {
      pathXLSFolder = _pathXLSFolderGD;
      filePathStatic = _filePathStaticGD;
    }
    else
    {
      pathXLSFolder = _pathXLSFolderNoGD;
      filePathStatic = _filePathStaticNoGD;
    }

    _filePathStatic = filePathStatic;

    using (var fileContent = File.Open(pathXLSFolder, FileMode.Open))
    {
      using (var reader = ExcelReaderFactory.CreateReader(fileContent))
      {
        DataSet dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
          ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
          {
            UseHeaderRow = true,
          }
        });

        TableCollection = dataSet.Tables;

        LoadConstDatas();
        LoadPlayerLevelDatas();
        LoadPlayerRatingLevelDatas();
        LoadLevelDatas();
        LoadProductDatas();
        LoadBuildDatas();
        LoadItemDatas();
        LoadShopDatas();
        LoadOrderDatas();
        LoadLevelProgressDatas();

        StreamWriter file = File.CreateText(filePathStatic);
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(file, Static);

        fileContent.Close();
        file.Close();
      }
    }

    ImportLocalization();
  }

  // ===================================================================================================
  private static void ImportLocalization()
  {
    var pathXLSFolderLocalization = "";
    var filePathLocalization = "";

    if (File.Exists(_pathXLSFolderGDLocalization))
    {
      pathXLSFolderLocalization = _pathXLSFolderGDLocalization;
      filePathLocalization = _filePathStaticGDLocalization;
    }
    else
    {
      pathXLSFolderLocalization = _pathXLSFolderNoGDLocalization;
      filePathLocalization = _filePathStaticNoGDLocalization;
    }

    _filePathLocalization = filePathLocalization;

    using (var fileContent = File.Open(pathXLSFolderLocalization, FileMode.Open))
    {
      using (var reader = ExcelReaderFactory.CreateReader(fileContent))
      {
        DataSet dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
        {
          ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
          {
            UseHeaderRow = true,
          }
        });

        TableCollection = dataSet.Tables;

        LoadLocalizationDatas();

        StreamWriter file = File.CreateText(filePathLocalization);
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(file, Static.LocalisationDatas);

        fileContent.Close();
        file.Close();
      }
    }
  }

  // ===================================================================================================
  public static StaticData GetStatic()
  {
    DebugX.Log("GetStatic");

    string staticJson = "";

#if UNITY_EDITOR
    ImportStatic();
    staticJson = File.ReadAllText(_filePathStatic);
#else
    if (File.Exists(_pathXLSFolderGD))
    {
      ImportStatic();
      staticJson = File.ReadAllText(_filePathStatic);
    }
    else
    {
      var jsonTextFile = Resources.Load<TextAsset>("static_base");
      staticJson = jsonTextFile.text;
    }
#endif

    if (string.IsNullOrEmpty(staticJson))
      return null;

    var newStatic = JsonConvert.DeserializeObject<StaticData>(staticJson);

    string localizationJson = "";

#if UNITY_EDITOR
    localizationJson = File.ReadAllText(_filePathLocalization);
#else
    if (File.Exists(_pathXLSFolderGD))
    {
      localizationJson = File.ReadAllText(_filePathLocalization);
    }
    else
    {
      var jsonTextFile = Resources.Load<TextAsset>("localization.json");
      localizationJson = jsonTextFile.text;
    }
#endif

    newStatic.LocalisationDatas = JsonConvert.DeserializeObject<List<LocalisationData>>(localizationJson);
    return newStatic;
  }

  // ===================================================================================================
  private static void LoadConstDatas()
  {
    Static.ConstDatas = new List<ConstData>();

    DataTable tbl = TableCollection["ConstDatas"];

    foreach (DataRow row in tbl.Rows)
    {
      ConstData table = new ConstData();

      if (string.IsNullOrEmpty(GetRowString(row, "Id")))
        break;

      table.Id = (EConstType) Enum.Parse(typeof(EConstType), GetRowString(row, "Id"));
      table.IntValue = GetRowInt(row, "IntValue");
      table.FloatValue = GetRowInt(row, "FloatValue");
      table.StringValue = GetRowString(row, "StringValue");
      table.BoolValue = GetRowBool(row, "BoolValue");

      Static.ConstDatas.Add(table);
    }

    DebugX.LogWarning($"Load ConstDatas Complete ({Static.ConstDatas.Count})!");
  }

  // ===================================================================================================
  private static void LoadPlayerLevelDatas()
  {
    Static.PlayerLevelDatas = new List<PlayerLevelData>();

    DataTable playerLevelDatas = TableCollection["PlayerLevelDatas"];
    DataTable playerLevelRewardsDatas = TableCollection["PlayerLevelRewardsDatas"];

    foreach (DataRow row in playerLevelDatas.Rows)
    {
      PlayerLevelData element = new PlayerLevelData();

      if (string.IsNullOrEmpty(GetRowString(row, "level")))
        break;

      element.Level = GetRowInt(row, "level");
      element.ExpForLevel = GetRowInt(row, "expForLevel");
      element.BattleSlotCount = GetRowInt(row, "battleSlotCount");

      element.Rewards = new List<Consume>();

      foreach (DataRow reward in playerLevelRewardsDatas.Rows)
      {
        var level = GetRowInt(reward, "level");

        if (string.IsNullOrEmpty(GetRowString(reward, "level")))
          break;

        if (!level.Equals(element.Level))
          continue;

        var type = GetRowString(reward, "type");
        var amount = GetRowInt(reward, "amount");

        element.Rewards.Add(new Consume(type, amount));
      }

      Static.PlayerLevelDatas.Add(element);
    }

    DebugX.LogWarning("Load PlayerLevelDatas Complete!");
  }

  // ===================================================================================================
  private static void LoadPlayerRatingLevelDatas()
  {
    Static.PlayerRatingLevelDatas = new List<PlayerRatingLevelData>();

    DataTable playerRatingLevelDatas = TableCollection["PlayerRatingLevelDatas"];
    DataTable playerRatingLevelRewardsDatas = TableCollection["PlayerRatingLevelRewardsDatas"];

    foreach (DataRow row in playerRatingLevelDatas.Rows)
    {
      PlayerRatingLevelData element = new PlayerRatingLevelData();

      if (string.IsNullOrEmpty(GetRowString(row, "ratingLevel")))
        break;

      element.RatingLevel = GetRowInt(row, "ratingLevel");
      element.Point = GetRowInt(row, "point");
      element.WinPoint = GetRowInt(row, "winPoint");
      element.LoosePoint = GetRowInt(row, "loosePoint");

      element.Rewards = new List<Consume>();

      foreach (DataRow reward in playerRatingLevelRewardsDatas.Rows)
      {
        var ratingLevel = GetRowInt(reward, "ratingLevel");

        if (string.IsNullOrEmpty(GetRowString(reward, "ratingLevel")))
          break;

        if (!ratingLevel.Equals(element.RatingLevel))
          continue;

        var type = GetRowString(reward, "type");
        var amount = GetRowInt(reward, "amount");

        element.Rewards.Add(new Consume(type, amount));
      }

      Static.PlayerRatingLevelDatas.Add(element);
    }

    DebugX.LogWarning("Load PlayerRatingLevelDatas Complete!");
  }

  // ===================================================================================================
  private static void LoadLevelDatas()
  {
    Static.LevelDatas = new List<LevelData>();

    DataTable levelDatas = TableCollection["LevelDatas"];
    DataTable levelSlotDatas = TableCollection["LevelSlotDatas"];

    foreach (DataRow row in levelDatas.Rows)
    {
      LevelData element = new LevelData();

      if (string.IsNullOrEmpty(GetRowString(row, "level")))
        break;

      element.Level = GetRowString(row, "level");
      element.Prefab = GetRowString(row, "prefab");

      element.SlotDatas = new List<LevelSlotData>();

      foreach (DataRow lsd in levelSlotDatas.Rows)
      {
        var level = GetRowString(lsd, "level");

        if (string.IsNullOrEmpty(level))
          break;

        if (!level.Equals(element.Level))
          continue;

        LevelSlotData levelSlotData = new LevelSlotData();

        levelSlotData.Index = GetRowInt(lsd, "index");
        levelSlotData.BuildingId = GetRowString(lsd, "buildingId");

        element.SlotDatas.Add(levelSlotData);
      }

      Static.LevelDatas.Add(element);
    }

    DebugX.LogWarning("Load LevelDatas Complete!");
  }

  // ===================================================================================================
  private static void LoadLevelProgressDatas()
  {
    Static.LevelProgressDatas = new List<LevelProgressData>();

    DataTable levelProgressDatas = TableCollection["LevelProgressDatas"];

    foreach (DataRow row in levelProgressDatas.Rows)
    {
      LevelProgressData element = new LevelProgressData();

      if (string.IsNullOrEmpty(GetRowString(row, "id")))
        break;

      element.Id = GetRowInt(row, "id");
      element.Prefab = GetRowString(row, "prefab");
      element.LevelDataId = GetRowString(row, "levelDataId");
      Static.LevelProgressDatas.Add(element);
    }

    DebugX.LogWarning("Load LoadLevelProgressDatas Complete!");
  }

    // ===================================================================================================
    private static void LoadProductDatas()
  {
    Static.ProductDatas = new List<ProductData>();

    DataTable productDatas = TableCollection["ProductDatas"];
    DataTable productReceiptDatas = TableCollection["ProductReceiptDatas"];

    foreach (DataRow row in productDatas.Rows)
    {
      ProductData element = new ProductData();

      if (string.IsNullOrEmpty(GetRowString(row, "id")))
        break;

      element.Id = GetRowString(row, "id");
      element.Weight = GetRowInt(row, "weight");
      element.ProductionTime = GetRowInt(row, "productionTime");

      element.Receipt = new List<Consume>();

      foreach (DataRow rec in productReceiptDatas.Rows)
      {
        var id = GetRowString(rec, "id");

        if (string.IsNullOrEmpty(id))
          break;

        if (!id.Equals(element.Id))
          continue;

        Consume consume = new Consume();

        consume.Type = GetRowString(rec, "type");
        consume.Amount = GetRowInt(rec, "amount");

        element.Receipt.Add(consume);
      }

      Static.ProductDatas.Add(element);
    }

    DebugX.LogWarning("Load ProductDatas Complete!");
  }

  // ===================================================================================================
  private static void LoadBuildDatas()
  {
    Static.BuildDatas = new List<BuildData>();

    DataTable buildDatas = TableCollection["BuildDatas"];
    DataTable buildProduceDatas = TableCollection["BuildProduceDatas"];

    foreach (DataRow row in buildDatas.Rows)
    {
      BuildData element = new BuildData();

      if (string.IsNullOrEmpty(GetRowString(row, "id")))
        break;

      element.Id = GetRowString(row, "id");
      element.Type = GetRowString(row, "type");
      element.Prefab = GetRowString(row, "prefab");
      element.SlotCountMax = GetRowInt(row, "slotCountMax");
      element.AutoStart = GetRowBool(row, "autoStart");
      element.ProduceMultiplier = GetRowFloat(row, "produceMultiplier");

      element.Produce = new List<string>();

      foreach (DataRow rec in buildProduceDatas.Rows)
      {
        var buildId = GetRowString(rec, "buildId");

        if (string.IsNullOrEmpty(buildId))
          break;

        if (!buildId.Equals(element.Id))
          continue;

        element.Produce.Add(GetRowString(rec, "productId"));
      }

      Static.BuildDatas.Add(element);
    }

    DebugX.LogWarning("Load BuildDatas Complete!");
  }

  // ===================================================================================================
  private static void LoadItemDatas()
  {
    Static.ItemDatas = new List<ItemData>();

    DataTable itemDatas = TableCollection["ItemDatas"];
    DataTable itemPriceDatas = TableCollection["ItemPriceDatas"];

    foreach (DataRow row in itemDatas.Rows)
    {
      ItemData element = new ItemData();

      if (string.IsNullOrEmpty(GetRowString(row, "id")))
        break;

      element.Id = GetRowString(row, "id");

      if (Enum.TryParse(GetRowString(row, "type"), out EItemDataType type))
      {
        element.Type = type;
      }

      if (Enum.TryParse(GetRowString(row, "storageType"), out EItemDataStorageType storageType))
      {
        element.StorageType = storageType;
      }

      element.TargetId = GetRowString(row, "targetId");
      element.MinPlayerLevel = GetRowInt(row, "minPlayerLevel");


      element.Prices = new List<Consume>();

      foreach (DataRow rec in itemPriceDatas.Rows)
      {
        var id = GetRowString(rec, "id");

        if (string.IsNullOrEmpty(id))
          break;

        if (!id.Equals(element.Id))
          continue;

        Consume consume = new Consume();

        consume.Type = GetRowString(rec, "type");
        consume.Amount = GetRowInt(rec, "amount");

        element.Prices.Add(consume);
      }

      Static.ItemDatas.Add(element);
    }

    DebugX.LogWarning("Load ItemDatas Complete!");
  }

  // ===================================================================================================
  private static void LoadShopDatas()
  {
    Static.ShopDatas = new List<ShopData>();

    DataTable shopDatas = TableCollection["ShopDatas"];
    DataTable shopItemDatas = TableCollection["ShopItemDatas"];

    foreach (DataRow row in shopDatas.Rows)
    {
      ShopData element = new ShopData();

      if (string.IsNullOrEmpty(GetRowString(row, "id")))
        break;

      element.Id = GetRowString(row, "id");
      element.SortOrder = GetRowInt(row, "sortOrder");

      element.ShopItems = new List<ShopItemData>();

      foreach (DataRow rec in shopItemDatas.Rows)
      {
        var shopId = GetRowString(rec, "shopId");

        if (string.IsNullOrEmpty(shopId))
          break;

        if (!shopId.Equals(element.Id))
          continue;

        ShopItemData shopItem = new ShopItemData();

        shopItem.ItemId = GetRowString(rec, "itemId");
        shopItem.SortOrder = GetRowInt(rec, "sortOrder");
        shopItem.IsPopular = GetRowBool(rec, "isPopular");

        element.ShopItems.Add(shopItem);
      }

      Static.ShopDatas.Add(element);
    }

    DebugX.LogWarning("Load ShopDatas Complete!");
  }

  // ===================================================================================================
  private static void LoadOrderDatas()
  {
    Static.OrderDatas = new List<OrderData>();

    DataTable orderDatas = TableCollection["OrderDatas"];
    DataTable orderProductsDatas = TableCollection["OrderProductsDatas"];

    foreach (DataRow row in orderDatas.Rows)
    {
      OrderData element = new OrderData();

      var id = GetRowString(row, "id");

      if (string.IsNullOrEmpty(id))
        break;

      element.MinPlayerLevel = GetRowInt(row, "minPlayerLevel");
      element.MaxPlayerLevel = GetRowInt(row, "maxPlayerLevel");
      element.MinOrderWeight = GetRowInt(row, "minOrderWeight");
      element.MaxOrderWeight = GetRowInt(row, "maxOrderWeight");
      element.OrdersCount = GetRowInt(row, "ordersCount");

      element.Products = new List<string>();

      foreach (DataRow rec in orderProductsDatas.Rows)
      {
        var orderId = GetRowString(rec, "orderId");

        if (string.IsNullOrEmpty(orderId))
          break;

        if (!orderId.Equals(id))
          continue;

        element.Products.Add(GetRowString(rec, "productId"));
      }

      Static.OrderDatas.Add(element);
    }

    DebugX.LogWarning("Load OrderDatas Complete!");
  }

  // ===================================================================================================
  private static void LoadLocalizationDatas()
  {
    Static.LocalisationDatas = new List<LocalisationData>();

    DataTable localisationDatas = TableCollection["LocalisationDatas"];

    foreach (DataRow row in localisationDatas.Rows)
    {
      LocalisationData element = new LocalisationData();

      if (string.IsNullOrEmpty(GetRowString(row, "key")))
        break;

      element.Key = GetRowString(row, "key");
      element.Ru = GetRowString(row, "ru");
      element.En = GetRowString(row, "en");
      element.De = GetRowString(row, "de");
      element.Fr = GetRowString(row, "fr");
      element.Es = GetRowString(row, "es");
      element.Jp = GetRowString(row, "jp");
      element.Zh = GetRowString(row, "zh");

      Static.LocalisationDatas.Add(element);
    }

    DebugX.LogWarning("Load LocalisationDatas Complete!");
  }

  // ===================================================================================================
  private static T EnumParse<T>(string str)
  {
    return (T) Enum.Parse(typeof(T), str);
  }

  #region getValue 
  // ===================================================================================================
  public static string GetRowString(DataRow row, string name)
  {
    try
    {
      var str = row.Field<string>(name);

      if (str.Equals("null") || string.IsNullOrEmpty(str))
        return null;

      return row.Field<string>(name);
    }
    catch
    {
      try
      {
        return row.Field<double>(name).ToString();
      }
      catch
      {
        return string.Empty;
      }
    }
  }

  // ===================================================================================================
  public static bool GetRowBool(DataRow row, string name)
  {
    try
    {
      return row.Field<bool>(name);
    }
    catch
    {
      return false;
    }
  }

  // ===================================================================================================
  public static long GetRowLong(DataRow row, string name)
  {
    try
    {
      return (long) row.Field<double>(name);
    }
    catch
    {
      return 0;
    }
  }

  // ===================================================================================================
  public static int GetRowInt(DataRow row, string name)
  {
    try
    {
      return (int) row.Field<double>(name);
    }
    catch
    {
      return 0;
    }
  }

  // ===================================================================================================
  public static double GetRowDouble(DataRow row, string name)
  {
    try
    {
      return row.Field<double>(name);
    }
    catch
    {
      return 0;
    }
  }

  // ===================================================================================================
  public static float GetRowFloat(DataRow row, string name)
  {
    try
    {
      return (float) row.Field<double>(name);
    }
    catch
    {
      return 0;
    }
  }
  #endregion
}
