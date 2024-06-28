using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class UIUtils
{
  public static Dictionary<string,Transform> Getters = new Dictionary<string, Transform>();

  // ===================================================================================================
  public static void SetGetter(string key, Transform pos)
  {
    if (Getters.ContainsKey(key))
    {
      Getters[key] = pos;
    }
    else
    {
      Getters.Add(key, pos);
    }
  }

  // ===================================================================================================
  public static Vector3 GetGetter(string key)
  {
    if (Getters.ContainsKey(key))
    {
      return Getters[key].position;
    }
    else if (Getters.ContainsKey("default"))
    {
      return Getters["default"].position;
    }
    else
    {
      return Vector3.zero;
    }
  }

  // ===================================================================================================
  public static Vector3 WorldToScreenPoint(this Vector3 pos)
  {
    return Global.Instance.Game.CameraController.MainCamera.WorldToScreenPoint(pos);
  }

  // ===================================================================================================
  public static void NeedConfirm(string text, string yesButton, string noButton, Action callbackYes = null, Action callbackNo = null)
  {
    DialogSystem.Instance?.Get<DialogConfirm>()?.Activate(
      text,
      yesButton,
      noButton,
      b =>
      {
        if (b)
        {
          callbackYes?.Invoke();
        }
        else
        {
          callbackNo?.Invoke();
        }
      });
  }

  // ===================================================================================================
  public static string GetHierarchyPath(GameObject main, GameObject value)
  {
    GameObject parent = value.transform.parent.gameObject;
    string res = " > " + value.name;
    while (!parent.Equals(main))
    {
      res = " > " + parent.name + res;
      parent = parent.transform.parent.gameObject;
    }
    return res;
  }

  // ===================================================================================================
  public static void UpdateLayout(this Transform xform)
  {
    
    UpdateLayout_Internal(xform);
    Canvas.ForceUpdateCanvases();
  }

  // ===================================================================================================
  private static void UpdateLayout_Internal(Transform xform)
  {
    if (xform == null || xform.Equals(null))
    {
      return;
    }

    // Update children first
    for (int x = 0; x < xform.childCount; ++x)
    {
      UpdateLayout_Internal(xform.GetChild(x));
    }

    // Update any components that might resize UI elements
    foreach (var layout in xform.GetComponents<LayoutGroup>())
    {
      layout.CalculateLayoutInputVertical();
      layout.CalculateLayoutInputHorizontal();
    }
    foreach (var fitter in xform.GetComponents<ContentSizeFitter>())
    {
      fitter.SetLayoutVertical();
      fitter.SetLayoutHorizontal();
    }
  }

  // ===================================================================================================
  private static bool IsPointerOverUIObject(Vector2 position)
  {
    PointerEventData _eventDataCurrentPosition = new PointerEventData(EventSystem.current);
    _eventDataCurrentPosition.position = new Vector2(position.x, position.y);
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(_eventDataCurrentPosition, results);
    return results.Count > 0;
  }

  // ===================================================================================================
  public static bool IsPointerOverGameObject(int pointerIndexId = -1)
  {
    if (
      Global.Instance.InpPlatform == Global.InputPlatform.Mouse ||
      Global.Instance.InpPlatform == Global.InputPlatform.Touch
      )
    {
      if (pointerIndexId >= 0)
      {
        try
        {
          if (
          EventSystem.current.IsPointerOverGameObject(Input.GetTouch(pointerIndexId).fingerId)
           || IsPointerOverUIObject(Input.mousePosition)
          )
            return true;
        }
        catch { }
      }
      else
      {
        if (
        EventSystem.current.IsPointerOverGameObject()
         || IsPointerOverUIObject(Input.mousePosition)
        )
          return true;
      }
    }
    else if (Global.Instance.InpPlatform == Global.InputPlatform.WebMultitouch)
    {
      if (
         //EventSystem.current.IsPointerOverGameObject() ||
         IsPointerOverUIObject(Input.mousePosition)
        )
        return true;
    }
    else
    {
      if (pointerIndexId >= 0)
      {
        try
        {

          if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(pointerIndexId).fingerId)
            || IsPointerOverUIObject(Input.GetTouch(pointerIndexId).position))
            return true;
        }
        catch { }        
      }
      else
      {
        foreach (var tchs in Input.touches)
        {
          if (
            EventSystem.current.IsPointerOverGameObject(tchs.fingerId)
             || IsPointerOverUIObject(tchs.position)
            )
            return true;
        }
      }        
    }
    return false;
  }

  // ===================================================================================================
  public static string GetRichBool(string name, bool value)
  {
    if (value)
    {
      return $"<b>{name}: <color=green>true</color></b>";
    }
    else
    {
      return $"<b>{name}: <color=#ff5035>false</color></b>";
    }
  }

  // ===================================================================================================
  public static string GetRichObjectIsNull(string name, object value)
  {
    if (value != null)
    {
      return $"<b>'{name} ({value.GetType().ToString()})' is <color=green>OK</color></b>";
    }
    else
    {
      return $"<b>'{name}' is <color=#ff5035>NULL</color></b>";
    }
  }
}