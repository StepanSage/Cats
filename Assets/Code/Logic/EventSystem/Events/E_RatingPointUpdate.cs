using Static;

public class E_RatingPointUpdate : EventBase
{
  public Consume Consume;

  public E_RatingPointUpdate(Consume consume)
  {
    DebugX.LogForEvents($"E_RatingPointUpdate : SEND");
    Consume = consume;
  }
}