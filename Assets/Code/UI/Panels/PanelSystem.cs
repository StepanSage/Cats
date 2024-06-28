using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Objects;
using UnityEngine;
using UnityEngine.Events;

public class PanelSystem : Singleton<PanelSystem>
{
  public UnityEvent<Panel> OnPanelClose = new UnityEvent<Panel>();
  public UnityEvent<Panel> OnPanelOpen = new UnityEvent<Panel>();

  public int OpenedCount => _showedPanels.Count(dlg => dlg.gameObject.activeSelf);

  [SerializeField]
  private GameObject _container = null;
  [SerializeField]
  private GameObject _containerMax = null;
  [SerializeField, HideInInspector]
  private List<Panel> _panels = new List<Panel>();
  private List<Panel> _showedPanels = new List<Panel>();
  private static readonly List<Type> _ignoreCloseAll = new List<Type> { };

  // ===================================================================================================
  public void Sort()
  {
    // ��������� ������ �������� ����, �� ������ � �� ������� �������� ���� (������ �� ������ ���� ���� ������ ����������)
    _showedPanels = _showedPanels.OrderBy(pnl => pnl.Group).ThenBy(pnl => pnl.ShowTime).ToList();
    foreach (var shwPnl in _showedPanels)
    {
      if (shwPnl.Group >= (byte)SortingGroup.MaxCommonTop)
      {
        continue;
      }
      shwPnl.transform.SetAsLastSibling();
    }
  }

  // ===================================================================================================
  public void AddToShowed(Panel panel)
  {
    if (_showedPanels.Any(pnl => pnl.Equals(panel)))
    {
      return;
    }
    _showedPanels.Add(panel);
  }

  // ===================================================================================================
  public bool Has<T>() where T : Panel
  {
    return _panels.Any(dlg => dlg.GetType().Name.Equals(typeof(T).Name));
  }

  // ===================================================================================================
  [CanBeNull]
  public T Get<T>() where T : Panel
  {
    Panel res = null;
    // ���� ������ ��� �� ������ �� �����
    if (!_panels.Any(pnl => pnl.GetType().Name.Equals(typeof(T).Name)))
    {
      var panel = Factory.Instance.GetPanel(typeof(T).Name);
      var panelScript = panel.GetComponent<Panel>();
      if (panelScript != null)
      {
        res = panelScript;
        // ������� ������ � ��������� � ������ ���������
        panel.transform.SetParent(
          res.Group >= (byte)SortingGroup.MaxCommonTop ?
            _containerMax.transform :
            _container.transform,
          false);
        // ���������� ������� �� default-���
        panel.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        var rect = panel.transform.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        // ���������� � ���������, ����� �� ������������� � ����������
        _panels.Add(panelScript);
        panelScript.Init(Instance);
        panelScript.SilentHide();
      }
      else
      {
        Debug.LogWarning($"[{typeof(T).ToString()}]: panel script not found on panel prefab.");
        Destroy(panel);
        return null;
      }
    }
    else
    {
      res = _panels.Find(pnl => pnl.GetType().Name.Equals(typeof(T).Name));
    }
    return (T)res;
  }

  // ===================================================================================================
  public void Open<T>(Action showCallback = null) where T : Panel
  {
    Panel res = null;
    // ���� ������ ��� �� ������ �� �����
    if (!_panels.Any(pnl => pnl.GetType().Name.Equals(typeof(T).Name)))
    {
      var panel = Factory.Instance.GetPanel(typeof(T).Name);
      var panelScript = panel.GetComponent<Panel>();
      if (panelScript != null)
      {
        res = panelScript;
        // ������� ������ � ��������� � ������ ���������
        panel.transform.SetParent(
          res.Group >= (byte)SortingGroup.MaxCommonTop ?
            _containerMax.transform :
            _container.transform,
          false);
        // ���������� ������� �� default-���
        panel.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        var rect = panel.transform.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        // ���������� � ���������, ����� �� ������������� � ����������
        _panels.Add(panelScript);
        panelScript.Init(this);
      }
      else
      {
        Debug.LogWarning($"[{typeof(T).GetType().ToString()}]: dialog script not found on dialog prefab.");
        Destroy(panel);
        return;
      }
    }
    else
    {
      res = _panels.Find(pnl => pnl.GetType().Name.Equals(typeof(T).Name));
    }

    // ������� �� ������ ���� ���� �������
    _showedPanels.RemoveAll(pnl => pnl.IsClose || pnl.IsCloseProcessing);

    // ���������� ������ ������ �� ����� ����� ������ �� �������� �� ���� ������� �� ��� ���
    if (res.Group >= (byte)SortingGroup.MaxCommonTop)
    {
      res.transform.SetAsLastSibling();
    }

    // ���� ��� ��� � ������ ���������
    if (!_showedPanels.Any(pnl => pnl.GetType().Name.Equals(typeof(T).Name)))
    {
      _showedPanels.Add(res);
      res.ShowTime = Time.unscaledTime;
      // ���������
      bool invokeOpen = !(res.IsOpen || res.IsOpenProcessing);
      // ���������� ������
      res.Show(() =>
      {
        if (invokeOpen)
        {
          try
          {
            OnPanelOpen?.Invoke(res);
          }
          catch { }
        }
        showCallback?.Invoke();
      });
    }
  }

  // ===================================================================================================
  public void Close<T>(Action hideCallback = null, bool silent = false) where T : Panel
  {
    var res = _panels.Find(dlg => dlg.GetType().Name.Equals(typeof(T).Name));
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
          OnPanelClose?.Invoke(res);
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
            OnPanelClose?.Invoke(res);
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

    foreach (var pnl in _panels)
    {
      if (_ignoreCloseAll.Contains(pnl.GetType()))
        continue;
      if (ignoreList != null && ignoreList.Contains(pnl.GetType()))
        continue;
      
      bool invokeClose = !(pnl.IsClose || pnl.IsCloseProcessing);
      if (silent)
      {
        pnl.SilentHide();
        if (invokeClose)
        {
          try
          {
            OnPanelClose?.Invoke(pnl);
          }
          catch { }
        }
      }
      else
      {
        pnl.Hide(() =>
        {
          if (invokeClose)
          {
            try
            {
              OnPanelClose?.Invoke(pnl);
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
    if (!_showedPanels.Any())
      return;
    var last = _showedPanels.TakeLast(1).First();
    bool invokeClose = !(last.IsClose || last.IsCloseProcessing);
    if (silent)
    {
      last.SilentHide();
      if (invokeClose)
      {
        try
        {
          OnPanelClose?.Invoke(last);
        }
        catch { }
      }
    }
    else
    {
      last.Hide(() =>
      {
        if (invokeClose)
        {
          try
          {
            OnPanelClose?.Invoke(last);
          }
          catch { }
        }
      });
    }
  }
}
