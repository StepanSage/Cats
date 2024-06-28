using Static;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
  [SerializeField]
  private GameObject _content;

  [SerializeField]
  private Button _btnMenu;
  [SerializeField]
  private Button _btnSettings;
  [SerializeField]
  private Button _btnExit;
  [SerializeField]
  private Button _btnTest;

  // ===================================================================================================
  private void OnEnable()
  {
    _content.SetActive(false);

    _btnMenu.onClick.AddListener(OnBtnMenuClick);
    _btnSettings.onClick.AddListener(OnBtnSettingsClick);
    _btnExit.onClick.AddListener(OnBtnExitClick);
    _btnTest.onClick.AddListener(OnBtnTestClick);
  }

  // ===================================================================================================
  private void OnDisable()
  {
    _btnMenu.onClick.RemoveListener(OnBtnMenuClick);
    _btnSettings.onClick.RemoveListener(OnBtnSettingsClick);
    _btnExit.onClick.RemoveListener(OnBtnExitClick);
    _btnTest.onClick.RemoveListener(OnBtnTestClick);
  }

  // ===================================================================================================
  public void OnBtnMenuClick()
  {
    _content.SetActive(!_content.activeSelf);
  }

  // ===================================================================================================
  public void OnBtnSettingsClick()
  {
    DialogSystem.Instance.Get<DialogSettings>()?.Activate();
  }

  // ===================================================================================================
  public void OnBtnExitClick()
  {
    Application.Quit();
  }

  // ===================================================================================================
  public void OnBtnTestClick()
  {
#if UNITY_EDITOR
    ProjectUtils.AnimatedReward(
      new List<Consume>()
      {
      new Consume ("gold",500),
      new Consume ("crystal",500),
      new Consume ("exp",500),
      new Consume ("rating_point",500)
      },
      _content.transform.position,
      Vector3.negativeInfinity,
      false
      );
#endif
  }
}
