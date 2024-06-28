using Static;

public class E_CrystalUpdate : EventBase
{
  public Consume Consume;

  public E_CrystalUpdate(Consume consume)
  {
    DebugX.LogForEvents($"E_CrystalUpdate : SEND");
    Consume = consume;
  }
}