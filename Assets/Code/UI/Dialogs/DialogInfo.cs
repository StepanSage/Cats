using System;
using TMPro;
using UnityEngine;

public class DialogInfo : Dialog
{
  [SerializeField]
  private TextMeshProUGUI _descriptionText = null;
  [SerializeField]
  private TextMeshProUGUI _btnOkText = null;

  private Action _callbackOk;

  // ===================================================================================================
  public override bool CanBackClose()
  {
    BtnOk_Click();
    return false;
  }

  // ===================================================================================================
  protected void Awake()
  {
    _group = (byte) SortingGroup.Top;
  }

  // ===================================================================================================
  public void Activate(string descriptionText, string btnOkText, Action callbackResult = null)
  {
    _callbackOk = callbackResult;
    _descriptionText.text = descriptionText;
    _btnOkText.text = btnOkText;
    Open();
  }

  // ===================================================================================================
  public void BtnOk_Click()
  {
    if (IsProcessing)
      return;

    Close();
    _callbackOk?.Invoke();
  }
}