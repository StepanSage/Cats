using Newtonsoft.Json;
using System.Collections.Generic;

namespace Static
{
  // ===================================================================================================
  public enum EConstType
  {
    PLAYER_SLOT_MAX,
    CAMERA_SPEED,
    CAMERA_VISIBLE
  }

  // ===================================================================================================
  public enum EItemDataType
  {
    Suit,
    BuildingDecor,
    Resource
  }

  // ===================================================================================================
  public enum EItemDataStorageType
  {
    Storage,
    Slot,
    Param
  }

  // ===================================================================================================
  public class StaticData
  {
    [JsonProperty("levelDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<LevelData> LevelDatas { get; set; }

    [JsonProperty("productDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<ProductData> ProductDatas { get; set; }

    [JsonProperty("playerLevelDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<PlayerLevelData> PlayerLevelDatas { get; set; }

    [JsonProperty("playerRatingLevelDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<PlayerRatingLevelData> PlayerRatingLevelDatas { get; set; }

    [JsonProperty("orderDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<OrderData> OrderDatas { get; set; }

    [JsonProperty("constDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<ConstData> ConstDatas { get; set; }

    [JsonProperty("buildDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<BuildData> BuildDatas { get; set; }

    [JsonProperty("itemDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<ItemData> ItemDatas { get; set; }

    [JsonProperty("shopDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<ShopData> ShopDatas { get; set; }

    [JsonProperty("levelProgressDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<LevelProgressData> LevelProgressDatas { get; set; }

    [JsonProperty("levelProgressConfigDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<LevelProgressConfigData> LevelProgressConfigDatas { get; set; }

    [JsonProperty("localisationDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<LocalisationData> LocalisationDatas { get; set; }
  }

  // ===================================================================================================
  public class ShopData
  {
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("sortOrder", NullValueHandling = NullValueHandling.Ignore)]
    public int SortOrder { get; set; }

    [JsonProperty("shopItems", NullValueHandling = NullValueHandling.Ignore)]
    public List<ShopItemData> ShopItems { get; set; }
  }

  // ===================================================================================================
  public class ShopItemData
  {
    [JsonProperty("itemId", NullValueHandling = NullValueHandling.Ignore)]
    public string ItemId { get; set; }

    [JsonProperty("sortOrder", NullValueHandling = NullValueHandling.Ignore)]
    public int SortOrder { get; set; }

    [JsonProperty("isPopular", NullValueHandling = NullValueHandling.Ignore)]
    public bool IsPopular { get; set; }
  }

  // ===================================================================================================
  public class ItemData
  {
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public EItemDataType Type { get; set; }

    [JsonProperty("storageType", NullValueHandling = NullValueHandling.Ignore)]
    public EItemDataStorageType StorageType { get; set; }

    [JsonProperty("targetId", NullValueHandling = NullValueHandling.Ignore)]
    public string TargetId { get; set; }

    [JsonProperty("prices", NullValueHandling = NullValueHandling.Ignore)]
    public List<Consume> Prices { get; set; }

    [JsonProperty("minPlayerLevel", NullValueHandling = NullValueHandling.Ignore)]
    public int MinPlayerLevel { get; set; }
  }

  // ===================================================================================================
  public class OrderData
  {
    [JsonProperty("minPlayerLevel", NullValueHandling = NullValueHandling.Ignore)]
    public int MinPlayerLevel { get; set; }

    [JsonProperty("maxPlayerLevel", NullValueHandling = NullValueHandling.Ignore)]
    public int MaxPlayerLevel { get; set; }

    [JsonProperty("ordersCount", NullValueHandling = NullValueHandling.Ignore)]
    public int OrdersCount { get; set; }

    [JsonProperty("minOrderWeight", NullValueHandling = NullValueHandling.Ignore)]
    public int MinOrderWeight { get; set; }

    [JsonProperty("maxOrderWeight", NullValueHandling = NullValueHandling.Ignore)]
    public int MaxOrderWeight { get; set; }

    [JsonProperty("products", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> Products { get; set; }
  }

  // ===================================================================================================
  public class PlayerLevelData
  {
    [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
    public int Level { get; set; }

    [JsonProperty("expForLevel", NullValueHandling = NullValueHandling.Ignore)]
    public int ExpForLevel { get; set; }

    [JsonProperty("rewards", NullValueHandling = NullValueHandling.Ignore)]
    public List<Consume> Rewards { get; set; }

    [JsonProperty("battleSlotCount", NullValueHandling = NullValueHandling.Ignore)]
    public int BattleSlotCount { get; set; }
  }

  // ===================================================================================================
  public class PlayerRatingLevelData
  {
    [JsonProperty("ratingLevel", NullValueHandling = NullValueHandling.Ignore)]
    public int RatingLevel { get; set; }

    [JsonProperty("point", NullValueHandling = NullValueHandling.Ignore)]
    public int Point { get; set; }

    [JsonProperty("winPoint", NullValueHandling = NullValueHandling.Ignore)]
    public int WinPoint { get; set; }

    [JsonProperty("loosePoint", NullValueHandling = NullValueHandling.Ignore)]
    public int LoosePoint { get; set; }

    [JsonProperty("rewards", NullValueHandling = NullValueHandling.Ignore)]
    public List<Consume> Rewards { get; set; }
  }

  // ===================================================================================================
  public class LevelData
  {
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Level { get; set; }

    [JsonProperty("prefab", NullValueHandling = NullValueHandling.Ignore)]
    public string Prefab { get; set; }

    [JsonProperty("slotDatas", NullValueHandling = NullValueHandling.Ignore)]
    public List<LevelSlotData> SlotDatas { get; set; }
}

  // ===================================================================================================
  public class ProductData
  {
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("weight", NullValueHandling = NullValueHandling.Ignore)]
    public int Weight { get; set; }

    [JsonProperty("productionTime", NullValueHandling = NullValueHandling.Ignore)]
    public int ProductionTime { get; set; }

    [JsonProperty("receipt", NullValueHandling = NullValueHandling.Ignore)]
    public List<Consume> Receipt { get; set; }
  }

  // ===================================================================================================
  public class LevelSlotData
  {
    [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
    public int Index { get; set; }

    [JsonProperty("buildingId", NullValueHandling = NullValueHandling.Ignore)]
    public string BuildingId { get; set; }
  }

  // ===================================================================================================
  public class ConstData
  {
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public EConstType Id { get; set; }

    [JsonProperty("intValue", NullValueHandling = NullValueHandling.Ignore)]
    public long IntValue { get; set; }

    [JsonProperty("floatValue", NullValueHandling = NullValueHandling.Ignore)]
    public double FloatValue { get; set; }

    [JsonProperty("stringValue", NullValueHandling = NullValueHandling.Ignore)]
    public string StringValue { get; set; }

    [JsonProperty("boolValue", NullValueHandling = NullValueHandling.Ignore)]
    public bool BoolValue { get; set; }
  }

  // ===================================================================================================
  public class BuildData
  {
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public string Id { get; set; }

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public string Type { get; set; }

    [JsonProperty("prefab", NullValueHandling = NullValueHandling.Ignore)]
    public string Prefab { get; set; }

    [JsonProperty("slotCountMax", NullValueHandling = NullValueHandling.Ignore)]
    public int SlotCountMax { get; set; }

    [JsonProperty("autoStart", NullValueHandling = NullValueHandling.Ignore)]
    public bool AutoStart { get; set; }

    [JsonProperty("produceMultiplier", NullValueHandling = NullValueHandling.Ignore)]
    public float ProduceMultiplier { get; set; } = 1f;

    [JsonProperty("produce", NullValueHandling = NullValueHandling.Ignore)]
    public List<string> Produce { get; set; }
  }

  // ===================================================================================================
  public class LevelProgressData
  {
    [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    public int Id { get; set; }

    [JsonProperty("prefab", NullValueHandling = NullValueHandling.Ignore)]
    public string Prefab { get; set; }

    [JsonProperty("levelDataId", NullValueHandling = NullValueHandling.Ignore)]
    public string LevelDataId { get; set; }
  }
    // ===================================================================================================
    public class LevelProgressConfigData
    {
        [JsonProperty("CameraSpeedData", NullValueHandling = NullValueHandling.Ignore)]
        public float CameraSpeedData { get;set; }

        [JsonProperty("CameraVisibleData", NullValueHandling = NullValueHandling.Ignore)]
        public float CameraVisible { get; set; }

    }

    // ===================================================================================================
    public class LocalisationData
  {
    [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
    public string Key { get; set; }

    [JsonProperty("ru", NullValueHandling = NullValueHandling.Ignore)]
    public string Ru { get; set; }

    [JsonProperty("en", NullValueHandling = NullValueHandling.Ignore)]
    public string En { get; set; }

    [JsonProperty("de", NullValueHandling = NullValueHandling.Ignore)]
    public string De { get; set; }

    [JsonProperty("fr", NullValueHandling = NullValueHandling.Ignore)]
    public string Fr { get; set; }

    [JsonProperty("es", NullValueHandling = NullValueHandling.Ignore)]
    public string Es { get; set; }

    [JsonProperty("Jp", NullValueHandling = NullValueHandling.Ignore)]
    public string Jp { get; set; }

    [JsonProperty("Zh", NullValueHandling = NullValueHandling.Ignore)]
    public string Zh { get; set; }
  }
}

