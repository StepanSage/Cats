using DG.Tweening;
using UnityEngine;
using Player;

public class DialogScaleAnimation : DialogAnimator
{
  [SerializeField]
  private GameObject _bg = null;
  [SerializeField]
  private GameObject _content = null;

  public bool ChangeScale = true;
  public bool ChangeAlpha = true;

  private CanvasGroup _canvasGroup;
  
  // ===================================================================================================
  private void InitCanvasGroup()
  {
    if (_canvasGroup != null) 
      return;
    _canvasGroup = gameObject.GetComponent<CanvasGroup>();
    if (_canvasGroup == null)
    {
      _canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
  }
    
  // ===================================================================================================
  public override void Init()
  {
    InitCanvasGroup();
    
    if (ChangeScale)
    {
      _content.transform.localScale = Vector3.one;
    }
    if (ChangeAlpha)
    {
      _canvasGroup.alpha = 0f;
    }
  }

  // ===================================================================================================
  public override void AnimateShow(float time)
  {
    if (time <= 0)
    {
      if (ChangeScale)
        _content.transform.localScale = Vector3.one;
      if (ChangeAlpha)
        _canvasGroup.alpha = 1f;
    }
    else
    {
      if (ChangeScale)
      {
        _content.transform.DOScale(Vector3.one, time)
          .ChangeValues(Vector3.zero, Vector3.one, time)
          .SetEase(Ease.OutBack)
          .Play();
      }
      if (ChangeAlpha)
      {
        _canvasGroup.DOFade(1f, time)
          .ChangeValues(0f, 1f, time)
          .SetEase(Ease.OutBack)
          .Play();
      }
    }
  }

  // ===================================================================================================
  public override void AnimateHide(float time)
  {
    if (time <= 0)
    {
      if (ChangeScale)
        _content.transform.localScale = Vector3.zero;
      if (ChangeAlpha)
        _canvasGroup.alpha = 0f;
    }
    else
    {
      if (ChangeScale)
      {
        _content.transform.DOScale(Vector3.one * 0.01f, time)
          .ChangeValues(Vector3.one, Vector3.one * 0.01f, time)
          .SetEase(Ease.InBack)
          .Play();
      }
      if (ChangeAlpha)
      {
        _canvasGroup.DOFade(0f, time)
          .ChangeValues(1f, 0f, time)
          .SetEase(Ease.InBack)
          .Play();
      }
    }
  }
}
