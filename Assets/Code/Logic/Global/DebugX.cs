using System;
using UnityEngine;

public class DebugX
{
  public enum EDebugXColor
  {
    White,
    Yellow,
    Red,
    Blue,
    Green,
    Orange,
    LightBlue,
    LightGreen,
    Grey
  }

  public static bool LogForViewEnable;
  public static bool LogForBehaviourEnable;
  public static bool LogForUtilsEnable;
  public static bool LogForUIEnable;
  public static bool LogForEventsEnable;
  public static bool LogForBattleEnable;

  public static void Init()
  {
    LogForViewEnable = true;
    LogForBehaviourEnable = true;
    LogForUtilsEnable = true;
    LogForUIEnable = true;
    LogForEventsEnable = true;
    LogForBattleEnable = true;
  }


  #region LogColor
  // ===================================================================================================
  public static void LogColorGrey(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.Grey);
  }

  // ===================================================================================================
  public static void LogColorLightGreen(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.LightGreen);
  }

  // ===================================================================================================
  public static void LogColorWhite(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.White);
  }

  // ===================================================================================================
  public static void LogColorYellow(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.Yellow);
  }

  // ===================================================================================================
  public static void LogColorRed(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.Red);
  }

  // ===================================================================================================
  public static void LogColorBlue(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.Blue);
  }

  // ===================================================================================================
  public static void LogColorGreen(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.Green);
  }

  // ===================================================================================================
  public static void LogColorOrange(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.Orange);
  }

  // ===================================================================================================
  public static void LogColorLightBlue(object message, bool bold = false)
  {
    LogColor(message, bold, EDebugXColor.LightBlue);
  }
  #endregion LogColor

  #region LogColorByType
  // ===================================================================================================
  public static void LogForView(object message, bool bold = false, bool error = false)
  {
    if (!LogForViewEnable)
      return;

    message = (error ? "<b>ERR: </b>" : "") + $"<b>TAG_VIEW:</b> {message}";
    LogColor(message, bold, EDebugXColor.Blue);
  }

  // ===================================================================================================
  public static void LogForBehaviour(object message, bool bold = false, bool error = false)
  {
    if (!LogForBehaviourEnable)
      return;

    message = (error ? "<b>ERR: </b>" : "") + $"<b>TAG_BEH:</b> {message}";
    LogColor(message, bold, EDebugXColor.Orange);
  }

  // ===================================================================================================
  public static void LogForUtils(object message, bool bold = false, bool error = false)
  {
    if (!LogForUtilsEnable)
      return;

    message = (error ? "<b>ERR: </b>" : "") + $"<b>TAG_UTILS:</b> {message}";
    LogColor(message, bold, EDebugXColor.Yellow);
  }

  // ===================================================================================================
  public static void LogForUI(object message, bool bold = false, bool error = false)
  {
    if (!LogForUIEnable)
      return;

    message = (error ? "<b>ERR: </b>" : "") + $"<b>TAG_UI:</b> {message}";
    LogColor(message, bold, EDebugXColor.Green);
  }

  // ===================================================================================================
  public static void LogForEvents(object message, bool bold = false, bool error = false)
  {
    if (!LogForEventsEnable)
      return;

    message = (error ? "<b>ERR: </b>" : "") + $"<b>TAG_Event:</b> {message}";
    LogColor(message, bold, EDebugXColor.LightBlue);
  }

  // ===================================================================================================
  public static void LogForBattle(object message, bool bold = false, bool error = false)
  {
    if (!LogForBattleEnable)
      return;

    message = (error ? "<b>ERR: </b>" : "") + $"<b>TAG_Battle:</b> {message}";
    LogColor(message, bold, EDebugXColor.Red);
  }
  #endregion LogColorByType

  // ===================================================================================================
  public static void LogColor(object message, bool bold = false, EDebugXColor color = EDebugXColor.White)
  {
    string msg = $"<color={color.ToString().ToLower()}>{message}</color>";

    if (bold)
    {
      msg = $"<b>{msg}</b>";
    }

    Debug.Log(msg);
  }

  // ===================================================================================================
  public static void LogError(string message, Exception e)
  {
    Debug.LogError($"{message}: {e.Message} ({e.StackTrace})");
  }

  // ===================================================================================================
  public static void LogError(object message)
  {
    Debug.LogError(message);
  }

  // ===================================================================================================
  public static void Log(object message)
  {
    Debug.Log(message);
  }

  // ===================================================================================================
  public static void LogWarning(object message)
  {
    Debug.LogWarning(message);
  }
}