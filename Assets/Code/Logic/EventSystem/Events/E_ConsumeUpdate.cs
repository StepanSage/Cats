using Static;

public class E_ConsumeUpdate : EventBase
{
  public Consume Consume; 
  
  public E_ConsumeUpdate(Consume consume) 
  {
    DebugX.LogForEvents($"E_ConsumeUpdate : SEND  {consume.Type} - {consume.Amount}");

    Consume = consume;
  }
}