using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
  public bool LimitQueueProcesing = false;
  public float QueueProcessTime = 0.0f;
  public delegate void EventDelegate<T>(T e) where T : EventBase;

  private Queue m_eventQueue = new Queue();
  private delegate void EventDelegate(EventBase e);

  private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();
  private Dictionary<System.Delegate, EventDelegate> delegateLookup = new Dictionary<System.Delegate, EventDelegate>();
  private Dictionary<System.Delegate, System.Delegate> onceLookups = new Dictionary<System.Delegate, System.Delegate>();

  // ===================================================================================================
  public void Init()
  {
  }

  // ===================================================================================================
  public void AddListener<T>(EventDelegate<T> del) where T : EventBase
  {
    AddDelegate<T>(del);
  }

  // ===================================================================================================
  public void AddListenerOnce<T>(EventDelegate<T> del) where T : EventBase
  {
    EventDelegate result = AddDelegate<T>(del);
    if (result != null)
    {
      onceLookups[result] = del;
    }
  }

  // ===================================================================================================
  public void RemoveListener<T>(EventDelegate<T> del) where T : EventBase
  {
    EventDelegate internalDelegate;
    if (delegateLookup.TryGetValue(del, out internalDelegate))
    {
      EventDelegate tempDel;
      if (delegates.TryGetValue(typeof (T), out tempDel))
      {
        tempDel -= internalDelegate;
        if (tempDel == null)
        {
          delegates.Remove(typeof (T));
        }
        else
        {
          delegates[typeof (T)] = tempDel;
        }
      }
      delegateLookup.Remove(del);
    }
  }

  // ===================================================================================================
  public void RemoveAll()
  {
    delegates.Clear();
    delegateLookup.Clear();
    onceLookups.Clear();
  }

  // ===================================================================================================
  public bool HasListener<T>(EventDelegate<T> del) where T : EventBase
  {
    return delegateLookup.ContainsKey(del);
  }

  // ===================================================================================================
  public void TriggerEvent(EventBase e)
  {
    EventDelegate del;
    if (delegates.TryGetValue(e.GetType(), out del))
    {
      try
      {
        del.Invoke(e);
      }
      catch (Exception exception)
      {
        Debug.LogError(exception.Message);
      }
      if (e.LogOnTriggered)
      {
        Debug.Log($"Event {e.GetType().ToString()} triggered.");
      }
      foreach (EventDelegate k in delegates[e.GetType()].GetInvocationList())
      {
        if (onceLookups.ContainsKey(k))
        {
          delegates[e.GetType()] -= k;
          if (delegates[e.GetType()] == null)
          {
            delegates.Remove(e.GetType());
          }
          delegateLookup.Remove(onceLookups[k]);
          onceLookups.Remove(k);
        }
      }
    }
  }

  // ===================================================================================================
  public bool QueueEvent(EventBase evt)
  {
    if (!delegates.ContainsKey(evt.GetType()))
      return false;

    m_eventQueue.Enqueue(evt);
    return true;
  }

  // ===================================================================================================
  private EventDelegate AddDelegate<T>(EventDelegate<T> del) where T : EventBase
  {
    if (delegateLookup.ContainsKey(del))
      return null;
    EventDelegate internalDelegate = (e) => del((T)e);
    delegateLookup[del] = internalDelegate;
    EventDelegate tempDel;
    if (delegates.TryGetValue(typeof(T), out tempDel))
    {
      delegates[typeof(T)] = tempDel += internalDelegate;
    }
    else
    {
      delegates[typeof(T)] = internalDelegate;
    }
    return internalDelegate;
  }

  // ===================================================================================================
  void Update()
  {
    float timer = 0.0f;
    while (m_eventQueue.Count > 0)
    {
      if (LimitQueueProcesing)
      {
        if (timer > QueueProcessTime)
          return;
      }
      EventBase evt = m_eventQueue.Dequeue() as EventBase;
      TriggerEvent(evt);
      if (LimitQueueProcesing)
        timer += Time.deltaTime;
    }
  }
}