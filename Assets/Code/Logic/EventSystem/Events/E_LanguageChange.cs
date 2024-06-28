using Static;
using static LocalizeManager;

public class E_LanguageChange : EventBase
{
  public ELanguage Language;

  public E_LanguageChange(ELanguage language)
  {
    DebugX.LogForEvents($"E_LanguageChange : SEND");
    Language = language;
  }
}