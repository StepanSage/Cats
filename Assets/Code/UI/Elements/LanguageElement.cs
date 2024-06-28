using Objects;
using System;
using UnityEngine;
using UnityEngine.UI;
using static LocalizeManager;

public class LanguageElement : MonoBehaviour
{
  [SerializeField]
  private Image _icon = null;
  [SerializeField]
  private GameObject _light;

  private ELanguage _language;
  private Action<ELanguage> _onLanguageClick;

  // ===================================================================================================
  public void Init(ELanguage language, Action<ELanguage> onLanguageClick)
  {
    _language = language;
    _onLanguageClick = onLanguageClick;
    Redraw();
    gameObject.SetActive(true);
  }

  // ===================================================================================================
  public void Redraw()
  {
    _icon.sprite = Factory.Instance.GetIcon(_language.ToString().ToLower());
    _light.SetActive(_language.Equals(Global.Instance.PlayerData.Language));
  }

  // ===================================================================================================
  public void BtnLevelSelect_Click()
  {
    _onLanguageClick?.Invoke(_language);
  }

  // ===================================================================================================
  public void OnEnable()
  {
    LocalizeManager.Instance.OnLanguageChange += OnLanguageChange;
  }

  // ===================================================================================================
  public void OnDisable()
  {
    if (LocalizeManager.Instance != null)
    {
      LocalizeManager.Instance.OnLanguageChange -= OnLanguageChange;
    }
  }

  // ===================================================================================================
  public void OnLanguageChange(ELanguage language)
  {
    _light.SetActive(_language.Equals(language));
  }
}
