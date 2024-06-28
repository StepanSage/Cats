using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
  public string key = "";
  private TextMeshProUGUI m_textMeshProUGUI;
  //private bool IsLocalizeAvaliable = false;

  // ===================================================================================================
  void Awake()
  {
    m_textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    LocalizeManager.Instance.OnLanguageChange += OnLanguageChange;
  }

  // ===================================================================================================
  void OnDestroy()
  {
    if (LocalizeManager.Instance != null)
    {
      LocalizeManager.Instance.OnLanguageChange -= OnLanguageChange;
    }
  }

  // ===================================================================================================
  void Start()
  {
      Refresh();
  }

  // ===================================================================================================
  void OnEnable()
  {
      Refresh();
  }

  // ===================================================================================================
  void Refresh()
  {
    m_textMeshProUGUI.text = LocalizeManager.Instance.Get(key);
    Canvas.ForceUpdateCanvases();
  }

  // ===================================================================================================
  void OnLanguageChange(LocalizeManager.ELanguage lang)
  {
    m_textMeshProUGUI.text = LocalizeManager.Instance.Get(key);
    Canvas.ForceUpdateCanvases();
  }
}
