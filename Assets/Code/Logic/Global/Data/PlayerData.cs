using System.Collections.Generic;
using Newtonsoft.Json;
using Code.ProgressLevel;
using Static;
using static LocalizeManager;

namespace Player
{
  public enum  EPlayerSlotType
  {
  Body,
  Head,
  Battle
  }
  
  public class PlayerData
  {
    [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
    public int Level = 1;

    [JsonProperty("experience", NullValueHandling = NullValueHandling.Ignore)]
    public int Experience = 0;

    [JsonProperty("ratingLevel", NullValueHandling = NullValueHandling.Ignore)]
    public int RatingLevel = 1;

    [JsonProperty("ratingLevelGetReward", NullValueHandling = NullValueHandling.Ignore)]
    public int RatingLevelGetReward = 0;

    [JsonProperty("ratingPoint", NullValueHandling = NullValueHandling.Ignore)]
    public int RatingPoint = 0;

    // Статический ИНВЕНТАРЬ - Список расходников типа золота, кристаллов и других предметов
    [JsonProperty("storage", NullValueHandling = NullValueHandling.Ignore)]
    public List<Consume> Storage = new List<Consume>();

    // Временный ИНВЕНТАРЬ - В который записываются получнные расходники пока летят по аниции, чтобы при ошибке и выходе из приложения не потерять заработок.
    [JsonProperty("waitableStorage", NullValueHandling = NullValueHandling.Ignore)]
    public List<Consume> WaitableStorage = new List<Consume>();

    //Слоты персонажа
    [JsonProperty("slots", NullValueHandling = NullValueHandling.Ignore)]
    public List<PlayerSlot> Slots = new List<PlayerSlot>();
    
    [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
    public ELanguage Language = ELanguage.RU;

    [JsonProperty("levelProgress",NullValueHandling = NullValueHandling.Ignore)]
    public List<Level> Levels = new List<Level>();

    [JsonProperty("levelStars", NullValueHandling = NullValueHandling.Ignore)]
    public List<LevelStarsData> LevelStars = new List<LevelStarsData>();
  }

  // ===================================================================================================
  public class PlayerSlot
  {
    [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
    public int Index;

    [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    public EPlayerSlotType Type;

    [JsonProperty("consume", NullValueHandling = NullValueHandling.Ignore)]
    public Consume Consume;

    public bool WaitConsume;

    // =====
    [JsonConstructor]
    public PlayerSlot(int index, EPlayerSlotType type)
    {
      Index = index;
      Type = type;
    }

    // =====
    public PlayerSlot(int index, EPlayerSlotType type, Consume consume)
    {
      Index = index;
      Type = type;
      Consume = consume;
    }
  }
  
  public class LevelStarsData
  {
    public int CountStars;

    [JsonConstructor]
    public LevelStarsData(int stars)
    {
        CountStars = stars;
    }
  }
}
