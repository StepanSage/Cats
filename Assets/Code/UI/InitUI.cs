using UnityEngine;

public class InitUI : MonoBehaviour
{
  public Loading DialogLoading;

  // ===================================================================================================
  public void Init()
  {
    DialogLoading?.Init();
  }
}