using UnityEngine;

public class ButtonSettings : MonoBehaviour
{
  // ===================================================================================================
  public void BtnSettings_Click()
  {
    DialogSystem.Instance.Get<DialogSettings>()?.Activate();
  }
}
