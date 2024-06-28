using UnityEngine;
using Player;
using Static;
using GameLogic;

public class Global : Singleton <Global>
{
  public enum InputPlatform
  {
    Mouse,
    Touch,
    Multitouch,
    WebMultitouch
  }

  public long Time
  {
    get
    {
      return (long) TimeUtils.GetUnixTime();
    }
  }

  public Game Game;

  public InputPlatform InpPlatform { get; set; } = InputPlatform.Mouse;

  public StaticData StaticData = new StaticData();
  public PlayerData PlayerData;

  public SoundService SoundService;

  public AdsBase Ads;
}