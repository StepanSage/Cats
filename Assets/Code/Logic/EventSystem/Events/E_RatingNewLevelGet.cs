public class E_RatingNewLevelGet : EventBase
{
  public int Level { get; set; }

  public E_RatingNewLevelGet(int level) 
  {
    DebugX.LogForEvents($"E_RatingNewLevelGet : SEND");
    Level = level;
  }
}