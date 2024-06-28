using Static;

public class E_ExpUpdate : EventBase
{
  public Consume Consume;

  public E_ExpUpdate(Consume consume)
  {
    DebugX.LogForEvents($"E_GoldUpdate : SEND");
    Consume = consume;
  }
}