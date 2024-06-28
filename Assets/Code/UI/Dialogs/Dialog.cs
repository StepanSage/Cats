using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class Dialog : MonoBehaviour
{
  public string Name => this.GetType().ToString();
  public Type Type => this.GetType();
  
  [SerializeField]
  private DialogAnimator _animator = null;
  public byte Group => _group;
  public DialogMode Mode => _mode;

  protected byte _group = (byte) SortingGroup.Default;
  protected DialogMode _mode = DialogMode.Normal;

  [SerializeField, HideInInspector]
  public float ShowTime = 0f;

  public bool IsCloseProcessing => _closeProccessing;
  public bool IsOpenProcessing => _openProccessing;
  public bool IsProcessing => (_closeProccessing || _openProccessing);
  public bool IsOpen => (!IsProcessing && gameObject.activeSelf);
  public bool IsClose => (!IsProcessing && !gameObject.activeSelf);

  protected float AnimationSpeed = 0.3f;
  protected UnityEvent OnStartShow = new UnityEvent();
  protected UnityEvent OnStartHide = new UnityEvent();
  protected UnityEvent OnShow = new UnityEvent();
  protected UnityEvent OnHide = new UnityEvent();

  private DialogSystem _dlgSystem;
  private bool _inited = false;
  private bool _openProccessing = false;
  private bool _closeProccessing = false;
  private Action _onShowCallback;
  private Action _onHideCallback;

  // ===================================================================================================
  public virtual void Init(DialogSystem owner)
  {
    if (_inited)
      return;

    _dlgSystem = owner;

    _openProccessing = false;
    _closeProccessing = false;
    gameObject.SetActive(false);
    _animator.Init();

    _inited = true;
  }

  // ===================================================================================================
  public virtual void Show(Action showCallback = null)
  {
    _onShowCallback = showCallback;
    if (_openProccessing || IsOpen)
    {
      try
      {
        _onShowCallback?.Invoke();
      }
      catch (Exception e) { Debug.LogError($"Error in Dialog [{this.GetType().ToString()}] show callback: {e.ToString()}"); }
      return;
    }

    CancelInvoke();
    if (_closeProccessing)
    {
      OnHideInvoke();
    }

    _closeProccessing = false;
    _openProccessing = true;

    try
    {
      OnStartShow?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Dialog [{this.GetType().ToString()}] OnStartShow event: {e.ToString()}"); }

    if (!gameObject.activeSelf)
      gameObject.SetActive(true);

    _animator.AnimateShow(AnimationSpeed);
    
    if (AnimationSpeed <= 0)
    {
      OnShowInvoke();
    }
    else
    {
      Invoke(nameof(OnShowInvoke), AnimationSpeed);
    }
  }

  // ===================================================================================================
  public virtual void Hide(Action hideCallback = null)
  {
    _onHideCallback = hideCallback;
    if (_closeProccessing || IsClose)
    {
      try
      {
        _onHideCallback?.Invoke();
      }
      catch (Exception e) { Debug.LogError($"Error in Dialog [{this.GetType().ToString()}] hide callback: {e.ToString()}"); }
      return;
    }

    CancelInvoke();

    _openProccessing = false;
    _closeProccessing = true;
    
    try
    {
      OnStartHide?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Dialog [{this.GetType().ToString()}] OnStartHide event: {e.ToString()}"); }

    _animator.AnimateHide(AnimationSpeed);

    if (AnimationSpeed <= 0)
    {
      OnHideInvoke();
    }
    else
    {
      Invoke(nameof(OnHideInvoke), AnimationSpeed);
    }
  }

  // ===================================================================================================
  // пустая повторная функция для того чтобы можно было указать ее в качестве OnClick объекта в инспекторе
  public virtual void Open()
  {
    if (IsOpen || IsOpenProcessing)
      return;

    _dlgSystem?.AddToShowed(this);
    ShowTime = Time.unscaledTime;
    _dlgSystem?.Sort();
    Show(() =>
    {
      _dlgSystem?.OnDialogOpen?.Invoke(this);
    });
  }

  // ===================================================================================================
  public virtual void Open(Action showCallback)
  {
    if (IsOpen || IsOpenProcessing)
      return;
    
    _dlgSystem?.AddToShowed(this);
    ShowTime = Time.unscaledTime;
    _dlgSystem?.Sort();
    Show(() =>
    {
      _dlgSystem?.OnDialogOpen?.Invoke(this);
      showCallback?.Invoke();
    });
  }

  // ===================================================================================================
  public virtual bool CanBackClose()
  {
    return true;
  }

  // ===================================================================================================
  public void BackClose()
  {
    if (CanBackClose())
    {
      if (IsClose || IsCloseProcessing)
        return;

      Hide(() =>
      {
        _dlgSystem?.OnDialogClose?.Invoke(this);
      });
    }
  }

  // ===================================================================================================
  // пустая повторная функция для того чтобы можно было указать ее в качестве OnClick объекта в инспекторе
  public virtual void Close()
  {
    if (IsClose || IsCloseProcessing)
      return;
    
    Hide(() =>
    {
      _dlgSystem?.OnDialogClose?.Invoke(this);
    });
  }

  // ===================================================================================================
  public virtual void Close(Action hideCallback)
  {
    if (IsClose || IsCloseProcessing)
      return;
    
    Hide(() =>
    {
      _dlgSystem?.OnDialogClose?.Invoke(this);
      hideCallback?.Invoke();
    });
  }

  // ===================================================================================================
  public void SilentHide()
  {
    CancelInvoke();
    _animator.AnimateHide(0f);
    _openProccessing = false;
    _closeProccessing = false;
    gameObject.SetActive(false);
  }

  // ===================================================================================================
  private void OnShowInvoke()
  {
    try
    {
      _onShowCallback?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Dialog [{this.GetType().ToString()}] show callback: {e.ToString()}"); }
    try
    {
      OnShow?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Dialog [{this.GetType().ToString()}] OnShow event: {e.ToString()}"); }
    _openProccessing = false;

    //EventManager.Instance.TriggerEvent(new E_DialogOpen(Type));
  }

  // ===================================================================================================
  private void OnHideInvoke()
  {
    gameObject.SetActive(false);
    _closeProccessing = false;

    try
    {
      _onHideCallback?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Dialog [{this.GetType().ToString()}] hide callback: {e.ToString()}"); }
    try
    {
      OnHide?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Dialog [{this.GetType().ToString()}] OnHide event: {e.ToString()}"); }

    //EventManager.Instance.TriggerEvent(new E_DialogClose(Type));
  }
}
