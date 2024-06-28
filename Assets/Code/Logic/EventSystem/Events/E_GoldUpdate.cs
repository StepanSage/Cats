using Static;

public class E_GoldUpdate : EventBase
{
  public Consume Consume;

  public E_GoldUpdate(Consume consume)
  {
    DebugX.LogForEvents($"E_GoldUpdate : SEND");
    Consume = consume;
  }
}