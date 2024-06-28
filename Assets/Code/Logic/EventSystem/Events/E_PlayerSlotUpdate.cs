using Player;

public class E_PlayerSlotUpdate : EventBase
{
  public PlayerSlot PlayerSlot;

  public E_PlayerSlotUpdate(PlayerSlot playerSlot) 
  {
    DebugX.LogForEvents($"E_PlayerSlotUpdate : SEND");
    PlayerSlot = playerSlot;
  }
}