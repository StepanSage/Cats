using GameLogic;

public class E_GameStateChange : EventBase
{
  public EGameState OldState;
  public EGameState NewState;

  public E_GameStateChange(EGameState oldState, EGameState newState)
  {
    OldState = oldState;
    NewState = newState;
    DebugX.LogForEvents($"E_GameStateChange : SEND  {OldState} - {NewState}");
  }
}