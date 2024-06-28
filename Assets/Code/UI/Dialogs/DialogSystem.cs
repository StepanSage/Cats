using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Objects;
using UnityEngine;
using UnityEngine.Events;

public enum SortingGroup
{
  Bottom = byte.MinValue,
  Default = 20,
  Top = 20,
  MaxCommonTop = byte.MaxValue
}

public enum DialogMode
{
  Normal,
  Notify
}

public class DialogSystem : Singleton<DialogSystem>
{
  public UnityEvent<Dialog> OnDialogClose = new UnityEvent<Dialog>();
  public UnityEvent<Dialog> OnDialogOpen = new UnityEvent<Dialog>();
  public UnityEvent OnApplicationTryExit = new UnityEvent();

  public int OpenedCount => _showedDialogs.Count(dlg => dlg.gameObject.activeSelf && (dlg.IsOpen || dlg.IsOpenProcessing));

  [SerializeField]
  private GameObject _container = null;
  [SerializeField]
  private GameObject _containerMax = null;
  [SerializeField, HideInInspector]
  private List<Dialog> _dialogs = new List<Dialog>();

  private List<Dialog> _showedDialogs = new List<Dialog>();
  private static readonly List<Type> _ignoreCloseAll = new List<Type>
  {
    //typeof(),
  };

  // ===================================================================================================
  public void Sort()
  {
    _showedDialogs = _showedDialogs.OrderBy(dlg => dlg.Group).ThenBy(dlg => dlg.ShowTime).ToList();
    foreach (var shwDlg in _showedDialogs)
    {
      if (shwDlg.Group >= (byte)SortingGroup.MaxCommonTop)
      {
        continue;
      }
      shwDlg.transform.SetAsLastSibling();
    }
  }

  // ===================================================================================================
  public void AddToShowed(Dialog dialog)
  {
    if (_showedDialogs.Any(dlg => dlg.Equals(dialog)))
    {
      return;
    }
    _showedDialogs.Add(dialog);
  }

  // ===================================================================================================
  public bool Has<T>() where T : Dialog
  {
    return _dialogs.Any(dlg => dlg.GetType().Name.Equals(typeof(T).Name));
  }

  // ===================================================================================================
  [CanBeNull]
  public T Get<T>() where T : Dialog
  {
    Dialog res = null;
    // если диалог еще не создан на сцене
    if (!_dialogs.Any(dlg => dlg.GetType().Name.Equals(typeof(T).Name)))
    {
      GameObject dialog = Factory.Instance.GetDialog(typeof(T).Name);
      var dialogScript = dialog.GetComponent<Dialog>();
      if (dialogScript != null)
      {
        res = dialogScript;
        // создаем диалог и добавляем в список созданных
        dialog.transform.SetParent(
          res.Group >= (byte) SortingGroup.MaxCommonTop ? 
            _containerMax.transform : 
            _container.transform, 
          false);
        // сбразываем позиции на default-ные
        dialog.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        var rect = dialog.transform.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        // запоминаем в созданных, чтобы не пересоздавать в дальнейшем
        _dialogs.Add(dialogScript);
        dialogScript.Init(Instance);
        dialogScript.SilentHide();
      }
      else
      {
        Debug.LogWarning($"[{typeof(T).ToString()}]: dialog script not found on dialog prefab.");
        Destroy(dialog);
        return null;
      }
    }
    else
    {
      res = _dialogs.Find(dlg => dlg.GetType().Name.Equals(typeof(T).Name));
    }
    return (T) res;
  }

