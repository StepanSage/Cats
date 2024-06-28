using System;
using Objects;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class Panel : MonoBehaviour
{
  public string Name => this.GetType().ToString();
  public Type Type => this.GetType();

  [SerializeField]
  private PanelAnimator _animator = null;
  public byte Group => _group;

  protected byte _group = (byte) SortingGroup.Default;
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

  private PanelSystem _pnlSystem;
  private bool _inited = false;
  private bool _openProccessing = false;
  private bool _closeProccessing = false;
  private Action _onShowCallback;
  private Action _onHideCallback;
  private AudioClip _openSound;

  // ===================================================================================================
  public virtual void Init(PanelSystem owner)
  {
    if (_inited)
      return;

    _pnlSystem = owner;

    _openProccessing = false;
    _closeProccessing = false;
    gameObject.SetActive(false);
    _animator.Init();

    _openSound = Factory.Instance.GetSound("popup");

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
      catch (Exception e) { Debug.LogError($"Error in Panel [{this.GetType().ToString()}] show callback: {e.ToString()}"); }
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
    catch (Exception e) { Debug.LogError($"Error in Panel [{this.GetType().ToString()}] OnStartShow event: {e.ToString()}"); }

    if (!gameObject.activeSelf)
      gameObject.SetActive(true);

    _animator.AnimateShow(AnimationSpeed);

    if (AnimationSpeed <= 0)
    {
      OnShowInvoke();
    }
    else
    {
      Invoke("OnShowInvoke", AnimationSpeed);
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
      catch (Exception e) { Debug.LogError($"Error in Panel [{this.GetType().ToString()}] hide callback: {e.ToString()}"); }
      return;
    }

    CancelInvoke();

    _openProccessing = false;
    _closeProccessing = true;

    try
    {
      OnStartHide?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Panel [{this.GetType().ToString()}] OnStartHide event: {e.ToString()}"); }

    _animator.AnimateHide(AnimationSpeed);

    if (AnimationSpeed <= 0)
    {
      OnHideInvoke();
    }
    else
    {
      Invoke("OnHideInvoke", AnimationSpeed);
    }
  }

  // ===================================================================================================
  public void Open()
  {
    if (IsOpen || IsOpenProcessing)
      return;
    
    _pnlSystem?.AddToShowed(this);
    ShowTime = Time.unscaledTime;
    Show(() =>
    {
      _pnlSystem?.OnPanelOpen?.Invoke(this);
    });
  }

  // ===================================================================================================
  public void Open(Action showCallback)
  {
    if (IsOpen || IsOpenProcessing)
      return;
    
    _pnlSystem?.AddToShowed(this);
    ShowTime = Time.unscaledTime;

    Show(() =>
    {
      _pnlSystem?.OnPanelOpen?.Invoke(this);
      showCallback?.Invoke();
    });
  }

  // ===================================================================================================
  public void Close()
  {
    if (IsClose || IsCloseProcessing)
      return;
    
    Hide(() =>
    {
      _pnlSystem?.OnPanelClose?.Invoke(this);
    });
  }

  // ===================================================================================================
  public void Close(Action hideCallback)
  {
    if (IsClose || IsCloseProcessing)
      return;
    
    Hide(() =>
    {
      _pnlSystem?.OnPanelClose?.Invoke(this);
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
    catch (Exception e) { Debug.LogError($"Error in Panel [{this.GetType().ToString()}] show callback: {e.ToString()}"); }
    try
    {
      OnShow?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Panel [{this.GetType().ToString()}] OnShow event: {e.ToString()}"); }
    _openProccessing = false;
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
    catch (Exception e) { Debug.LogError($"Error in Panel [{this.GetType().ToString()}] hide callback: {e.ToString()}"); }
    try
    {
      OnHide?.Invoke();
    }
    catch (Exception e) { Debug.LogError($"Error in Panel [{this.GetType().ToString()}] OnHide event: {e.ToString()}"); }
  }
}
