using System;

public class Timer
{
  public event Action Update;
  public double StartTime => _startTime;
  public float  Duration => _duration;

  private double _startTime;
  private float _duration;
  private bool _isActive = false;
  private bool _loop = false;

  // ===================================================================================================
  public Timer(float duration, bool loop = false)
  {
    _duration = duration;
    _loop = loop;
  }

  // ===================================================================================================
  public Timer() { }

  // ===================================================================================================
  public void Init(float duration, bool loop = false)
  {
    _duration = duration;
    _loop = loop;
  }

  // ===================================================================================================
  public void Start()
  {
    _startTime = Global.Instance.Time;
    _isActive = true;
  }

  // ===================================================================================================
  public void Stop()
  {
    _isActive = false;
  }

  // ===================================================================================================
  public bool IsActive()
  {
    return _isActive;
  }

  // ===================================================================================================
  public void Restart()
  {
    _startTime = Global.Instance.Time;
  }

  // ===================================================================================================
  public void Tick()
  {
    if (_isActive && this.IsExpired())
    {
      this.Update?.Invoke();
      if (_loop)
      {
        _startTime = Global.Instance.Time;
      }
      else
      {
        Stop();
      }
    }
  }

}
