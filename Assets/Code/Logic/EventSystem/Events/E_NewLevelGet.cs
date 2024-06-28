public class E_NewLevelGet : EventBase
{
  public int Level { get; set; }

  public E_NewLevelGet(int level) 
  {
    DebugX.LogForEvents($"E_NewLevelGet : SEND");
    Level = level;
  }
}