using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
  public Action OnGetFinish;
  public Action OnUseFinish;

  // ===================================================================================================
  public void GetFinish()
  {
    OnGetFinish?.Invoke();
  }

  // ===================================================================================================
  public void UseFinish()
  {
    OnUseFinish?.Invoke();
  }
}