  // ===================================================================================================
  public void Open<T>(Action showCallback = null) where T : Dialog
  {
    Dialog res = null;
    // если диалог еще не создан на сцене
    if (!_dialogs.Any(dlg => dlg.GetType().Name.Equals(typeof(T).Name)))
    {
      GameObject dialog = Factory.Instance.GetDialog(typeof(T).Name);
      var dialogScript = dialog.GetComponent<Dialog>();
      if (dialogScript != null)
      {
        res = dialogScript;
        // создаем диалог и добавляем в список созданных
        dialog.transform.SetParent(
          res.Group >= (byte)SortingGroup.MaxCommonTop ?
            _containerMax.transform :
            _container.transform,
          false);
        // сбразываем позиции на default-ные
        dialog.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        var rect = dialog.transform.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        // запоминаем в созданных, чтобы не пересоздавать в дальнейшем
        _dialogs.Add(dialogScript);
        dialogScript.Init(this);
      }
      else
      {
        Debug.LogWarning($"[{typeof(T).GetType().ToString()}]: dialog script not found on dialog prefab.");
        Destroy(dialog);
        return;
      }
    }
    else
    {
      res = _dialogs.Find(dlg => dlg.GetType().Name.Equals(typeof(T).Name));
    }

    _showedDialogs.RemoveAll(dlg => dlg.IsClose || dlg.IsCloseProcessing);
    if (res.Group >= (byte) SortingGroup.MaxCommonTop)
    {
      res.transform.SetAsLastSibling();
    }
    
    if (!_showedDialogs.Any(dlg => dlg.GetType().Name.Equals(typeof(T).Name)))
    {
      _showedDialogs.Add(res);
      res.ShowTime = Time.unscaledTime;
     
      Sort();
      bool invokeOpen = !(res.IsOpen || res.IsOpenProcessing);
      res.Show(() =>
      {
        if (invokeOpen)
        {
          try
          {
            OnDialogOpen?.Invoke(res);
          }
          catch { }
        }
        showCallback?.Invoke();
      });
    }
  }

  // ===================================================================================================
  public void Close<T>(Action hideCallback = null, bool silent = false) where T : Dialog
  {
    var res = _dialogs.Find(dlg => dlg.GetType().Name.Equals(typeof(T).Name));
    if (res == null)
      return;
    bool invokeClose = !(res.IsClose || res.IsCloseProcessing);
    if (silent)
    {
      res.SilentHide();
      if (invokeClose)
      {
        try
        {
          OnDialogClose?.Invoke(res);
        }
        catch { }
      }
    }
    else
    {
      res.Hide(() =>
      {
        if (invokeClose)
        {
          try
          {
            OnDialogClose?.Invoke(res);
          }
          catch { }
        }
        hideCallback?.Invoke();
      });
    }
  }

  // ===================================================================================================
  public void CloseAll(bool silent = false, List<Type> ignoreList = null)
  {
    if (OpenedCount <= 0)
      return;

    foreach (var dlg in _dialogs)
    {
      if (_ignoreCloseAll.Contains(dlg.GetType()))
        continue;
      if (ignoreList != null && ignoreList.Contains(dlg.GetType()))
        continue;
      
      bool invokeClose = !(dlg.IsClose || dlg.IsCloseProcessing);
      if (silent)
      {
        dlg.SilentHide();
        if (invokeClose)
        {
          try
          {
            OnDialogClose?.Invoke(dlg);
          }
          catch { }
        }
      }
      else
      {
        dlg.Hide(() =>
        {
          if (invokeClose)
          {
            try
            {
              OnDialogClose?.Invoke(dlg);
            }
            catch { }
          }
        });
      }
    }
  }

  // ===================================================================================================
  public void CloseLast(bool silent = false)
  {
    if (!_showedDialogs.Any())
      return;
    var lastDlg = _showedDialogs.TakeLast(1).First();
    bool invokeClose = !(lastDlg.IsClose || lastDlg.IsCloseProcessing);
    if (silent)
    {
      lastDlg.SilentHide();
      if (invokeClose)
      {
        try
        {
          OnDialogClose?.Invoke(lastDlg);
        }
        catch { }
      }
    }
    else
    {
      lastDlg.Hide(() =>
      {
        if (invokeClose)
        {
          try
          {
            OnDialogClose?.Invoke(lastDlg);
          }
          catch { }
        }
      });
    }
  }

  // ===================================================================================================
  public void BackCloseLast()
  {
    _showedDialogs.RemoveAll(dlg => dlg.IsClose || dlg.IsCloseProcessing);
    
    if (!_showedDialogs.Any())
    {
      OnApplicationTryExit?.Invoke();
      return;
    }
    
    var lastDlg = _showedDialogs.TakeLast(1).First();
    lastDlg.BackClose();
  }
}
