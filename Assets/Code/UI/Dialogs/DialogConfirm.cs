using System;
using TMPro;
using UnityEngine;

public class DialogConfirm : Dialog
{
  [SerializeField]
  private TextMeshProUGUI _descriptionText = null;
  [SerializeField]
  private TextMeshProUGUI _btnOkText = null;
  [SerializeField]
  private TextMeshProUGUI _btnCancelText = null;

  private Action<bool> _callbackResult;

  // ===================================================================================================
  public override bool CanBackClose()
  {
    BtnCancel_Click();
    return false;
  }

  // ===================================================================================================
  protected void Awake()
  {
    _group = (byte) SortingGroup.MaxCommonTop;
  }

  // ===================================================================================================
  public void Activate(string descriptionText, string btnOkText, string btnCancelText, Action<bool> callbackResult = null)
  {
    _callbackResult = callbackResult;
    _descriptionText.text = descriptionText;
    _btnOkText.text = btnOkText;
    _btnCancelText.text = btnCancelText;

    Open();
  }

  // ===================================================================================================
  public void BtnOk_Click()
  {
    if (IsProcessing)
      return;

    Close();
    _callbackResult?.Invoke(true);
  }

  // ===================================================================================================
  public void BtnCancel_Click()
  {
    if (IsProcessing)
      return;

    Close();
    _callbackResult?.Invoke(false);
  }
}