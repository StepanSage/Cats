using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class LocalizeManager : Singleton<LocalizeManager>
{
  public enum ELanguage
  {
    RU,
    EN,
    FR,
    JP,
    DE,
    ES
  }

  public ELanguage CurrentLanguage => currentLanguage;
  public Action<ELanguage> OnLanguageChange;
  private ELanguage currentLanguage;

  // ===================================================================================================
  public void SetLanguage(ELanguage lang)
  {
    currentLanguage = lang;
    Global.Instance.PlayerData.Language = lang;
    OnLanguageChange?.Invoke(currentLanguage);
  }

  // ===================================================================================================
  public string GetOrEmpty(string wordKey)
  {
    var str = Get(wordKey);
    if (str.Equals(wordKey))
    {
      return "";
    }
    return str;
  }

  // ===================================================================================================
  public string Get(string wordKey)
  {
    if(Global.Instance.StaticData.LocalisationDatas == null)
      return wordKey;

    var locData = Global.Instance.StaticData.LocalisationDatas.Where(ld => ld.Key.Equals(wordKey));

    if (locData.Any())
    {
      var ldt = locData.FirstOrDefault();
      string defaultWord = "";
      if (ldt.En != null)
      {
        defaultWord = ldt.En;
      }
      switch (currentLanguage)
      {
        case ELanguage.RU:
          if (ldt.Ru == null)
          {
            if (ldt.En != null)
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for language \"{currentLanguage.ToString()}\" not filled. Key: {wordKey}");
              return ldt.En;
            }
            else
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for key \"{wordKey}\" not found.");
              return wordKey;
            }
          }
          else
          {
            return ldt.Ru;
          }
        case ELanguage.EN:
          if (ldt.En != null)
          {
            return ldt.En;
          }
          else
          {
            Debug.LogWarning($"{GetType().ToString()}: Localization for key \"{wordKey}\" not found.");
            return wordKey;
          }
        case ELanguage.FR:
          if (ldt.Fr == null)
          {
            if (ldt.En != null)
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for language \"{currentLanguage.ToString()}\" not filled. Key: {wordKey}");
              return ldt.En;
            }
            else
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for key \"{wordKey}\" not found.");
              return wordKey;
            }
          }
          else
          {
            return ldt.Fr;
          }
        case ELanguage.JP:
          if (ldt.Jp == null)
          {
            if (ldt.En != null)
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for language \"{currentLanguage.ToString()}\" not filled. Key: {wordKey}");
              return ldt.En;
            }
            else
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for key \"{wordKey}\" not found.");
              return wordKey;
            }
          }
          else
          {
            return ldt.Jp;
          }
        case ELanguage.DE:
          if (ldt.De == null)
          {
            if (ldt.En != null)
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for language \"{currentLanguage.ToString()}\" not filled. Key: {wordKey}");
              return ldt.En;
            }
            else
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for key \"{wordKey}\" not found.");
              return wordKey;
            }
          }
          else
          {
            return ldt.De;
          }
        case ELanguage.ES:
          if (ldt.Es == null)
          {
            if (ldt.En != null)
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for language \"{currentLanguage.ToString()}\" not filled. Key: {wordKey}");
              return ldt.En;
            }
            else
            {
              Debug.LogWarning($"{GetType().ToString()}: Localization for key \"{wordKey}\" not found.");
              return wordKey;
            }
          }
          else
          {
            return ldt.Es;
          }
      }
    }
    return wordKey;
  }

  // ===================================================================================================
  public bool Has(string wordKey)
  {
    var locData = Global.Instance.StaticData.LocalisationDatas.Where(ld => ld.Key.Equals(wordKey));
    return locData != null;
  }

  // ===================================================================================================
  public string Get(string wordKey, Dictionary<string, string> values)
  {
    string locText = "";

    var locData = Global.Instance.StaticData.LocalisationDatas.Where(ld => ld.Key.Equals(wordKey));

    if (locData.Any())
    {
      var ldt = locData.FirstOrDefault();
      switch (currentLanguage)
      {
        case ELanguage.RU:
          locText = ldt.Ru;
          break;
        case ELanguage.EN:
          locText = ldt.En;
          break;
        case ELanguage.FR:
          locText = ldt.Fr;
          break;
        case ELanguage.JP:
          locText = ldt.Jp;
          break;
        case ELanguage.DE:
          locText = ldt.De;
          break;
        case ELanguage.ES:
          locText = ldt.Es;
          break;
        default:
          locText = "";
          break;
      }
    }

    if (string.IsNullOrEmpty(locText))
      return wordKey;

    string pattern = @"[r,n]\[[^\]]*?\]";
    Regex rgx = new Regex(pattern);
    string result = "";

    foreach (Match match in rgx.Matches(locText))
    {
      result = match.Value.Substring(2, match.Value.Length - 3);
      var results = result.Split(',');


      switch (currentLanguage)
      {
        //ru
        case ELanguage.RU:

          string value = values[results[0]];
          string sub1 = (value.Length >= 1 ? value.Substring(value.Length - 1) : "");
          string sub2 = (value.Length >= 2 ? value.Substring(value.Length - 2) : "");

          if (results.Length == 1)
          {
            locText = locText.Replace(match.Value, values[results[0]]);
          }
          else if (sub1.Equals("1") && !sub2.Equals("11"))
          {
            locText = locText.Replace(match.Value, values[results[0]] + " " + results[1]);
          }
          else if (
                    (sub1.Equals("2") && !sub2.Equals("12")) ||
                    (sub1.Equals("3") && !sub2.Equals("13")) ||
                    (sub1.Equals("4") && !sub2.Equals("14"))
                  )
          {
            locText = locText.Replace(match.Value, values[results[0]] + " " + results[2]);
          }
          else
          {
            locText = locText.Replace(match.Value, values[results[0]] + " " + results[3]);
          }
          break;

        //other
        default:
          if (results.Length == 1)
          {
            locText = locText.Replace(match.Value, values[results[0]]);
          }
          else
          {
            if (short.Parse(values[results[0]]) <= 1)
            {
              locText = locText.Replace(match.Value, values[results[0]] + " " + results[1]);
            }
            else if (short.Parse(values[results[0]]) <= 4)
            {
              locText = locText.Replace(match.Value, values[results[0]] + " " + results[2]);
            }
            else
            {
              locText = locText.Replace(match.Value, values[results[0]] + " " + results[3]);
            }
          }
          break;
      }
    }
    return locText;
  }
}
