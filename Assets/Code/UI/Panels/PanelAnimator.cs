using UnityEngine;

public abstract class PanelAnimator : MonoBehaviour
{
  public abstract void Init();
  public abstract void AnimateShow(float time);
  public abstract void AnimateHide(float time);
}